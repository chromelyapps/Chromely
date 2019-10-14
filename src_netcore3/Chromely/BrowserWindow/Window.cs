using System;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Native;
using Xilium.CefGlue;

namespace Chromely.BrowserWindow
{
    /// <summary>
    /// The window.
    /// </summary>
    public class Window : NativeWindow, IWindow
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
        /// Gets the browser.
        /// </summary>
        public CefGlueBrowser Browser { get; private set; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr HostHandle => Handle;
         

        public void CenterToScreen()
        {
            //TODO: Implement
        }

        public void Exit()
        {
            Quit(_hostConfig.Platform);
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
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Exception returned for MacOS not supported.
        /// </exception>
        protected override void OnCreated(object sender, CreatedEventArgs createdEventArgs)
        {
            var windowInfo = CefWindowInfo.Create();
            var placement = _hostConfig.HostPlacement;
            windowInfo.SetAsChild(createdEventArgs.WinXID, new CefRectangle(0, 0, placement.Width, placement.Height));

            Browser.Create(windowInfo);
        }

        protected override void OnMoving(object sender, MovingEventArgs movingEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                Browser.CefBrowser.GetHost().NotifyMoveOrResizeStarted();
            }
        }

        /// <summary>
        /// The on resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                var nativeMethods = NativeGuiFactory.GetNativeGui(this._hostConfig.Platform);
                nativeMethods.ResizeWindow(_browserWindowHandle, sizeChangedEventArgs.Width, sizeChangedEventArgs.Height);
            }
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnClose(object sender, CloseEventArgs closeChangedEventArgs)
        {
        }

        /// <summary>
        /// The on browser created.
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
        }

    }
}