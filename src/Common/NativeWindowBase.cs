// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using NetCoreEx.Geometry;
using WinApi.Gdi32;
using WinApi.Kernel32;
using WinApi.User32;

namespace Chromely.Common
{
    /// <summary>
    /// The native window base.
    /// </summary>
    internal class NativeWindowBase
    {
        private static readonly NativeMethods.LowLevelKeyboardProc _hookCallback = HookCallback;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration _hostConfig;

        /// <summary>
        /// WindowProc ref : prevent GC Collect
        /// </summary>
        private WindowProc _windowProc;

        private IntPtr _hookID = IntPtr.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindowBase"/> class.
        /// </summary>
        public NativeWindowBase()
        {
            Handle = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindowBase"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// Chromely configuration.
        /// </param>
        public NativeWindowBase(ChromelyConfiguration hostConfig)
        {
            Handle = IntPtr.Zero;
            _hostConfig = hostConfig;
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The exit.
        /// </summary>
        public static void Exit()
        {
            if (IoC.GetInstance(typeof(IChromelyWindow), typeof(IChromelyWindow).FullName) is IChromelyWindow window)
            {
                User32Methods.SendMessage(window.Handle, (int)WM.SYSCOMMAND, (IntPtr)SysCommand.SC_CLOSE, IntPtr.Zero);
            }
        }

        /// <summary>
        /// The show window.
        /// </summary>
        public void ShowWindow()
        {
            CreateWindow();
        }

        /// <summary>
        /// The close window externally.
        /// </summary>
        public void CloseWindowExternally()
        {
            User32Methods.SendMessage(Handle, (int)WM.SYSCOMMAND, (IntPtr)SysCommand.SC_CLOSE, IntPtr.Zero);
        }

        /// <summary>
        /// The get client size.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/>.
        /// </returns>
        public Size GetClientSize()
        {
            var size = new Size();
            if (Handle != IntPtr.Zero)
            {
                User32Methods.GetClientRect(Handle, out var rectangle);
                size.Width = rectangle.Width;
                size.Height = rectangle.Height;
            }

            return size;
        }

        /// <summary>
        /// The get window size.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/>.
        /// </returns>
        public Size GetWindowSize()
        {
            var size = new Size();
            if (Handle != IntPtr.Zero)
            {
                User32Methods.GetWindowRect(Handle, out var rectangle);
                size.Width = rectangle.Width;
                size.Height = rectangle.Height;
            }

            return size;
        }

        /// <summary>
        /// The center to screen.
        /// </summary>
        /// <param name="useWorkArea">
        /// The use work area.
        /// </param>
        public void CenterToScreen(bool useWorkArea = true)
        {
            var monitor = User32Methods.MonitorFromWindow(Handle, MonitorFlag.MONITOR_DEFAULTTONEAREST);
            User32Helpers.GetMonitorInfo(monitor, out var monitorInfo);
            var screenRect = useWorkArea ? monitorInfo.WorkRect : monitorInfo.MonitorRect;
            var midX = screenRect.Width / 2;
            var midY = screenRect.Height / 2;
            var size = GetWindowSize();
            var left = midX - (size.Width / 2);
            var top = midY - (size.Height / 2);

            User32Methods.SetWindowPos(
                Handle,
                IntPtr.Zero,
                left,
                top,
                -1,
                -1,
                WindowPositionFlags.SWP_NOACTIVATE | WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_NOZORDER);
        }

        /// <summary>
        /// The on create.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected virtual void OnCreate(IntPtr hwnd, int width, int height)
        {
        }

        /// <summary>
        /// The on moving.
        /// </summary>
        protected virtual void OnMoving()
        {
        }

        /// <summary>
        /// The on size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected virtual void OnSize(int width, int height)
        {
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected virtual void OnExit()
        {
        }

        /// <summary>
        /// The create window.
        /// </summary>
        private void CreateWindow()
        {
            var instanceHandle = Kernel32Methods.GetModuleHandle(IntPtr.Zero);

            _windowProc = WindowProc;

            var wc = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                ClassName = "chromelywindow",
                CursorHandle = User32Helpers.LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                IconHandle = GetIconHandle(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW,
                BackgroundBrushHandle = new IntPtr((int)StockObject.WHITE_BRUSH),
                WindowProc = _windowProc,
                InstanceHandle = instanceHandle
            };

            var resReg = User32Methods.RegisterClassEx(ref wc);
            if (resReg == 0)
            {
                Log.Error("chromelywindow registration failed");
                return;
            }

            var styles = GetWindowStyles(_hostConfig.HostPlacement.State);

            var placement = _hostConfig.HostPlacement;

            NativeMethods.RECT rect;
            rect.Left = placement.Left;
            rect.Top = placement.Top;
            rect.Right = placement.Left + placement.Width;
            rect.Bottom = placement.Top + placement.Height;

            NativeMethods.AdjustWindowRectEx(ref rect, styles.Item1, false, styles.Item2);

            var hwnd = User32Methods.CreateWindowEx(
                styles.Item2,
                wc.ClassName,
                _hostConfig.HostPlacement.Frameless ? string.Empty : _hostConfig.HostTitle,
                styles.Item1,
                rect.Left,
                rect.Top,
                rect.Right - rect.Left,
                rect.Bottom - rect.Top,
                IntPtr.Zero,
                IntPtr.Zero,
                instanceHandle,
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                Log.Error("chromelywindow creation failed");
                return;
            }

            if (_hostConfig.HostPlacement.KioskMode)
            {
                //// Set new window style and size.
                var windowHDC = User32Methods.GetDC(Handle);
                var fullscreenWidth = Gdi32Methods.GetDeviceCaps(windowHDC, (int)DeviceCapsParams.HORZRES);
                var fullscreenHeight = Gdi32Methods.GetDeviceCaps(windowHDC, (int)DeviceCapsParams.VERTRES);
                User32Methods.ReleaseDC(Handle, windowHDC);

                User32Methods.SetWindowLongPtr(Handle, (int)WindowLongFlags.GWL_STYLE, (IntPtr)styles.Item1);
                User32Methods.SetWindowLongPtr(Handle, (int)WindowLongFlags.GWL_EXSTYLE, (IntPtr)styles.Item2);

                User32Methods.SetWindowPos(Handle, (IntPtr)HwndZOrder.HWND_TOP, 0, 0, fullscreenWidth, fullscreenHeight,
                    WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_FRAMECHANGED);

                User32Methods.ShowWindow(Handle, ShowWindowCommands.SW_MAXIMIZE);

                try
                {
                    this._hookID = NativeMethods.SetHook(_hookCallback);
                }
                catch
                {
                    DetachKeyboardHook();
                }
            }
            else
            {
                User32Methods.ShowWindow(Handle, styles.Item3);
            }
            User32Methods.UpdateWindow(Handle);

            User32Methods.RegisterHotKey(IntPtr.Zero, 1, KeyModifierFlags.MOD_CONTROL, VirtualKey.L);

        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {

                var msg = (WM)wParam;
                var hookInfo = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);

                var key = (VirtualKey)hookInfo.vkCode;



                bool alt = User32Methods.GetKeyState(VirtualKey.MENU).IsPressed;
                bool control = User32Methods.GetKeyState(VirtualKey.CONTROL).IsPressed;

                //if (alt && key == VirtualKey.F4)
                //{
                //    Application.Current.Shutdown();
                //    return (IntPtr)1; // Handled.
                //}

                if (!AllowKeyboardInput(alt, control, key))
                {
                    return (IntPtr)1; // Handled.
                }
            }

            return NativeMethods.CallNextHookEx(NativeInstance._hookID, nCode, wParam, lParam);
        }

        /// <summary>Determines whether the specified keyboard input should be allowed to be processed by the system.</summary>
        /// <remarks>Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.</remarks>
        public static bool AllowKeyboardInput(bool alt, bool control, VirtualKey key)
        {
            // Disallow various special keys.
            if (key <= VirtualKey.BACK || key == VirtualKey.NONAME ||
                key == VirtualKey.MENU || key == VirtualKey.PAUSE ||
                key == VirtualKey.HELP)
            {
                return false;
            }

            // Disallow ranges of special keys.
            // Currently leaves volume controls enabled; consider if this makes sense.
            // Disables non-existing Keys up to 65534, to err on the side of caution for future keyboard expansion.
            if ((key >= VirtualKey.LWIN && key <= VirtualKey.SLEEP) ||
                (key >= VirtualKey.KANA && key <= VirtualKey.HANJA) ||
                (key >= VirtualKey.CONVERT && key <= VirtualKey.MODECHANGE) ||
                //(key >= VirtualKey.BROWSER_BACK && key <= VirtualKey.BROWSER_HOME) ||
                (key >= VirtualKey.MEDIA_NEXT_TRACK && key <= VirtualKey.LAUNCH_APP2) ||
                (key >= VirtualKey.PROCESSKEY && key <= (VirtualKey)65534))
            {
                return false;
            }

            // Disallow specific key combinations. (These component keys would be OK on their own.)
            if ((alt && key == VirtualKey.TAB) ||
                (alt && key == VirtualKey.SPACE) ||
                (control && key == VirtualKey.ESCAPE))
            {
                return false;
            }

            // Allow anything else (like letters, numbers, spacebar, braces, and so on).
            return true;
        }

        /// <summary>
        /// Detach the keyboard hook; call during shutdown to prevent calls as we unload
        /// </summary>
        protected static void DetachKeyboardHook()
        {
            if (NativeInstance._hookID != IntPtr.Zero)
                NativeMethods.UnhookWindowsHookEx(NativeInstance._hookID);
        }

        internal static NativeWindowBase NativeInstance { get; set; }

        /// <summary>
        /// The window proc.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="umsg">
        /// The umsg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private IntPtr WindowProc(IntPtr hwnd, uint umsg, IntPtr wParam, IntPtr lParam)
        {
            var msg = (WM)umsg;
            switch (msg)
            {
                case WM.HOTKEY:
                    {
                        if (wParam == (IntPtr)1)
                        {
                            User32Methods.PostMessage(Handle, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
                            return IntPtr.Zero;
                        }
                        break;
                    }
                case WM.SYSKEYDOWN:
                    {
                        if (_hostConfig.HostPlacement.KioskMode && (wParam == (IntPtr)VirtualKey.F4))
                        {
                            return IntPtr.Zero;
                        }
                        break;
                    }

                case WM.CREATE:
                    {
                        NativeInstance = this;
                        Handle = hwnd;
                        var size = GetClientSize();
                        OnCreate(hwnd, size.Width, size.Height);
                        break;
                    }

                case WM.ERASEBKGND:
                    return new IntPtr(1);

                case WM.NCPAINT:
                    {
                        if (_hostConfig.HostPlacement.Frameless && _hostConfig.HostPlacement.FramelessOptions.IsResizable)
                        {
                            return IntPtr.Zero;
                        }
                        break;
                    }

                case WM.NCACTIVATE:
                    {
                        if (_hostConfig.HostPlacement.Frameless && _hostConfig.HostPlacement.FramelessOptions.IsResizable)
                        {
                            var result = User32Methods.DefWindowProc(hwnd, umsg, wParam, new IntPtr(-1));
                            return result;
                        }
                        break;
                    }

                case WM.NCCALCSIZE:
                    {
                        if (_hostConfig.HostPlacement.Frameless && _hostConfig.HostPlacement.FramelessOptions.IsResizable)
                        {
                            var result = User32Methods.DefWindowProc(hwnd, umsg, wParam, lParam);
                            NativeMethods.NCCALCSIZE_PARAMS csp;
                            csp = (NativeMethods.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(
                                lParam,
                                typeof(NativeMethods.NCCALCSIZE_PARAMS));
                            csp.rgrc[0].Top -= _hostConfig.HostPlacement.FramelessOptions.WhiteStripeHeight; // # remove top whitestripe border!
                            Marshal.StructureToPtr(csp, lParam, false);
                            return result;
                        }
                        break;
                    }

                case WM.MOVING:
                case WM.MOVE:
                    {
                        OnMoving();
                        return  IntPtr.Zero;
                    }

                case WM.SIZE:
                    {
                        var size = GetClientSize();
                        OnSize(size.Width, size.Height);
                        break;
                    }

                case WM.CLOSE:
                    {
                        OnExit();
                        User32Methods.PostQuitMessage(0);
                        Environment.Exit(0);
                        break;
                    }
            }

            return User32Methods.DefWindowProc(hwnd, umsg, wParam, lParam);
        }

        /// <summary>
        /// The get window styles.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommands> GetWindowStyles(WindowState state)
        {
            if (_hostConfig.UseHostCustomCreationStyle)
            {
                var customCreationStyle = _hostConfig.HostCustomCreationStyle as WindowCreationStyle;
                if (customCreationStyle != null)
                {
                    return GetWindowStyles(customCreationStyle, state);
                }
            }

            var styles = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            var exStyles = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;

            if (_hostConfig.HostPlacement.NoResize)
            {
                styles = WindowStyles.WS_OVERLAPPEDWINDOW & ~WindowStyles.WS_THICKFRAME | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_hostConfig.HostPlacement.NoMinMaxBoxes)
            {
                styles &= ~WindowStyles.WS_MINIMIZEBOX;
                styles &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            if (_hostConfig.HostPlacement.Frameless)
            {
                styles = WindowStyles.WS_POPUPWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                if (_hostConfig.HostPlacement.FramelessOptions.IsResizable)
                {
                    styles |= WindowStyles.WS_SIZEBOX;
                }
            }

            if (_hostConfig.HostPlacement.KioskMode)
            {
                styles &= ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME);
                exStyles &= ~(WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_WINDOWEDGE | WindowExStyles.WS_EX_CLIENTEDGE | WindowExStyles.WS_EX_STATICEDGE);
            }

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
                    }

                case WindowState.Fullscreen:
                    {
                        styles |= WindowStyles.WS_MAXIMIZE;
                        exStyles = WindowExStyles.WS_EX_TOOLWINDOW;
                        return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
                    }
            }

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);
        }

        /// <summary>
        /// The get window styles.
        /// </summary>
        /// <param name="customCreationStyle">
        /// The custom style.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<WindowStyles, WindowExStyles, ShowWindowCommands> GetWindowStyles(WindowCreationStyle customCreationStyle, WindowState state)
        {
            var styles = customCreationStyle.WindowStyles;
            var exStyles = customCreationStyle.WindowExStyles;

            switch (state)
            {
                case WindowState.Normal:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);

                case WindowState.Maximize:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);

                case WindowState.Fullscreen:
                    return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWMAXIMIZED);
            }

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommands>(styles, exStyles, ShowWindowCommands.SW_SHOWNORMAL);
        }
        /// <summary>
        /// The get icon handle.
        /// </summary>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private IntPtr GetIconHandle()
        {
            var hIcon = NativeMethods.LoadIconFromFile(_hostConfig.HostIconFile);
            return hIcon ?? User32Helpers.LoadIcon(IntPtr.Zero, SystemIcon.IDI_APPLICATION);
        }
    }
}