// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public abstract partial class NativeHostBase : IChromelyNativeHost
    {
        private const int PROCESS_IDLE_ID = 0;

        [DllImport(Libraries.Kernel32)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public unsafe static extern IntPtr LoadIconW(
         IntPtr hInstance,
         IntPtr lpIconName);

        protected const string Chromely_WINDOW_CLASS = "ChromelyWindowClass";
        protected const uint IDI_APPLICATION = 32512;
        protected const int CW_USEDEFAULT = unchecked((int)0x80000000);
        protected static RECT DefaultBounds => new RECT(CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT);
        public static NativeHostBase NativeInstance;
        protected static bool WindowInterceptorInitialized = false;
        protected IWindowMessageInterceptor _messageInterceptor;
        protected IWindowOptions _options;
        protected IntPtr _handle;
        protected bool _windowFrameless;
        protected bool _isInitialized;
        protected IntPtr _consoleParentInstance;
        protected WNDPROC _wndProc;
        protected KeyboardLLHook _keyboardHook;
        protected WindowStylePlacement _windoStylePlacement;
        protected IKeyboadHookHandler _keyboadHandler;

        public event EventHandler<CreatedEventArgs> HostCreated;
        public event EventHandler<MovingEventArgs> HostMoving;
        public event EventHandler<SizeChangedEventArgs> HostSizeChanged;
        public event EventHandler<CloseEventArgs> HostClose;

        public NativeHostBase(IWindowMessageInterceptor messageInterceptor, IKeyboadHookHandler keyboadHandler)
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
            _messageInterceptor = messageInterceptor;
            _keyboadHandler = keyboadHandler;
        }

        public IntPtr Handle => _handle;

        public unsafe virtual void CreateWindow(IWindowOptions options, bool debugging)
        {
            _keyboadHandler?.SetNativeHost(this);
           _options = options;
            _windowFrameless = _options.WindowFrameless;

            _wndProc = WndProc;
            _consoleParentInstance = GetConsoleWindow();
            _options.WindowState = (_options.Fullscreen || _options.KioskMode) ?  WindowState.Fullscreen : _options.WindowState;
            _windoStylePlacement = new WindowStylePlacement(_options);

            User32.WNDCLASS wcex = new User32.WNDCLASS();
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

            var stylePlacement = GetWindowStylePlacement(_options.WindowState);

            var hWnd = User32.CreateWindowExW(
                            stylePlacement.ExStyles,
                            Chromely_WINDOW_CLASS,
                            _options.Title,
                            stylePlacement.Styles,
                            stylePlacement.RECT.left,
                            stylePlacement.RECT.top,
                            stylePlacement.RECT.Width,
                            stylePlacement.RECT.Height,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            _consoleParentInstance,
                            null);

            if (hWnd == IntPtr.Zero)
            {
                Logger.Instance.Log.LogError("Chromelywindow creation failed");
                return;
            }

            PlaceWindow(hWnd, stylePlacement);
            InstallHooks(hWnd);
            ShowWindow(hWnd, stylePlacement.ShowCommand);
            UpdateWindow(hWnd);
            RegisterHotKeys(hWnd);
        }

        public virtual IntPtr GetNativeHandle()
        {
            return IntPtr.Zero;
        }

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
                Logger.Instance.Log.LogError(exception, exception.Message);
            }
        }

        public virtual Size GetWindowClientSize()
        {
            return GetClientSize();
        }

        public virtual float GetWindowDpiScale()
        {
            const int StandardDpi = 96;
            float scale = 1;
            var hdc = GetDC(_handle);
            try {
                var dpi = Gdi32.GetDeviceCaps(hdc, Gdi32.DeviceCapability.LOGPIXELSY);
                scale = (float)dpi / StandardDpi;
            }
            finally {
                ReleaseDC(_handle, hdc);
            }
            return scale;
        }

        public virtual void SetupMessageInterceptor(IntPtr browserWindowHandle)
        {
            if (!WindowInterceptorInitialized && _windowFrameless)
            {
                _messageInterceptor?.Setup(this, browserWindowHandle);
                WindowInterceptorInitialized = true;
            }
        }

        public virtual void ResizeBrowser(IntPtr browserHande, int width, int height)
        {
            SetWindowPos(browserHande, IntPtr.Zero, 0, 0, width, height, SWP.NOZORDER);
        }

        public virtual WindowState GetWindowState() 
        {
            var placement = new WINDOWPLACEMENT();
            placement.length = (uint)Marshal.SizeOf(placement);
            GetWindowPlacement(_handle, out placement);

            switch (placement.showCmd) {
                case SW.Maximized:
                    return WindowState.Maximize;
                case SW.Minimized:
                    return WindowState.Minimize;
                case SW.Normal:
                    return WindowState.Normal;
            }
            // If unknown
            return WindowState.Normal;
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

            return  result == BOOL.TRUE;
        }

        public virtual void Exit()
        {
            if (_handle != IntPtr.Zero)
            {
                try
                {
                    DetachHooks();
                    ShowWindow(_handle, SW.HIDE);

                    uint processId = 0;
                    GetWindowThreadProcessId(_handle, out processId);
                    if (processId != PROCESS_IDLE_ID)
                    {
                        var process = Process.GetProcessById((int)processId);
                        if (process != null)
                        {
                            process.Kill();
                        }
                    }

                }
                catch {}
            }
        }

        public virtual void SetWindowTitle(string title)
        {
            if (Handle != IntPtr.Zero)
            {
                SetWindowText(Handle, title);
            }
        }

        #region Create Window Protected
        protected virtual Rectangle GetWindowBounds()
        {
            var bounds = new System.Drawing.Rectangle(_options.Position.X, _options.Position.Y, _options.Size.Width, _options.Size.Height);

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
        protected virtual Size GetClientSize()
        {
            var size = new Size();
            if (_handle != IntPtr.Zero)
            {
                RECT rect = new RECT();
                GetClientRect(_handle, ref rect);
                size.Width = rect.Width;
                size.Height = rect.Height;
            }

            return size;
        }

        protected virtual void PreCreated(IntPtr hWnd)
        {
        }

        protected virtual void OnCreated(IntPtr hWnd)
        {
        }

        protected virtual void PlaceWindow(IntPtr hWnd, WindowStylePlacement stylePlacement)
        {
            if (_options.Fullscreen || _options.KioskMode)
            {
                SetFullscreenScreen(hWnd, (int)stylePlacement.Styles, (int)stylePlacement.ExStyles);
            }
            else
            {
                // Center window if State is Normal
                if (_options.WindowState == WindowState.Normal && _options.StartCentered)
                {
                    WindowHelper.CenterWindowToScreen(hWnd);
                }
            }

            WINDOWPLACEMENT wpPrev;
            var placement = GetWindowPlacement(hWnd, out wpPrev);
            if (placement == BOOL.TRUE)
            {
                _windoStylePlacement.WindowPlacement = wpPrev;
            }
        }

        protected virtual void OnSizeChanged(int width, int height)
        {
            var handler = HostSizeChanged;
            handler?.Invoke(width, new SizeChangedEventArgs(width, height));
        }

        #endregion Create Window Protected

        #region Create Window Private

        protected virtual IntPtr GetIconHandle()
        {
            var hIcon = IconHandler.LoadIconFromFile(_options.RelativePathToIconFile);
            try
            {
                if (hIcon == null)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    var iconAsResource = assembly?.GetManifestResourceNames()
                        .FirstOrDefault(res => res.EndsWith(_options.RelativePathToIconFile));
                    if (iconAsResource != null)
                    {
                        using (var resStream = assembly.GetManifestResourceStream(iconAsResource))
                        {
                            using(var fileStream = new FileStream(_options.RelativePathToIconFile, FileMode.Create))
                            {
                                resStream?.CopyTo(fileStream);
                            }
                        }
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

        protected virtual WindowStylePlacement GetWindowStylePlacement(WindowState state)
        {
            WindowStylePlacement windowStyle = new WindowStylePlacement(_options);
            if (_options.UseCustomStyle && _options != null && _options.CustomStyle.IsValid())
            {
                return GetWindowStyles(_options.CustomStyle, state);
            }

            var styles = WindowStylePlacement.NormalStyles;
            var exStyles = WindowStylePlacement.NormalExStyles;

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
                _options.DisableResizing = _options.KioskMode ? true : _options.DisableResizing;
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

        protected WindowStylePlacement GetWindowStyles(WindowCustomStyle customCreationStyle, WindowState state)
        {
            WindowStylePlacement windowStyle = new WindowStylePlacement(_options);
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
            switch (state)
            {
                case WindowState.Normal:
                    return SW.SHOWNORMAL;

                case WindowState.Maximize:
                case WindowState.Fullscreen:
                    return SW.SHOWMAXIMIZED;
            }

            return SW.SHOWNORMAL;
        }

        #endregion Create Window Private

        #region WndProc

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

            return isHandled;
        }

        #endregion WndProc Methods
        #endregion WndProc

        #region Message Loop

        protected static void RunMessageLoopInternal()
        {
            MSG msg = new MSG();
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
}