// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable
#pragma warning disable IDE0034
#pragma warning disable IDE0052

namespace Chromely.NativeHosts;

public partial class ChromelyMacHost : IChromelyNativeHost
{
    protected readonly IChromelyConfiguration _config;

    public event EventHandler<CreatedEventArgs> HostCreated;
    public event EventHandler<MovingEventArgs> HostMoving;
    public event EventHandler<SizeChangedEventArgs> HostSizeChanged;
    public event EventHandler<CloseEventArgs> HostClose;

    private delegate void RunMessageLoopCallback();
    private delegate void CefShutdownCallback();
    private delegate void InitCallbackEvent(IntPtr app, IntPtr pool);
    private delegate void CreateCallbackEvent(IntPtr window, IntPtr view);
    private delegate void MovingCallbackEvent();
    private delegate void ResizeCallbackEvent(int width, int height);
    private delegate int CloseBrowserEvent();
    private delegate void QuitCallbackEvent();

    private ChromelyParam _configParam;
    private IWindowOptions _options;
    private GCHandle _titleHandle;
    private IntPtr _appHandle;
    private IntPtr _poolHandle;
    private IntPtr _windowHandle;
    private IntPtr _viewHandle;
    private bool _isInitialized;

    public ChromelyMacHost(IChromelyConfiguration config)
    {
        _config = config;
        _options = _config?.WindowOptions ?? new WindowOptions();
        _appHandle = IntPtr.Zero;
        _poolHandle = IntPtr.Zero;
        _windowHandle = IntPtr.Zero;
        _viewHandle = IntPtr.Zero;
        _isInitialized = false;
    }
    public virtual IntPtr Handle => _windowHandle;

    public virtual void CreateWindow()
    {
        _configParam = InitParam(RunCallback,
                                                ShutdownCallback,
                                                InitCallback,
                                                CreateCallback,
                                                MovingCallback,
                                                ResizeCallback,
                                                CloseBrowser,
                                                QuitCallback);


        _configParam.centerscreen = _options.WindowState == WindowState.Normal && _options.StartCentered ? 1 : 0;
        _configParam.frameless = _options.WindowFrameless ? 1 : 0;
        _configParam.fullscreen = _options.WindowState == Core.Host.WindowState.Fullscreen ? 1 : 0;
        _configParam.noresize = _options.DisableResizing ? 1 : 0;
        _configParam.nominbutton = _options.DisableMinMaximizeControls ? 1 : 0;
        _configParam.nomaxbutton = _options.DisableMinMaximizeControls ? 1 : 0;

        _configParam.titleUtf8Ptr = GetUTF8Title(_options.Title);

        _configParam.x = _options.Position.X;
        _configParam.y = _options.Position.Y;
        _configParam.width = _options.Size.Width;
        _configParam.height = _options.Size.Height;

        createwindow(ref _configParam);
    }

    public virtual IntPtr GetNativeHandle()
    {
        return _viewHandle;
    }

    public virtual void Run()
    {
    }

    public virtual Size GetWindowClientSize()
    {
        return new Size();
    }

    public virtual float GetWindowDpiScale()
    {
        return 1.0f;
    }

    /// <summary> Gets the current window state Maximised / Normal / Minimised etc. </summary>
    /// <returns> The window state. </returns>
    public virtual WindowState GetWindowState()
    {
        // TODO required for frameless Maccocoa mode
        return WindowState.Normal;
    }

    /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
    /// <param name="state"> The state to set. </param>
    /// <returns> True if it succeeds, false if it fails. </returns>
    public virtual bool SetWindowState(WindowState state)
    {
        // TODO required for frameless Maccocoa mode
        return false;
    }

    public virtual void SetupMessageInterceptor(IntPtr browserWindowHandle)
    {
    }

    public virtual void ResizeBrowser(IntPtr browserWindow, int width, int height)
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void SetWindowTitle(string title)
    {
        Logger.Instance.Log.LogInformation("ChromelyMacHost::SetWindowTitle is not implemented yet!");
    }


    public virtual void ToggleFullscreen(IntPtr hWnd)
    {
    }

    #region Create & Manage Window

    protected virtual void RunCallback()
    {
        try
        {
            CefRuntime.RunMessageLoop();
            CefRuntime.Shutdown();
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in ChromelyMacHost::RunCallback");
            Logger.Instance.Log.LogError(exception, exception.Message);
        }
    }

    protected virtual void ShutdownCallback()
    {
        CefRuntime.QuitMessageLoop();
    }

    protected virtual void InitCallback(IntPtr app, IntPtr pool)
    {
        _appHandle = app;
        _poolHandle = pool;
    }

    protected virtual void CreateCallback(IntPtr window, IntPtr view)
    {
        _titleHandle.Free();
        _windowHandle = window;
        _viewHandle = view;
        var createdEvent = new CreatedEventArgs(_windowHandle, _viewHandle);
        HostCreated?.Invoke(this, createdEvent);
        _isInitialized = true;
    }

    protected virtual void MovingCallback()
    {
        if (_viewHandle != IntPtr.Zero && _isInitialized)
        {
            HostMoving?.Invoke(this, new MovingEventArgs());
        }
    }

    protected virtual void ResizeCallback(int width, int height)
    {
        if (_viewHandle != IntPtr.Zero && _isInitialized)
        {
            HostSizeChanged?.Invoke(this, new SizeChangedEventArgs(width, height));
        }
    }

    protected virtual int CloseBrowser()
    {
        try
        {
            var browser = _config?.JavaScriptExecutor?.GetBrowser() as CefBrowser;
            if (browser != null)
            {
                var host = browser.GetHost();
                if (host != null && !host.TryCloseBrowser())
                {
                    return 1;
                }
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in ChromelyMacHost::CloseBrowser");
            Logger.Instance.Log.LogError(exception, exception.Message);
        }

        return 0;
    }

    protected virtual void QuitCallback()
    {
        try
        {
            CefRuntime.QuitMessageLoop();
            HostClose?.Invoke(this, new CloseEventArgs());
            Environment.Exit(0);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in ChromelyMacHost::QuitCallback");
            Logger.Instance.Log.LogError(exception);
        }
    }

    private static ChromelyParam InitParam(RunMessageLoopCallback runCallback,
                                                CefShutdownCallback cefShutdownCallback,
                                                InitCallbackEvent initCallback,
                                                CreateCallbackEvent createCallback,
                                                MovingCallbackEvent movingCallback,
                                                ResizeCallbackEvent resizeCallback,
                                                CloseBrowserEvent closeBrowserCallback,
                                                QuitCallbackEvent quitCallback)
    {

        ChromelyParam configParam = new();
        configParam.runMessageLoopCallback = Marshal.GetFunctionPointerForDelegate(runCallback);
        configParam.cefShutdownCallback = Marshal.GetFunctionPointerForDelegate(cefShutdownCallback);
        configParam.initCallback = Marshal.GetFunctionPointerForDelegate(initCallback);
        configParam.createCallback = Marshal.GetFunctionPointerForDelegate(createCallback);
        configParam.movingCallback = Marshal.GetFunctionPointerForDelegate(movingCallback);
        configParam.resizeCallback = Marshal.GetFunctionPointerForDelegate(resizeCallback);
        configParam.closeBrowserCallback = Marshal.GetFunctionPointerForDelegate(closeBrowserCallback);
        configParam.exitCallback = Marshal.GetFunctionPointerForDelegate(quitCallback);

        return configParam;
    }

    #endregion

    #region Title Encoding
    private static bool TitleNonASCIIChars(string title)
    {
        return (System.Text.Encoding.UTF8.GetByteCount(title) != title.Length);
    }

    private IntPtr GetUTF8Title(string title)
    {
        try
        {
            if (TitleNonASCIIChars(title))
            {
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(title);
                _titleHandle = GCHandle.Alloc(utf8Bytes, GCHandleType.Pinned);
                return _titleHandle.AddrOfPinnedObject();
            }

            byte[] bytes = Encoding.ASCII.GetBytes(title);
            _titleHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            return _titleHandle.AddrOfPinnedObject();
        }
        catch { }

        return IntPtr.Zero;
    }

    #endregion
}