// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.App
{
    using System;
    using Chromely.CefGlue.Gtk.Browser;
    using Chromely.Core;
    using Xilium.CefGlue;

    /// <summary>
    /// The window.
    /// </summary>
    public class Window : NativeWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly ApplicationBase mApplication;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser mCore;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr mBrowserWindowHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public Window(ApplicationBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig)
        {
            this.mHostConfig = hostConfig;
            this.mCore = new CefGlueBrowser(this, hostConfig, new CefBrowserSettings());
            this.mCore.Created += this.BrowserCreated;
            this.mApplication = application;

            this.ShowWindow();
        }

        /// <summary>
        /// The web browser.
        /// </summary>
        public CefGlueBrowser WebBrowser => this.mCore;

        #region Close/Dispose

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (this.mCore != null)
            {
                var browser = this.mCore.CefBrowser;
                var host = browser.GetHost();
                host.CloseBrowser();
                host.Dispose();
                browser.Dispose();
                this.mBrowserWindowHandle = IntPtr.Zero;
            }
        }

        #endregion Close/Dispose

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
        protected override void OnRealized(object sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    var parentHandle = this.HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, this.mHostConfig.HostWidth, this.mHostConfig.HostHeight));
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(this.HostXid, new CefRectangle(0, 0, this.mHostConfig.HostWidth, this.mHostConfig.HostHeight));
                    break;

                case CefRuntimePlatform.MacOSX:
                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }

            this.mCore.Create(windowInfo);
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
        protected override void OnResize(object sender, EventArgs e)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                if (this.mBrowserWindowHandle != IntPtr.Zero)
                {
                    // ReSharper disable once InlineOutVariableDeclaration
                    int width;
                    // ReSharper disable once InlineOutVariableDeclaration
                    int height;
                    this.GetSize(out width, out height);

                    NativeMethods.SetWindowPos(this.mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height);
                }
            }
            else
            {
                base.OnResize(sender, e);
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
        protected override void OnExit(object sender, EventArgs e)
        {
            this.mApplication.Quit();
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
        private void BrowserCreated(object sender, EventArgs e)
        {
            this.mBrowserWindowHandle = this.mCore.CefBrowser.GetHost().GetWindowHandle();

            // ReSharper disable once InlineOutVariableDeclaration
            int width;
            // ReSharper disable once InlineOutVariableDeclaration
            int height;
            this.GetSize(out width, out height);

            NativeMethods.SetWindowPos(this.mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height);
        }
    }
}
