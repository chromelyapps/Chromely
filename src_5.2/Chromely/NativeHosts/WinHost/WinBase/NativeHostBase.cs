// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable
#pragma warning disable CA1401
#pragma warning disable CA1806
#pragma warning disable CA1822
#pragma warning disable CA2211

namespace Chromely.NativeHosts;

/// <summary>
/// Represents Windows OS native host.
/// </summary>
public abstract partial class NativeHostBase : IChromelyNativeHost
{
    private const int PROCESS_IDLE_ID = 0;

    [DllImport(Libraries.Kernel32)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
    private unsafe static extern IntPtr LoadIconW(
     IntPtr hInstance,
     IntPtr lpIconName);

    protected const string Chromely_WINDOW_CLASS = "ChromelyWindowClass";
    protected const uint IDI_APPLICATION = 32512;
    protected const int CW_USEDEFAULT = unchecked((int)0x80000000);
    protected static RECT DefaultBounds => new(CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT);
    public static NativeHostBase NativeInstance;
    protected static bool WindowInterceptorInitialized = false;

    protected readonly IChromelyConfiguration _config;
    protected readonly IWindowMessageInterceptor _messageInterceptor;
    protected readonly IKeyboadHookHandler _keyboadHandler;
    protected readonly IWindowOptions _options;

    protected IntPtr _handle;
    protected bool _windowFrameless;
    protected bool _isInitialized;
    protected IntPtr _consoleParentInstance;
    private WNDPROC _wndProc;
    private KeyboardLLHook _keyboardHook;
    private Win32WindowStyles _windowStyles;

    public event EventHandler<CreatedEventArgs> HostCreated;
    public event EventHandler<MovingEventArgs> HostMoving;
    public event EventHandler<SizeChangedEventArgs> HostSizeChanged;
    public event EventHandler<CloseEventArgs> HostClose;

    /// <summary>
    /// Initializes a new instance of <see cref="NativeHostBase"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="messageInterceptor">Instance of <see cref="IWindowMessageInterceptor"/>.</param>
    /// <param name="keyboadHandler">Instance of <see cref="IKeyboadHookHandler"/>.</param>
    public NativeHostBase(IChromelyConfiguration config, IWindowMessageInterceptor messageInterceptor, IKeyboadHookHandler keyboadHandler)
    {
        _config = config;
        _options = _config?.WindowOptions ?? new WindowOptions();
        _isInitialized = false;
        _handle = IntPtr.Zero;
        _messageInterceptor = messageInterceptor;
        _keyboadHandler = keyboadHandler;
    }

    /// <inheritdoc/>
    public IntPtr Handle => _handle;

    /// <inheritdoc/>
    public unsafe virtual void CreateWindow()
    {
        _keyboadHandler?.SetNativeHost(this);
        _windowFrameless = _options.WindowFrameless;

        _wndProc = WndProc;
        _consoleParentInstance = GetConsoleWindow();
        _options.WindowState = (_options.Fullscreen || _options.KioskMode) ? WindowState.Fullscreen : _options.WindowState;
        _windowStyles = new Win32WindowStyles(_options);

        User32.WNDCLASS wcex = new();
        wcex.style = User32.CS.HREDRAW | User32.CS.VREDRAW;
        wcex.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProc);
        wcex.cbClsExtra = 0;
        wcex.cbWndExtra = 0;
        wcex.hIcon = GetIconHandle();
        wcex.hCursor = User32.LoadCursorW(IntPtr.Zero, (IntPtr)CursorResourceId.IDC_ARROW);
        wcex.hbrBackground = Gdi32.GetStockObject(Gdi32.StockObject.WHITE_BRUSH);
        wcex.lpszMenuName = null;
        wcex.hInstance = _consoleParentInstance;

        fixed (char* c = Chromely_WINDOW_CLASS)
        {
            wcex.lpszClassName = c;
        }

        if (User32.RegisterClassW(ref wcex) == 0)
        {
            Logger.Instance.Log.LogError("Chromelywindow registration failed");
            return;
        }

        var windowStyles = GetWindowStyles(_options.WindowState);

        var hWnd = User32.CreateWindowExW(
                        windowStyles.ExStyles,
                        Chromely_WINDOW_CLASS,
                        _options.Title,
                        windowStyles.Styles,
                        windowStyles.RECT.left,
                        windowStyles.RECT.top,
                        windowStyles.RECT.Width,
                        windowStyles.RECT.Height,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        _consoleParentInstance,
                        null);

        if (hWnd == IntPtr.Zero)
        {
            Logger.Instance.Log.LogError("Chromelywindow creation failed");
            return;
        }

        PlaceWindow(hWnd, windowStyles);
        InstallHooks(hWnd);
        ShowWindow(hWnd, windowStyles.ShowCommand);
        UpdateWindow(hWnd);
        RegisterHotKeys(hWnd);
    }

    /// <inheritdoc/>
    public virtual IntPtr GetNativeHandle()
    {
        return IntPtr.Zero;
    }

    /// <inheritdoc/>
    public virtual void Run()
    {
        try
        {
            if (_options.UseOnlyCefMessageLoop)
            {
                CefRuntime.RunMessageLoop();
                CefRuntime.Shutdown();
            }
            else
            {
                RunMessageLoopInternal();
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError("Error in NativeHostBase::Run");
            Logger.Instance.Log.LogError(exception);
        }
    }

    /// <inheritdoc/>
    public virtual Size GetWindowClientSize()
    {
        return GetClientSize();
    }

    /// <inheritdoc/>
    public virtual float GetWindowDpiScale()
    {
        const int StandardDpi = 96;
        float scale = 1;
        var hdc = GetDC(_handle);
        try
        {
            var dpi = Gdi32.GetDeviceCaps(hdc, Gdi32.DeviceCapability.LOGPIXELSY);
            scale = (float)dpi / StandardDpi;
        }
        finally
        {
            ReleaseDC(_handle, hdc);
        }
        return scale;
    }

    /// <inheritdoc/>
    public virtual void SetupMessageInterceptor(IntPtr browserWindowHandle)
    {
        if (!WindowInterceptorInitialized && _windowFrameless)
        {
            _messageInterceptor?.Setup(this, browserWindowHandle);
            WindowInterceptorInitialized = true;
        }
    }

    /// <inheritdoc/>
    public virtual void ResizeBrowser(IntPtr browserHandle, int width, int height)
    {
        SetWindowPos(browserHandle, IntPtr.Zero, 0, 0, width, height, SWP.NOZORDER);
    }

    /// <inheritdoc/>
    public virtual WindowState GetWindowState()
    {
        var placement = new WINDOWPLACEMENT();
        placement.length = (uint)Marshal.SizeOf(placement);
        GetWindowPlacement(_handle, out placement);

        return placement.showCmd switch
        {
            SW.Maximized => WindowState.Maximize,
            SW.Minimized => WindowState.Minimize,
            SW.Normal => WindowState.Normal,
            // If unknown
            _ => WindowState.Normal,
        };
    }


    /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
    /// <param name="state"> The state to set. </param>
    /// <returns> True if it succeeds, false if it fails. </returns>
    public virtual bool SetWindowState(WindowState state)
    {
        // Window State should not change for Kiosk mode
        if (this._options.KioskMode)
        {
            return false;
        }

        BOOL result = BOOL.FALSE;

        switch (state)
        {
            case WindowState.Normal:
                // Restore the window
                result = ShowWindow(_handle, SW.RESTORE);
                break;

            case WindowState.Minimize:
                // Minimize the window
                result = ShowWindow(_handle, SW.SHOWMINIMIZED);
                break;

            case WindowState.Maximize:
                // Maximize the window
                result = ShowWindow(_handle, SW.SHOWMAXIMIZED);
                break;

                // TODO full screen support
                // try moving the fullscreen code from CreateWindow / kiosk mode
                // into its own function
        }

        return result == BOOL.TRUE;
    }

    /// <inheritdoc/>
    public virtual void Exit()
    {
        if (_handle != IntPtr.Zero)
        {
            try
            {
                DetachHooks();
                ShowWindow(_handle, SW.HIDE);

                GetWindowThreadProcessId(_handle, out uint processId);
                if (processId != PROCESS_IDLE_ID)
                {
                    var process = Process.GetProcessById((int)processId);
                    if (process is not null)
                    {
                        process.Kill();
                    }
                }

            }
            catch { }
        }
    }

    /// <inheritdoc/>
    public virtual void SetWindowTitle(string title)
    {
        if (Handle != IntPtr.Zero)
        {
            SetWindowText(Handle, title);
        }
    }

    #region Create Window Protected

    /// <summary>
    /// Get window bounds.
    /// </summary>
    /// <returns>instance of <see cref="Rectangle"/>.</returns>
    protected virtual Rectangle GetWindowBounds()
    {
        var bounds = new Rectangle(_options.Position.X, _options.Position.Y, _options.Size.Width, _options.Size.Height);

        switch (_options.WindowState)
        {
            case WindowState.Normal:
                break;

            case WindowState.Maximize:
            case WindowState.Fullscreen:
                bounds = WindowHelper.FullScreenBounds(bounds);
                _options.Position = new WindowPosition(bounds.X, bounds.Y);
                _options.Size = new WindowSize(bounds.Width, bounds.Height);
                break;
        }

        RECT rect;
        rect.left = bounds.Left;
        rect.top = bounds.Top;
        rect.right = bounds.Right;
        rect.bottom = bounds.Bottom;

        return rect;
    }

    /// <summary>
    /// Get window client size.
    /// </summary>
    /// <returns>instance of <see cref="Size"/>.</returns>
    protected virtual Size GetClientSize()
    {
        var size = new Size();
        if (_handle != IntPtr.Zero)
        {
            RECT rect = new();
            GetClientRect(_handle, ref rect);
            size.Width = rect.Width;
            size.Height = rect.Height;
        }

        return size;
    }

    /// <summary>
    /// Method to run before Win32 window is created.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    protected virtual void PreCreated(IntPtr hWnd)
    {
    }

    /// <summary>
    /// Method to run when Win32 window is created.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    protected virtual void OnCreated(IntPtr hWnd)
    {
    }

    /// <summary>
    /// Window placement function.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="stylePlacement">Placement style - instance of <see cref="Win32WindowStyles"/>.</param>
    protected virtual void PlaceWindow(IntPtr hWnd, Win32WindowStyles stylePlacement)
    {
        if (_options.Fullscreen || _options.KioskMode)
        {
            SetWindowToFullscreen(hWnd, (int)stylePlacement.Styles, (int)stylePlacement.ExStyles);
        }
        else
        {
            // Center window if State is Normal
            if (_options.WindowState == WindowState.Normal && _options.StartCentered)
            {
                WindowHelper.CenterWindowToScreen(hWnd);
            }
        }

        var placement = GetWindowPlacement(hWnd, out WINDOWPLACEMENT wpPrev);
        if (placement == BOOL.TRUE)
        {
            _windowStyles.WindowPlacement = wpPrev;
        }
    }

    /// <summary>
    /// On window size changed method.
    /// </summary>
    /// <param name="width">The new window width.</param>
    /// <param name="height">he new window height.</param>
    protected virtual void OnSizeChanged(int width, int height)
    {
        var handler = HostSizeChanged;
        handler?.Invoke(width, new SizeChangedEventArgs(width, height));
    }

    #endregion Create Window Protected

    #region Create Window Private

    /// <summary>
    /// Gets window icon handle.
    /// </summary>
    /// <returns>The pointer to the icon - instance of <see cref="IntPtr"/></returns>
    protected virtual IntPtr GetIconHandle()
    {
        var hIcon = IconHandler.LoadIconFromFile(_options.RelativePathToIconFile);
        try
        {
            if (hIcon is null)
            {
                var assembly = Assembly.GetEntryAssembly();
                var iconAsResource = assembly?.GetManifestResourceNames()
                    .FirstOrDefault(res => res.EndsWith(_options.RelativePathToIconFile));
                if (iconAsResource is not null)
                {
                    using var resStream = assembly.GetManifestResourceStream(iconAsResource);
                    using var fileStream = new FileStream(_options.RelativePathToIconFile, FileMode.Create);
                    resStream?.CopyTo(fileStream);
                }
                hIcon = IconHandler.LoadIconFromFile(_options.RelativePathToIconFile);
            }
        }
        catch
        {
            // ignore
        }
        return hIcon ?? LoadIconW(IntPtr.Zero, (IntPtr)IDI_APPLICATION);
    }

    /// <summary>
    /// Get window styles.
    /// </summary>
    /// <param name="state">The window state - type of <see cref="WindowState"/>.</param>
    /// <returns>instance of <see cref="Win32WindowStyles"/>.</returns>
    protected virtual Win32WindowStyles GetWindowStyles(WindowState state)
    {
        Win32WindowStyles windowStyle = new(_options);
        if (_options.UseCustomStyle && _options is not null && _options.CustomStyle.IsValid())
        {
            return GetWindowStyles(_options.CustomStyle, state);
        }

        var styles = Win32WindowStyles.NormalStyles;
        var exStyles = Win32WindowStyles.NormalExStyles;

        if (_options.DisableResizing)
        {
            styles = WS.OVERLAPPEDWINDOW & ~WS.THICKFRAME | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
            styles &= ~WS.MAXIMIZEBOX;
        }

        if (_options.DisableMinMaximizeControls)
        {
            styles &= ~WS.MINIMIZEBOX;
            styles &= ~WS.MAXIMIZEBOX;
        }

        if (_options.WindowFrameless)
        {
            styles = WS.POPUPWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
            styles |= WS.SIZEBOX;
        }

        if (_options.KioskMode || _options.Fullscreen)
        {
            styles &= ~(WS.CAPTION);
            exStyles &= ~(WS_EX.DLGMODALFRAME | WS_EX.WINDOWEDGE | WS_EX.CLIENTEDGE | WS_EX.STATICEDGE);
            state = WindowState.Fullscreen;
            _options.DisableResizing = _options.KioskMode || _options.DisableResizing;
        }

        windowStyle.Styles = styles;
        windowStyle.ExStyles = exStyles;
        windowStyle.RECT = GetWindowBounds();

        switch (state)
        {
            case WindowState.Normal:
                windowStyle.ShowCommand = SW.SHOWNORMAL;
                break;

            case WindowState.Maximize:
                windowStyle.Styles |= WS.MAXIMIZE;
                windowStyle.ShowCommand = SW.SHOWMAXIMIZED;
                break;

            case WindowState.Fullscreen:
                windowStyle.ShowCommand = SW.SHOWMAXIMIZED;
                break;

            default:
                break;
        }

        return windowStyle;
    }

    /// <summary>
    /// Get window styles.
    /// </summary>
    /// <param name="customCreationStyle">Custom window styles.</param>
    /// <param name="state">The window state - type of <see cref="WindowState"/>.</param>
    /// <returns>instance of <see cref="Win32WindowStyles"/>.</returns>
    protected Win32WindowStyles GetWindowStyles(WindowCustomStyle customCreationStyle, WindowState state)
    {
        Win32WindowStyles windowStyle = new(_options);
        var styles = (WS)customCreationStyle.WindowStyles;
        var exStyles = (WS_EX)customCreationStyle.WindowExStyles;

        windowStyle.Styles = styles;
        windowStyle.ExStyles = exStyles;
        windowStyle.RECT = GetWindowBounds();

        switch (state)
        {
            case WindowState.Normal:
                windowStyle.ShowCommand = SW.SHOWNORMAL;
                break;

            case WindowState.Maximize:
                windowStyle.Styles |= WS.MAXIMIZE;
                windowStyle.ShowCommand = SW.SHOWMAXIMIZED;
                break;

            case WindowState.Fullscreen:
                windowStyle.ShowCommand = SW.SHOWMAXIMIZED;
                break;

            default:
                break;
        }

        return windowStyle;
    }

    private SW GetShowWindowCommand(WindowState state)
    {
        return state switch
        {
            WindowState.Normal => SW.SHOWNORMAL,
            WindowState.Maximize or WindowState.Fullscreen => SW.SHOWMAXIMIZED,
            _ => SW.SHOWNORMAL,
        };
    }

    #endregion Create Window Private

    #region WndProc



    /// <summary>
    /// Processes messages for the native host.
    /// </summary>
    /// <param name="hWnd">The window handle receiving the message</param>
    /// <param name="message">Identifier of the message.</param>
    /// <param name="wParam">Additional info associated with the message.</param>
    /// <param name="lParam">Additional info associated with the message.</param>
    /// <returns>IntPtr.Zero is message is handled, otherwise other values.</returns>
    protected virtual IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
    {
        WM wmMsg = (WM)message;
        switch (wmMsg)
        {
            case WM.NCCREATE:
                {
                    NativeInstance = this;
                    _handle = hWnd;
                    PreCreated(hWnd);
                    break;
                }

            case WM.CREATE:
                {
                    OnCreated(hWnd);
                    var createdEvent = new CreatedEventArgs(_handle, _handle);
                    HostCreated?.Invoke(this, createdEvent);
                    _isInitialized = true;
                    break;
                }

            case WM.ERASEBKGND:
                return new IntPtr(1);

            case WM.NCHITTEST:
                if (_options.WindowFrameless)
                {
                    return (IntPtr)HT.CAPTION;
                }
                break;

            case WM.MOVING:
            case WM.MOVE:
                {
                    HostMoving?.Invoke(this, new MovingEventArgs());
                    return IntPtr.Zero;
                }

            case WM.SIZING:
            case WM.SIZE:
                {
                    var size = GetClientSize();
                    OnSizeChanged(size.Width, size.Height);
                    break;
                }
            case WM.GETMINMAXINFO:
                {
                    if (HandleMinMaxSizes(lParam))
                    {
                        return IntPtr.Zero;
                    }
                    break;
                }

            case WM.CLOSE:
                {
                    if (_handle != IntPtr.Zero && _isInitialized)
                    {
                        HostClose?.Invoke(this, new CloseEventArgs());
                    }

                    DetachHooks();
                    DestroyWindow(_handle);
                    break;
                }

            case WM.DESTROY:
                {
                    if (_options.UseOnlyCefMessageLoop)
                    {
                        CefRuntime.QuitMessageLoop();
                    }
                    PostQuitMessage(0);
                    break;
                }
            default:
                break;
        }

        return (DefWindowProcW(hWnd, wmMsg, wParam, lParam));
    }

    #region WndProc Methods

    protected virtual void HandleSizeChanged(int width, int height)
    {
        HostSizeChanged?.Invoke(this, new SizeChangedEventArgs(width, height));
    }

    private unsafe bool HandleMinMaxSizes(IntPtr lParam)
    {
        bool isHandled = false;

        MINMAXINFO* mmi = (MINMAXINFO*)lParam;
        if (!_options.MinimumSize.IsEmpty)
        {
            mmi->ptMinTrackSize.X = _options.MinimumSize.Width;
            mmi->ptMinTrackSize.Y = _options.MinimumSize.Height;
            isHandled = true;
        }

        if (!_options.MaximumSize.IsEmpty)
        {
            mmi->ptMaxTrackSize.X = _options.MaximumSize.Width;
            mmi->ptMaxTrackSize.Y = _options.MaximumSize.Height;
            isHandled = true;
        }

        // https://stackoverflow.com/questions/39816031/maximize-window-maintaining-taskbar-limits
        if (_options.WindowFrameless && _options.WindowState == WindowState.Maximize)
        {
            IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR.DEFAULTTONEAREST);

            MONITORINFOEXW monInfo = new(null);
            monInfo.cbSize = (uint)Marshal.SizeOf(monInfo);

            var captionHeight = GetSystemMetrics(SystemMetric.SM_CYCAPTION);

            GetMonitorInfoW(handle, ref monInfo);
            var workArea = monInfo.rcWork;
            var monitorArea = monInfo.rcMonitor;
            mmi->ptMaxPosition.X = Math.Abs(workArea.left - monitorArea.left) - captionHeight / 2;
            mmi->ptMaxPosition.Y = Math.Abs(workArea.top - monitorArea.top);
            mmi->ptMaxSize.X = Math.Abs(workArea.right - workArea.left) + captionHeight;
            mmi->ptMaxSize.Y = Math.Abs(workArea.bottom - workArea.top) + captionHeight;

            isHandled = true;
        }

        return isHandled;
    }

    #endregion WndProc Methods
    #endregion WndProc

    #region Message Loop

    protected static void RunMessageLoopInternal()
    {
        MSG msg = new();
        while (GetMessageW(ref msg, IntPtr.Zero, 0, 0) != 0)
        {
            if (msg.message == WM.CLOSE)
            {
                NativeInstance.DetachHooks();
            }

            TranslateMessage(ref msg);
            DispatchMessageW(ref msg);
        }
    }

    #endregion

    #region Install/Detach Hooks

    public virtual void InstallHooks(IntPtr handle)
    {
        try
        {
            _keyboardHook = new KeyboardLLHook(handle, _options, _keyboadHandler);
            _keyboardHook.Install();
        }
        catch
        {
            DetachHooks();
        }
    }

    public virtual void DetachHooks()
    {
        try
        {
            _keyboardHook?.Dispose();
            _keyboardHook = null;
        }
        catch { }
    }


    #endregion

    protected static readonly HT[] BorderHitTestResults =
    {
            HT.TOP,
            HT.TOPLEFT,
            HT.TOPRIGHT,
            HT.BOTTOM,
            HT.BOTTOMLEFT,
            HT.BOTTOMRIGHT,
            HT.LEFT,
            HT.RIGHT,
            HT.BORDER
        };
}