// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinApiWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using WinApi.User32;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    /// <summary>
    /// The window.
    /// </summary>
    public class Window : NativeWindow, IWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase mApplication;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private CefGlueBrowser mBrowser;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr mBrowserWindowHandle;
        private IntPtr mBrowserWndProc;
        private IntPtr mBrowserRenderWidgetHandle;
        private IntPtr mBrowserRenderWidgetWndProc;
        private List<ChildWindow> childWindows;
        private List<WndProcOverride> wndProcOverrides = new List<WndProcOverride>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public Window(HostBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig)
        {
            mHostConfig = hostConfig;
            mBrowser = new CefGlueBrowser(this, hostConfig, new CefBrowserSettings());
            mBrowser.Created += OnBrowserCreated;
            mApplication = application;

            // Set event handler
            mBrowser.SetEventHandlers();

            ShowWindow();
        }

        /// <summary>
        /// The browser.
        /// </summary>
        public CefGlueBrowser Browser => mBrowser;

        public void CenterToScreen()
        {
            base.CenterToScreen();
        }

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (mBrowser != null)
            {
                mBrowser.Dispose();
                mBrowser = null;
                mBrowserWindowHandle = IntPtr.Zero;
            }
        }

        #endregion Dispose

        /// <summary>
        /// The on realized.
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
        /// <exception cref="NotSupportedException">
        /// Exception returned for MacOS not supported.
        /// </exception>
        protected override void OnCreate(IntPtr hwnd, int width, int height)
        {
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(hwnd, new CefRectangle(0, 0, mHostConfig.HostWidth, mHostConfig.HostHeight));
            mBrowser.Create(windowInfo);
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
        protected override void OnSize(int width, int height)
        {
            if (mBrowserWindowHandle != IntPtr.Zero)
            {
                if (width == 0 && height == 0)
                {
                    // For windowed browsers when the frame window is minimized set the
                    // browser window size to 0x0 to reduce resource usage.
                    NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, 0, 0, WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOACTIVATE);
                }
                else
                {
                    NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height, WindowPositionFlags.SWP_NOZORDER);
                }
            }
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected override void OnExit()
        {
            mApplication.Quit();
        }

        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private struct ChildWindow
        {
            public IntPtr Handle;
            public string ClassName;
        }

        private class EnumChildWindowsDetails
        {
            public List<ChildWindow> Windows = new List<ChildWindow>();
        }

        private class WndProcOverride
        {
            private IntPtr handle;
            private IntPtr originalWndProc;
            private string className;
            private WindowProc newWndProc;

            public WndProcOverride(IntPtr wndHandle, string wndClassName)
            {
                handle = wndHandle;
                className = wndClassName;
                newWndProc = new WindowProc(OverridenWndProc);
                originalWndProc = User32Methods.SetWindowLongPtr(handle, -4, Marshal.GetFunctionPointerForDelegate(newWndProc));
            }

            private IntPtr OverridenWndProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
            {
                var msg = (WM)uMsg;
                var originalRet = User32Methods.CallWindowProc(originalWndProc, hWnd, uMsg, wParam, lParam);
                switch (msg)
                {
                    case WM.NCHITTEST:
                        {
                            // If we hit a non client area (our overlapped frame) then we'll force an NCHITTEST to bubble back up to the main process
                            IntPtr hitTest = HitTestNCA(hWnd, wParam, lParam);
                            if (hitTest != IntPtr.Zero)
                            {
                                return new IntPtr(-1);
                            }
                            break;
                        }
                }
                return originalRet;
            }
        }

        /// <summary>
        /// The browser created.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnBrowserCreated(object sender, EventArgs e)
        {
            mBrowserWindowHandle = mBrowser.CefBrowser.GetHost().GetWindowHandle();
            if (mBrowserWindowHandle != IntPtr.Zero)
            {
                var size = GetClientSize();
                NativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, size.Width, size.Height, WindowPositionFlags.SWP_NOZORDER);

                var childWindowsDetails = new EnumChildWindowsDetails();
                var gcHandle = GCHandle.Alloc(childWindowsDetails);
                EnumChildWindows(Handle, new EnumWindowProc(EnumWindow), GCHandle.ToIntPtr(gcHandle));

                foreach (ChildWindow childWindow in childWindowsDetails.Windows)
                {
                    var wndProcOverride = new WndProcOverride(childWindow.Handle, childWindow.ClassName);
                    wndProcOverrides.Add(wndProcOverride);
                }

                childWindows = childWindowsDetails.Windows;

                gcHandle.Free();
            }
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            var buffer = new StringBuilder(128);
            User32Methods.GetClassName(hWnd, buffer, buffer.Capacity);

            var childWindow = new ChildWindow()
            {
                Handle = hWnd,
                ClassName = buffer.ToString()
            };

            var gcHandleDetails = GCHandle.FromIntPtr(lParam);
            var details = (EnumChildWindowsDetails)gcHandleDetails.Target;
            details.Windows.Add(childWindow);

            return true;
        }
    }
}
