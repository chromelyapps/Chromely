// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    using System;
    using System.Runtime.InteropServices;

    using Core;
    using Core.Host;
    using Core.Infrastructure;
    using global::CefSharp;
    using NetCoreEx.Geometry;
    using WinApi.DwmApi;
    using WinApi.Gdi32;
    using WinApi.Kernel32;
    using WinApi.User32;

    /// <summary>
    /// The native window.
    /// </summary>
    public class NativeWindow
    {
        /// <summary>
        /// The m host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// WindowProc ref : prevent GC Collect
        /// </summary>
        private WindowProc mWindowProc;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        public NativeWindow()
        {
            Handle = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// Chromely configuration.
        /// </param>
        public NativeWindow(ChromelyConfiguration hostConfig)
        {
            Handle = IntPtr.Zero;
            mHostConfig = hostConfig;
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The run message loop.
        /// </summary>
        public static void RunMessageLoop()
        {
            while (User32Methods.GetMessage(out Message msg, IntPtr.Zero, 0, 0) != 0)
            {
                Cef.DoMessageLoopWork();

                User32Methods.TranslateMessage(ref msg);
                User32Methods.DispatchMessage(ref msg);
            }
        }

        /// <summary>
        /// The exit.
        /// </summary>
        public static void Exit()
        {
            User32Methods.PostQuitMessage(0);
        }

        /// <summary>
        /// The show window.
        /// </summary>
        public void ShowWindow()
        {
            CreateWindow();
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

            mWindowProc = WindowProc;
            
            var wc = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                ClassName = "chromelywindow",
                CursorHandle = User32Helpers.LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                IconHandle = GetIconHandle(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW,
                BackgroundBrushHandle = new IntPtr((int)StockObject.WHITE_BRUSH),
                WindowProc = mWindowProc,
                InstanceHandle = instanceHandle
            };

            var resReg = User32Methods.RegisterClassEx(ref wc);
            if (resReg == 0)
            {
                Log.Error("chromelywindow registration failed");
                return;
            }

            var styles = GetWindowStyles(mHostConfig.HostState);

            var hwnd = User32Methods.CreateWindowEx(
                styles.Item2,
                wc.ClassName,
                mHostConfig.HostTitle,
                styles.Item1,
                0,
                0,
                mHostConfig.HostWidth,
                mHostConfig.HostHeight,
                IntPtr.Zero,
                IntPtr.Zero,
                instanceHandle,
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                Log.Error("chromelywindow creation failed");
                return;
            }

            User32Methods.ShowWindow(Handle, styles.Item3);
            User32Methods.UpdateWindow(Handle);
        }

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
                case WM.ACTIVATE:
                    {
                        if (mHostConfig.HostFrameless)
                        {
                            Margins frameMargins = new Margins(7, 7, 27, 7);
                            DwmApiMethods.DwmExtendFrameIntoClientArea(Handle, ref frameMargins);
                            User32Methods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_NOOWNERZORDER | WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_FRAMECHANGED);
                        }
                        break;
                    }

                case WM.CREATE:
                    {
                        Handle = hwnd;
                        var size = GetClientSize();
                        OnCreate(hwnd, size.Width, size.Height);
                        break;
                    }

                case WM.ERASEBKGND:
                    return new IntPtr(1);

                case WM.SIZE:
                    {
                        var size = GetClientSize();
                        OnSize(size.Width, size.Height);
                        break;
                    }

                case WM.CLOSE:
                    {
                        OnExit();
                        Exit();
                        break;
                    }

                case WM.NCCALCSIZE:
                    {
                        if (mHostConfig.HostFrameless)
                        {
                            // Reasoning for checking wParam is listed here https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-nccalcsize
                            if (wParam.ToInt32() > 0)
                            {
                                return IntPtr.Zero;
                            }
                        }
                        break;
                    }

                case WM.NCHITTEST:
                    {
                        // This might be a bit redundant to perform and should find another way
                        // to pass the return value rather than performing a hit test again.
                        var lRet = HitTestNCA(hwnd, wParam, lParam);
                        return lRet;
                    }
            }

            return User32Methods.DefWindowProc(hwnd, umsg, wParam, lParam);
        }

        internal static IntPtr HitTestNCA(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            // Get the point coordinates for the hit test.
            Point mousePoint = new Point(lParam.ToInt32() >> 16, lParam.ToInt32() & 0xFFFF);

            // Get the window rectangle.
            Rectangle rectWindow;
            User32Methods.GetWindowRect(hWnd, out rectWindow);

            // Get the frame rectangle, adjusted for the style without a caption.
            Rectangle rectFrame = new Rectangle(4, 4, 27, 4);
            User32Methods.AdjustWindowRectEx(ref rectFrame, WindowStyles.WS_OVERLAPPEDWINDOW & ~WindowStyles.WS_CAPTION, false, 0);
            ushort row = 1;
            ushort col = 1;
            bool onTopResizeBorder = false;

            // Determine if the point is at the top or bottom of the window.
            if (mousePoint.Y >= rectWindow.Top && mousePoint.Y < rectWindow.Top + 27)
            {
                onTopResizeBorder = (mousePoint.Y < (rectWindow.Top - rectFrame.Top));
                row = 0;
            }
            else if (mousePoint.Y < rectWindow.Bottom && mousePoint.Y >= rectWindow.Bottom - 7)
            {
                row = 2;
            }

            // Determine if the point is at the left or right of the window.
            if (mousePoint.X >= rectWindow.Left && mousePoint.X < rectWindow.Left + 7)
            {
                col = 0;
            }
            else if (mousePoint.X < rectWindow.Right && mousePoint.X >= rectWindow.Right - 7)
            {
                col = 2;
            }

            // Defines the tests to determine what value to return for NCHITTEST
            int[,] hitTests =
            {
                { 13, onTopResizeBorder ? 12 : 2, 11 },
                { 10, 0, 11 },
                { 16, 15, 17 }
            };

            return (IntPtr)hitTests[row, col];
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
            var styles = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            var exStyles = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;

            if (mHostConfig.HostFrameless)
            {
                styles = WindowStyles.WS_CAPTION | WindowStyles.WS_POPUP | WindowStyles.WS_THICKFRAME | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_CAPTION | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
                //exStyles = WindowExStyles.WS_EX_TOOLWINDOW;
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
        /// The get icon handle.
        /// </summary>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private IntPtr GetIconHandle()
        {
            var hIcon = NativeMethods.LoadIconFromFile(mHostConfig.HostIconFile);
            return hIcon ?? User32Helpers.LoadIcon(IntPtr.Zero, SystemIcon.IDI_APPLICATION);
        }
    }
}