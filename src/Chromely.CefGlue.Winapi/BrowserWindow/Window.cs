// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Common;
using Chromely.Core;
using WinApi.User32;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    /// <summary>
    /// The window.
    /// </summary>
    internal class Window : NativeWindow, IWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase _application;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration _hostConfig;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr _browserWindowHandle;

        private ChromeWidgetMessageInterceptor _interceptor;

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
            _hostConfig = hostConfig;
            Browser = new CefGlueBrowser(this, hostConfig, new CefBrowserSettings());
            Browser.Created += OnBrowserCreated;
            _application = application;

            // Set event handler
            Browser.SetEventHandlers();

            ShowWindow();
        }

        /// <summary>
        /// The browser.
        /// </summary>
        public CefGlueBrowser Browser { get; private set; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr HostHandle => Handle;

        public void CenterToScreen()
        {
            base.CenterToScreen();
        }

        /// <inheritdoc />
        public new void Exit()
        {
            base.CloseWindowExternally();
        }

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (Browser != null)
            {
                Browser.Dispose();
                Browser = null;
                _browserWindowHandle = IntPtr.Zero;
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
            var placement = _hostConfig.HostPlacement;
            windowInfo.SetAsChild(hwnd, new CefRectangle(0, 0, placement.Width, placement.Height));
            Browser.Create(windowInfo);
        }

        /// <summary>
        /// The on moving.
        /// </summary>
        protected override void OnMoving()
        {
            if(_browserWindowHandle != IntPtr.Zero)
            {
                Browser.CefBrowser.GetHost().NotifyMoveOrResizeStarted();
            }
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
            if (_browserWindowHandle != IntPtr.Zero)
            {
                if (width == 0 && height == 0)
                {
                    // For windowed browsers when the frame window is minimized set the
                    // browser window size to 0x0 to reduce resource usage.
                    NativeMethods.SetWindowPos(_browserWindowHandle, IntPtr.Zero, 0, 0, 0, 0, WindowPositionFlags.SWP_NOZORDER | WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOACTIVATE);
                }
                else
                {
                    NativeMethods.SetWindowPos(_browserWindowHandle, IntPtr.Zero, 0, 0, width, height, WindowPositionFlags.SWP_NOZORDER);
                }
            }
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected override void OnExit()
        {
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
            _browserWindowHandle = Browser.CefBrowser.GetHost().GetWindowHandle();
            if (_browserWindowHandle != IntPtr.Zero)
            {
                var size = GetClientSize();
                NativeMethods.SetWindowPos(_browserWindowHandle, IntPtr.Zero, 0, 0, size.Width, size.Height, WindowPositionFlags.SWP_NOZORDER);

                if (_hostConfig.HostPlacement.Frameless && _hostConfig.HostPlacement.FramelessOptions.IsDraggable)
                {
                    ChromeWidgetMessageInterceptor.Setup(_interceptor, Handle, _hostConfig.HostPlacement.FramelessOptions, (message) =>
                    {
                        var msg = (WM)message.Value;
                        switch (msg)
                        {
                            case WM.LBUTTONDOWN:
                                User32Methods.ReleaseCapture();
                                NativeMethods.SendMessage(Handle, (int)WM.SYSCOMMAND, NativeMethods.SC_DRAGMOVE, 0);
                                break;
                        }

                    });
                }
            }
        }
    }
}
