// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable
#pragma warning disable CA1806
#pragma warning disable IDE0034

namespace Chromely.NativeHosts;

public partial class ChromelyLinuxHost : IChromelyNativeHost
{
    // X-Window error codes
    // https://tronche.com/gui/x/xlib/event-handling/protocol-errors/default-handlers.html

    public event EventHandler<CreatedEventArgs> HostCreated;
    public event EventHandler<MovingEventArgs> HostMoving;
    public event EventHandler<SizeChangedEventArgs> HostSizeChanged;
    public event EventHandler<CloseEventArgs> HostClose;

    private IWindowOptions _options;
    private IntPtr _handle;
    private IntPtr _xid;
    private bool _isInitialized;
    private bool _debugging;

    private readonly RealizeCallback _onRealizedDelegate;
    private readonly SizeAllocateCallback _onSizeAllocateDelegate;
    private readonly ResizeCallback _onResizeDelegate;
    private readonly DestroyCallback _onDestroyDelegate;

    private IntPtr _onRealizedSignal;
    private IntPtr _onSizeAllocateSignal;
    private IntPtr _onResizeSignal;
    private IntPtr _onDestroySignal;

    private readonly GClosureNotify _onFreeNotify;

    private readonly XHandleXError _onHandleErrorDelegate;
    private readonly XHandleXIOError _onHandleIOErrorDelegate;

    public ChromelyLinuxHost()
    {
        gdk_set_allowed_backends("x11");

        _isInitialized = false;
        _handle = IntPtr.Zero;
        _xid = IntPtr.Zero;

        _onRealizedDelegate = new RealizeCallback(OnRealized);
        _onSizeAllocateDelegate = new SizeAllocateCallback(OnSizeAllocate);
        _onResizeDelegate = new ResizeCallback(OnResize);
        _onDestroyDelegate = new DestroyCallback(OnDestroy);

        _onFreeNotify = new GClosureNotify(FreeData);

        _onHandleErrorDelegate = new XHandleXError(HandleError);
        _onHandleIOErrorDelegate = new XHandleXIOError(HandleIOError);
    }

    public virtual IntPtr Handle => _handle;

    public virtual void CreateWindow(IWindowOptions config, bool debugging)
    {
        _options = config;
        _debugging = debugging;

        Init(0, null);

        var wndType = _options.WindowFrameless
            ? GtkWindowType.GtkWindowPopup
            : GtkWindowType.GtkWindowToplevel;

        _handle = CreateNewWindow((int)wndType);

        SetWindowTitle(_options.Title);
        SetAppIcon(_handle, _options.RelativePathToIconFile);
        SetWindowDefaultSize(_options.Size.Width, _options.Size.Height);

        if (_options.WindowState == WindowState.Normal && _options.StartCentered)
        {
            SetWindowPosistion((int)GtkWindowPosition.GtkWinPosCenter);
        }

        switch (_options.WindowState)
        {
            case WindowState.Normal:
                break;

            case WindowState.Maximize:
                SetWindowMaximize();
                break;

            case WindowState.Fullscreen:
                SetFullscreen();
                break;
        }

        ConnectRealizeSignal(_onRealizedDelegate, _onFreeNotify);
        ConnectSizeAllocateSignal(_onSizeAllocateDelegate, _onFreeNotify);
        ConnectResizeSignal(_onResizeDelegate, _onFreeNotify);
        ConnectDestroySignal(_onDestroyDelegate, _onFreeNotify);

        SetDefaultWindowVisual(_handle);

        ShowWindow();
    }

    public virtual IntPtr GetNativeHandle()
    {
        try
        {
            IntPtr gdkHandle = gtk_widget_get_window(_handle);
            Utils.AssertNotNull("GetNativeHandle:gtk_widget_get_window", gdkHandle);
            IntPtr xid = gdk_x11_window_get_xid(gdkHandle);
            Utils.AssertNotNull("GetNativeHandle:gdk_x11_window_get_xid", xid);
            return xid;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::GetNativeHandle");
            Logger.Instance.Log.LogError(exception);
        }

        return IntPtr.Zero;
    }

    public virtual void Run()
    {
        try
        {
            CefRuntime.RunMessageLoop();
            CefRuntime.Shutdown();
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::Run");
            Logger.Instance.Log.LogError(exception);
        }
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
        // TODO required for frameless linux / gtk3 mode
        return WindowState.Normal;
    }

    /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
    /// <param name="state"> The state to set. </param>
    /// <returns> True if it succeeds, false if it fails. </returns>
    public virtual bool SetWindowState(WindowState state)
    {
        // TODO required for frameless linux / gtk3 mode
        return false;
    }

    public virtual void SetupMessageInterceptor(IntPtr browserWindowHandle)
    {
    }

    public virtual void ResizeBrowser(IntPtr browserWindow, int width, int height)
    {
        try
        {
            IntPtr gdkDisplay = gtk_widget_get_display(_handle);
            Utils.AssertNotNull("ResizeBrowser:gtk_widget_get_display", gdkDisplay);
            IntPtr x11Display = gdk_x11_display_get_xdisplay(gdkDisplay);
            Utils.AssertNotNull("ResizeBrowser:gdk_x11_display_get_xdisplay", x11Display);
            XMoveResizeWindow(x11Display, browserWindow, 0, 0, width, height);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetWindowMaximize");
            Logger.Instance.Log.LogError(exception);
        }
    }

    public virtual void Exit()
    {
        try
        {
            CefRuntime.QuitMessageLoop();
            gtk_main_quit();
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::Quit");
            Logger.Instance.Log.LogError(exception);
        }
    }

    public virtual void SetWindowTitle(string title)
    {
        try
        {
            gtk_window_set_title(_handle, title);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetWindowTitle");
            Logger.Instance.Log.LogError(exception);
        }
    }

    public virtual void ToggleFullscreen(IntPtr hWnd)
    {
    }

    protected virtual void SetWindowMaximize()
    {
        try
        {
            gtk_window_maximize(_handle);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetWindowMaximize");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void SetFullscreen()
    {
        try
        {
            gtk_window_fullscreen(_handle);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetFullscreen");
            Logger.Instance.Log.LogError(exception);
        }
    }


    #region CreateWindow

    protected delegate void SizeAllocateCallback(IntPtr window, IntPtr allocation, int baseline);
    protected delegate bool ResizeCallback(IntPtr window, GtkEvent gtkEvent, IntPtr data);
    protected delegate void RealizeCallback(IntPtr window);
    protected delegate bool DestroyCallback(IntPtr window, IntPtr data);
    protected delegate void QuitCallback();

    protected delegate short HandleErrorCallback(IntPtr display, ref XErrorEvent errorEven);
    protected delegate short HandleIOErrorCallback(IntPtr d);

    protected virtual void Init(int argc, string[] argv)
    {
        try
        {
            gtk_init(argc, argv);
            InstallX11ErrorHandlers();
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::Init");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void InstallX11ErrorHandlers()
    {
        try
        {
            // Copied from CEF upstream cefclient.
            // Install xlib error handlers so that the application won't be terminated
            // on non-fatal errors. Must be done after initializing GTK.
            XSetErrorHandler(_onHandleErrorDelegate);
            XSetIOErrorHandler(_onHandleIOErrorDelegate);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::InstallX11ErrorHandlers");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual short HandleError(IntPtr display, ref XErrorEvent errorEvent)
    {
        try
        {
            if (this._debugging && errorEvent.request_code > 0 && errorEvent.error_code > 0)
            {
                var errorMsgSb = new StringBuilder(160);
                XGetErrorText(display, errorEvent.error_code, errorMsgSb, errorMsgSb.Capacity);
                var requestType = GetRequestType(errorEvent.request_code);
                Logger.Instance.Log.LogWarning("Request Code: {errorEvent.request_code}\tRequest Type: {requestType}\tX11 Error: {errorMsgSb.ToString()}",
                                                               errorEvent.request_code, requestType, errorMsgSb.ToString());
            }

            return 0;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::HandleError");
            Logger.Instance.Log.LogError(exception);
        }

        return 0;
    }

    protected virtual short HandleIOError(IntPtr d)
    {
        return 0;
    }

    protected virtual IntPtr CreateNewWindow(int windowType)
    {
        try
        {
            GtkWindowType type = (GtkWindowType)windowType;
            _handle = gtk_window_new(type);
            Utils.AssertNotNull("CreateNewWindow", _handle);
            return _handle;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::CreateNewWindow");
            Logger.Instance.Log.LogError(exception);
        }

        return IntPtr.Zero;
    }

    protected virtual void SetWindowDefaultSize(int width, int height)
    {
        try
        {
            gtk_window_set_default_size(_handle, width, height);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetWindowDefaultSize");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void SetWindowPosistion(int windowPosition)
    {
        try
        {
            GtkWindowPosition position = (GtkWindowPosition)windowPosition;
            gtk_window_set_position(_handle, position);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::SetWindowPosistion");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void SetAppIcon(IntPtr window, string filename)
    {
        try
        {
            filename = IconHandler.IconFullPath(filename);
            if (!string.IsNullOrWhiteSpace(filename))
            {
                IntPtr error = IntPtr.Zero;
                gtk_window_set_icon_from_file(window, filename, out error);
                if (error != IntPtr.Zero)
                {
                    Logger.Instance.Log.LogError("Icon handle not successfully freed.");
                }
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void ShowWindow()
    {
        try
        {
            gtk_widget_show_all(_handle);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::ShowWindow");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual Size GetWindowSize()
    {
        try
        {
            gtk_window_get_size(_handle, out int width, out int height);
            return new Size(width, height);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::GetWindowSize");
            Logger.Instance.Log.LogError(exception);
        }

        return new Size();
    }

    protected virtual void OnRealized(IntPtr window)
    {
        try
        {
            _xid = GetNativeHandle();
            if (_xid == IntPtr.Zero)
            {
                throw new Exception("Window XID is invalid");
            }

            var createdEvent = new CreatedEventArgs(window, _xid);
            HostCreated?.Invoke(this, createdEvent);
            _isInitialized = true;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::OnRealized");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void OnSizeAllocate(IntPtr window, IntPtr allocation, int baseline)
    {
        if (_handle != IntPtr.Zero && _isInitialized)
        {
            var size = GetWindowSize();
            HostSizeChanged?.Invoke(this, new SizeChangedEventArgs(size.Width, size.Height));
        }
    }

    protected virtual bool OnResize(IntPtr window, GtkEvent gtkEvent, IntPtr data)
    {
        if (_handle != IntPtr.Zero && _isInitialized)
        {
            HostMoving?.Invoke(this, new MovingEventArgs());
        }
        return false;
    }

    protected virtual bool OnDestroy(IntPtr window, IntPtr data)
    {
        Xilium.CefGlue.CefRuntime.QuitMessageLoop();
        if (_handle != IntPtr.Zero && _isInitialized)
        {
            HostClose?.Invoke(this, new CloseEventArgs());
        }

        return false;
    }

    protected virtual void ConnectRealizeSignal(RealizeCallback callback, GClosureNotify destroyData)
    {
        _onRealizedSignal = Marshal.GetFunctionPointerForDelegate(callback);

        RegisterHandler(
            "realize",
            _onRealizedSignal,
            destroyData,
            GConnectFlags.GConnectAfter,
            IntPtr.Zero);
    }

    protected virtual void ConnectSizeAllocateSignal(SizeAllocateCallback callback, GClosureNotify destroyData)
    {
        _onSizeAllocateSignal = Marshal.GetFunctionPointerForDelegate(callback);

        RegisterHandler(
            "size-allocate",
            _onSizeAllocateSignal,
            destroyData,
            GConnectFlags.GConnectAfter,
            IntPtr.Zero);
    }

    protected virtual void ConnectResizeSignal(ResizeCallback callback, GClosureNotify destroyData)
    {
        _onResizeSignal = Marshal.GetFunctionPointerForDelegate(callback);

        RegisterHandler(
            "configure-event",
            _onResizeSignal,
            destroyData,
            GConnectFlags.GConnectAfter,
            IntPtr.Zero);
    }

    protected virtual void ConnectDestroySignal(DestroyCallback callback, GClosureNotify destroyData)
    {
        _onDestroySignal = Marshal.GetFunctionPointerForDelegate(callback);

        RegisterHandler(
            "delete-event",
            _onDestroySignal,
            destroyData,
            GConnectFlags.GConnectAfter,
            IntPtr.Zero);
    }

    protected virtual void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
    {
        try
        {
            g_signal_connect_data(_handle, signalName, handler, data, destroyData, connectFlags);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in LinuxGtk3Host::RegisterHandler");
            Logger.Instance.Log.LogError(exception);
        }
    }

    protected virtual void FreeData()
    {
    }

    #endregion CreateWindow
}