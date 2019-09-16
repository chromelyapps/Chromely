// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromeWidgetMessageInterceptor.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinApi.User32;
using Chromely.Core.Host;
using NetCoreEx.Geometry;

namespace Chromely.Common
{
    /// <summary>
    /// Intercepts Windows messages sent to the browser's widget sub-window.
    /// 
    /// It is necessary to listen to the widget sub-window because it receives all Windows messages
    /// and forwards them to CEF, rather than the Window's handle.
    /// 
    /// The supplied Action delegate is fired upon each message.
    /// </summary>
    internal class ChromeWidgetMessageInterceptor
    {
        private static Action<Message> _forwardAction;
        private static IntPtr _parentHandle;
        private static IntPtr _childHandle;
        private static  WindowProc _childWndProc;
        private static IntPtr _oldWindProc;
        private static IntPtr _childWndProcHandle;
        private static IntPtr _prevWndProc;
        private IFramelessOptions _framelessOptions;

        private ChromeWidgetMessageInterceptor(IntPtr window, IntPtr childHandle, IFramelessOptions framelessOptions, Action<Message> forwardAction)
        {
            _parentHandle = window;
            _childHandle = childHandle;
            _childWndProc = WndProc;
            _framelessOptions = framelessOptions;

            _childWndProcHandle = Marshal.GetFunctionPointerForDelegate(_childWndProc);
            _oldWindProc = NativeMethods.SetWindowLongPtr(_childHandle, (int)WindowLongFlags.GWLP_WNDPROC, _childWndProcHandle);
            NativeMethods.SetWindowLongPtr(_childHandle, (int)WindowLongFlags.GWLP_USERDATA, _oldWindProc);
            _forwardAction = forwardAction;
        }

        /// <summary>
        /// Asynchronously wait for the Chromium widget window to be created for the given ChromiumWebBrowser,
        /// and when created hook into its Windows message loop.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="window">The browser window to intercept Windows messages for.</param>
        /// <param name="framelessOptions">The frameless options.</param>
        /// <param name="forwardAction">This action will be called whenever a Windows message is received.</param>
        internal static void Setup(ChromeWidgetMessageInterceptor interceptor, IntPtr window, IFramelessOptions framelessOptions, Action<Message> forwardAction)
        {
            Task.Run(() =>
            {
                try
                {
                    bool foundWidget = false;
                    while (!foundWidget)
                    {
                        if (ChromeWidgetHandleFinder.TryFindHandle(window, out var chromeWidgetHostHandle))
                        {
                            foundWidget = true;
                            interceptor = new ChromeWidgetMessageInterceptor(window, chromeWidgetHostHandle, framelessOptions, forwardAction);
                        }
                        else
                        {
                            // Chrome hasn't yet set up its message-loop window.
                            Thread.Sleep(10);
                        }
                    }
                }
                catch { }
            });
        }

        private IntPtr WndProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            _prevWndProc = User32Helpers.GetWindowLongPtr(hWnd, WindowLongFlags.GWLP_USERDATA);

            var msg = (WM)uMsg;
            switch (msg)
            {
                case WM.LBUTTONDOWN:

                    if (_forwardAction != null)
                    {
                        if (IsCursorInDraggableRegion(hWnd, lParam))
                        {
                            Message message = new Message();
                            message.Hwnd = hWnd;
                            message.Value = uMsg;
                            message.WParam = wParam;
                            message.LParam = lParam;
                            _forwardAction(message);

                            return IntPtr.Zero;
                        }
                    }
                    break;
            }

            return User32Methods.CallWindowProc(_prevWndProc, hWnd, uMsg, wParam, lParam);
        }

        private bool IsCursorInDraggableRegion(IntPtr hWnd, IntPtr lParam)
        {
            Point pt;
            pt.X = NativeMethods.LOWORD(lParam);
            pt.Y = NativeMethods.HIWORD(lParam);

            User32Methods.ClientToScreen(hWnd, ref pt);

            Rectangle rect;
            User32Methods.GetWindowRect(_parentHandle, out rect);

            // Mouse must be within Window
            if (!User32Methods.PtInRect(ref rect, pt))
            {
                return false;
            }

            Rectangle draggableRegion = new Rectangle
            {
                Left = rect.Left,
                Top = rect.Top,
                Right = rect.Right - _framelessOptions.NoDragWidth,
                Bottom = rect.Top + _framelessOptions.Height
            };

            // Mouse must be within draggable Region
            if (User32Methods.PtInRect(ref draggableRegion, pt))
            {
                return true;
            }

            return false;
        }
    }

    internal class ChromeWidgetHandleFinder
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private readonly IntPtr mainHandle;
        private string seekClassName;
        private IntPtr descendantFound;

        private ChromeWidgetHandleFinder(IntPtr handle)
        {
            this.mainHandle = handle;
        }

        private IntPtr FindDescendantByClassName(string className)
        {
            descendantFound = IntPtr.Zero;
            seekClassName = className;

            EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
            EnumChildWindows(this.mainHandle, childProc, IntPtr.Zero);

            return descendantFound;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            var buffer = new StringBuilder(128);
            User32Methods.GetClassName(hWnd, buffer, buffer.Capacity);

            if (buffer.ToString() == seekClassName)
            {
                descendantFound = hWnd;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Chrome's message-loop Window isn't created synchronously, so this may not find it.
        /// If so, you need to wait and try again later.
        /// </summary>
        public static bool TryFindHandle(IntPtr window, out IntPtr chromeWidgetHostHandle)
        {
            var windowHandleInfo = new ChromeWidgetHandleFinder(window);
            const string chromeWidgetHostClassName = "Chrome_RenderWidgetHostHWND";
            chromeWidgetHostHandle = windowHandleInfo.FindDescendantByClassName(chromeWidgetHostClassName);
            return chromeWidgetHostHandle != IntPtr.Zero;
        }
    }
}

