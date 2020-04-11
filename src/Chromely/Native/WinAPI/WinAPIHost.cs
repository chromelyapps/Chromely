using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
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

        protected static readonly LowLevelKeyboardProc _hookCallback = HookCallback;

        protected IWindowOptions _options;
        protected IntPtr _handle;
        protected bool _isInitialized;

        protected static string ClassName = "chromelywindow";
        protected static IntPtr _consoleParentInstance;
        protected WndProc _wndProc;
        protected IntPtr _hookID = IntPtr.Zero;

        public WinAPIHost()
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
        }

        public IntPtr Handle => _handle;

        public virtual void CreateWindow(IWindowOptions options, bool debugging)
        {
            _options = options;
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

            var styles = GetWindowStyles(_options.WindowState);

            WinNativeMethods.RECT rect;
            rect.Left = _options.Position.X;
            rect.Top = _options.Position.Y;
            rect.Right = _options.Position.X + _options.Size.Width;
            rect.Bottom = _options.Position.Y + _options.Size.Height;

            AdjustWindowRectEx(ref rect, styles.Item1, false, styles.Item2);

            var hWnd = CreateWindowEx(styles.Item2,
                        ClassName,
                        _options.Title,
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

            if (_options.KioskMode)
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
                if (_options.WindowState == WindowState.Normal && _options.StartCentered)
                {
                    CenterWindowToScreen(hWnd);
                }

                ShowWindow(hWnd, styles.Item3);
            }

            UpdateWindow(hWnd);
            RegisterHotKey(IntPtr.Zero, 1, (uint)KeyModifiers.Control, (uint)Keyboard.VirtualKeyStates.L);
        }

        public virtual IntPtr GetNativeHandle()
        {
            return IntPtr.Zero;
        }

        public virtual void Run()
        {
            RunMessageLoopInternal();
            CefRuntime.Shutdown();
        }

        public virtual Size GetWindowClientSize()
        {
            return GetClientSize();
        }

        /// <summary>
        ///     Under windows the x / y click events can be scaled based on the dpi / scaling setting.
        /// </summary>
        /// <returns> The window scaling factor. </returns>
        public virtual float GetWindowDpiScalingFactor() {
            // The returned value is the dpi, dividing by 96f results in the scale
            var windowscale = GetDpiForWindow(_handle) / 96f;
            return windowscale;
        }

        public virtual void ResizeBrowser(IntPtr browserWindow, int width, int height)
        {
            if (browserWindow != IntPtr.Zero)
            {
                SetWindowPos(browserWindow, IntPtr.Zero, 0, 0, width, height, SetWindowPosFlags.IgnoreZOrder);
            }
        }

        public virtual void Exit()
        {
            if (_handle != IntPtr.Zero)
            {
                try
                {
                    ShowWindow(_handle, ShowWindowCommand.SW_HIDE);
                    CefRuntime.Shutdown();
                    Task.Run(() =>
                    {
                        int excelProcessId = -1;
                        GetWindowThreadProcessId(_handle, ref excelProcessId);
                        var process = Process.GetProcessById(excelProcessId);
                        if (process != null)
                        {
                            process.Kill();
                        }
                    });
                }
                catch {}
            }
        }

        public virtual void MessageBox(string message, int type)
        {
        }

        #region CreateWindow

        private IntPtr GetIconHandle()
        {
            var hIcon = LoadIconFromFile(_options.RelativePathToIconFile);
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
                    hIcon = LoadIconFromFile(_options.RelativePathToIconFile);
                }
            }
            catch
            {
                // ignore
            }
            return hIcon ?? LoadIcon(IntPtr.Zero, (IntPtr)IDI_APPLICATION);
        }

        protected virtual WinNativeMethods.RECT GetWindowBounds()
        {
            var styles = GetWindowStyles(_options.WindowState);
            var bounds = new System.Drawing.Rectangle(_options.Position.X, _options.Position.Y, _options.Size.Width, _options.Size.Height);

            switch (_options.WindowState)
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

        protected virtual Tuple<WindowStyles, WindowExStyles, ShowWindowCommand> GetWindowStyles(WindowState state)
        {
            if (_options.UseCustomStyle && _options != null && _options.CustomStyle.IsValid())
            {
                return GetWindowStyles(_options.CustomStyle, state);
            }

            var styles = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            var exStyles = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;

            if (_options.DisableResizing)
            {
                styles = WindowStyles.WS_OVERLAPPEDWINDOW & ~WindowStyles.WS_THICKFRAME | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_options.DisableMinMaximizeControls)
            {
                styles &= ~WindowStyles.WS_MINIMIZEBOX;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_options.WindowFrameless)
            {
                styles = WindowStyles.WS_POPUPWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles |= WindowStyles.WS_SIZEBOX;
            }

            if (_options.KioskMode)
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

        protected virtual Tuple<WindowStyles, WindowExStyles, ShowWindowCommand> GetWindowStyles(WindowCustomStyle customCreationStyle, WindowState state)
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

        protected virtual ShowWindowCommand GetShowWindowCommand(WindowState state)
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

        protected virtual IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
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
                        if (_options.KioskMode && (wParam == (IntPtr)Keyboard.VirtualKeyStates.VK_F4))
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
                    if (_options.WindowFrameless)
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

        protected virtual Size GetClientSize()
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

        protected static void RunMessageLoopInternal()
        {
            while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (msg.message == (uint)WM.CLOSE)
                {
                    DetachKeyboardHook();
                }

                if (NativeInstance._options.KioskMode && msg.message == (uint)WM.HOTKEY && msg.wParam == (IntPtr)1)
                {
                    PostMessage(NativeInstance._handle, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                if (NativeInstance._options.WindowFrameless || NativeInstance._options.KioskMode)
                {
                    CefRuntime.DoMessageLoopWork();
                }

                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        #endregion
    }
}