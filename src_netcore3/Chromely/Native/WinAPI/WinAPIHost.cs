using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using PInvoke.Net;
using Xilium.CefGlue;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native
{
    public partial class WinAPIHost : IChromelyNativeHost
    {
        public event EventHandler<CreatedEventArgs> Created;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<CloseEventArgs> Close;

        private static readonly LowLevelKeyboardProc _hookCallback = HookCallback;

        private IChromelyConfiguration _config;
        private IntPtr _handle;
        private bool _isInitialized;

        private static string AppName = "Chromely";
        private static string ClassName = "chromelywindow";
        private static IntPtr _consoleParentInstance;
        private WndProc _wndProc;
        private IntPtr _hookID = IntPtr.Zero;

        public WinAPIHost()
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
        }

        public void CreateWindow(IChromelyConfiguration config)
        {
            _config = config;
            _wndProc = WndProc;
            _consoleParentInstance = GetConsoleWindow();

            WNDCLASSEX wcex = new WNDCLASSEX();
            wcex.style = ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw;
            wcex.cbSize = (uint)Marshal.SizeOf(wcex);
            wcex.lpfnWndProc = _wndProc;
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hIcon = GetIconHandle();
            wcex.hIconSm = GetIconHandle();
            wcex.hCursor = LoadCursor(IntPtr.Zero, (int)IDC_ARROW);
            wcex.hbrBackground = GetStockObject(StockObjects.WHITE_BRUSH);
            wcex.lpszMenuName = null;
            wcex.lpszClassName = "chromelywindow";
            wcex.hInstance = _consoleParentInstance;

            if (RegisterClassEx(ref wcex) == 0)
            {
                Logger.Instance.Log.Error("chromelywindow registration failed");
                return;
             }

            var styles = GetWindowStyles(_config.WindowState);

            WinNativeMethods.RECT rect;
            rect.Left = _config.WindowLeft;
            rect.Top = _config.WindowTop;
            rect.Right = _config.WindowLeft + _config.WindowWidth;
            rect.Bottom = _config.WindowTop + _config.WindowHeight;

            AdjustWindowRectEx(ref rect, styles.Item1, false, styles.Item2);

            var hWnd = CreateWindowEx(styles.Item2,
                        ClassName,
                        AppName,
                        styles.Item1,
                        rect.Left,
                        rect.Top,
                        rect.Width,
                        rect.Height,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero);

            if (hWnd == IntPtr.Zero)
            {
                Logger.Instance.Log.Error("chromelywindow creation failed");
                return;
            }

            if (_config.WindowKioskMode)
            {
                //// Set new window style and size.
                var windowHDC = GetDC(hWnd);
                var fullscreenWidth = GetDeviceCaps(windowHDC, (int)DeviceCap.HORZRES);
                var fullscreenHeight = GetDeviceCaps(windowHDC, (int)DeviceCap.VERTRES);
                ReleaseDC(hWnd, windowHDC);

                SetWindowLongPtr(hWnd, (int)WindowLongFlags.GWL_STYLE, (IntPtr)styles.Item1);
                SetWindowLongPtr(hWnd, (int)WindowLongFlags.GWL_EXSTYLE, (IntPtr)styles.Item2);

                SetWindowPos(hWnd, (IntPtr)HwndZOrder.Top, 0, 0, fullscreenWidth, fullscreenHeight,
                    SetWindowPosFlags.IgnoreZOrder | SetWindowPosFlags.FrameChanged);

                ShowWindow(hWnd, ShowWindowCommand.SW_MAXIMIZE);

                try
                {
                    this._hookID = SetHook(_hookCallback);
                }
                catch
                {
                    DetachKeyboardHook();
                }
            }
            else
            {
                // Center window if State is Normal
                if (_config.WindowState == WindowState.Normal && _config.WindowCenterScreen)
                {
                    CenterWindowToScreen(hWnd);
                }

                ShowWindow(hWnd, styles.Item3);
            }

            UpdateWindow(hWnd);
            RegisterHotKey(IntPtr.Zero, 1, (uint)KeyModifiers.Control, (uint)Keyboard.VirtualKeyStates.L);
        }

        public IntPtr GetNativeHandle()
        {
            return IntPtr.Zero;
        }

        public void Run()
        {
            RunMessageLoopInternal();
            CefRuntime.Shutdown();
        }

        public Size GetWindowClientSize()
        {
            return GetClientSize();
        }

        public void ResizeBrowser(IntPtr browserWindow, int width, int height)
        {
            if (browserWindow != IntPtr.Zero)
            {
                SetWindowPos(browserWindow, IntPtr.Zero, 0, 0, width, height, SetWindowPosFlags.IgnoreZOrder);
            }
        }

        public void Exit()
        {
            if (_handle != IntPtr.Zero)
            {
                SendMessage(_handle, (int)WM.SYSCOMMAND, (IntPtr)SysCommand.SC_CLOSE, IntPtr.Zero);
            }
        }

        public void MessageBox(string message, int type)
        {
        }

        #region CreateWindow
        private IntPtr GetIconHandle()
        {
            var hIcon = LoadIconFromFile(_config.WindowIconFile);
            return hIcon ?? LoadIcon(IntPtr.Zero, (IntPtr)IDI_APPLICATION);
        }

        private WinNativeMethods.RECT GetWindowBounds()
        {
            var styles = GetWindowStyles(_config.WindowState);
            var bounds = new System.Drawing.Rectangle(_config.WindowLeft, _config.WindowTop, _config.WindowWidth, _config.WindowHeight);

            switch (_config.WindowState)
            {
                case WindowState.Maximize:
                case WindowState.Normal:
                    break;

                case WindowState.Fullscreen:
                    bounds = FullScreenBounds(bounds);
                    break;
            }

            WinNativeMethods.RECT rect;
            rect.Left = bounds.Left;
            rect.Top = bounds.Top;
            rect.Right = bounds.Right;
            rect.Bottom = bounds.Bottom;

            AdjustWindowRectEx(ref rect, styles.Item1, false, styles.Item2);

            return rect;
        }

        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommand> GetWindowStyles(WindowState state)
        {
            if (_config.UseWindowCustomStyle && _config != null && _config.WindowCustomStyle.IsValid())
            {
                return GetWindowStyles(_config.WindowCustomStyle, state);
            }

            var styles = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            var exStyles = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;

            if (_config.WindowNoResize)
            {
                styles = WindowStyles.WS_OVERLAPPEDWINDOW & ~WindowStyles.WS_THICKFRAME | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_config.WindowNoMinMaxBoxes)
            {
                styles &= ~WindowStyles.WS_MINIMIZEBOX;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_config.WindowFrameless)
            {
                styles = WindowStyles.WS_POPUPWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles |= WindowStyles.WS_SIZEBOX;
            }

            if (_config.WindowKioskMode)
            {
                styles &= ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME);
                exStyles &= ~(WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_WINDOWEDGE | WindowExStyles.WS_EX_CLIENTEDGE | WindowExStyles.WS_EX_STATICEDGE);
            }

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWMAXIMIZED);
                    }

                case WindowState.Fullscreen:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        exStyles = WindowExStyles.WS_EX_TOOLWINDOW;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWMAXIMIZED);
                    }
            }

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWNORMAL);
        }

        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommand> GetWindowStyles(WindowCustomStyle customCreationStyle, WindowState state)
        {
            var styles = (WindowStyles)customCreationStyle.WindowStyles;
            var exStyles = (WindowExStyles)customCreationStyle.WindowExStyles;

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWMAXIMIZED);

                case WindowState.Fullscreen:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWMAXIMIZED);
            }

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(styles, exStyles, ShowWindowCommand.SW_SHOWNORMAL);
        }

        private ShowWindowCommand GetShowWindowCommand(WindowState state)
        {
            switch (state)
            {
                case WindowState.Normal:
                    return ShowWindowCommand.SW_SHOWNORMAL;

                case WindowState.Maximize:
                case WindowState.Fullscreen:
                    return ShowWindowCommand.SW_SHOWMAXIMIZED;
            }

            return ShowWindowCommand.SW_SHOWNORMAL;
        }

        #endregion CreateWindow

        #region WndProc

        private IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {

            WM msg = (WM)message;
            switch (msg)
            {
                case WM.HOTKEY:
                    {
                        if (wParam == (IntPtr)1)
                        {
                            PostMessage(_handle, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
                            return IntPtr.Zero;
                        }
                        break;
                    }
                case WM.SYSKEYDOWN:
                    {
                        if (_config.WindowKioskMode && (wParam == (IntPtr)Keyboard.VirtualKeyStates.VK_F4))
                        {
                            return IntPtr.Zero;
                        }
                        break;
                    }

                case WM.CREATE:
                    {
                        NativeInstance = this;
                        _handle = hWnd;
                        var createdEvent = new CreatedEventArgs(IntPtr.Zero, _handle, _handle);
                        Created?.Invoke(this, createdEvent);
                        _isInitialized = true;
                        break;
                    }

                case WM.ERASEBKGND:
                    return new IntPtr(1);

                case WM.NCHITTEST:
                    if (_config.WindowFrameless)
                    {
                        return (IntPtr)HT_CAPTION;
                    }
                    break;

                case WM.MOVING:
                case WM.MOVE:
                    {
                        Moving?.Invoke(this, new MovingEventArgs());
                        return IntPtr.Zero;
                    }

                case WM.SIZE:
                    {
                        var size = GetClientSize();
                        SizeChanged?.Invoke(this, new SizeChangedEventArgs(size.Width, size.Height));
                        break;
                    }

                case WM.CLOSE:
                    {
                        if (_handle != IntPtr.Zero && _isInitialized)
                        {
                            Close?.Invoke(this, new CloseEventArgs());
                        }
                        PostQuitMessage(0);
                        Environment.Exit(0);
                        break;
                    }
            }

            return (DefWindowProc(hWnd, message, wParam, lParam));
          
        }

        private Size GetClientSize()
        {
            var size = new Size();
            if (_handle != IntPtr.Zero)
            {
                GetClientRect(_handle, out var rectangle);
                size.Width = rectangle.Width;
                size.Height = rectangle.Height;
            }

            return size;
        }

        #endregion WndProc

        #region Message Loop

        private static void RunMessageLoopInternal()
        {
            while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (msg.message == (uint)WM.CLOSE)
                {
                    DetachKeyboardHook();
                }

                if (NativeInstance._config.WindowKioskMode && msg.message == (uint)WM.HOTKEY && msg.wParam == (IntPtr)1)
                {
                    PostMessage(NativeInstance._handle, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                if (NativeInstance._config.WindowFrameless || NativeInstance._config.WindowKioskMode)
                {
                    CefRuntime.DoMessageLoopWork();
                }

                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        #endregion

        /// <summary>
        /// The win.
        /// </summary>
        private static class NativeMethods
        {
        }
    }
}