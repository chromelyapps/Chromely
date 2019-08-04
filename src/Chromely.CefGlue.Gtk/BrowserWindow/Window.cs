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
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Gtk.BrowserWindow
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

        /// <summary>
        /// The exit - externally/programatically.
        /// </summary>
        public void Exit()
        {
            _application?.Quit();
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
        protected override void OnRealized(object sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();
            var placement = _hostConfig.HostPlacement;
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    var parentHandle = HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, placement.Width, placement.Height)); 
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(HostXid, new CefRectangle(0, 0, placement.Width, placement.Height));
                    break;

                case CefRuntimePlatform.MacOSX:
                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }

            Browser.Create(windowInfo);
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
                if (_browserWindowHandle != IntPtr.Zero)
                {
                    GetSize(out var width, out var height);
                    NativeMethods.SetWindowPos(_browserWindowHandle, IntPtr.Zero, 0, 0, width, height);
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
            _application?.Quit();
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
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                if (_browserWindowHandle != IntPtr.Zero)
                {
                    // ReSharper disable once InlineOutVariableDeclaration
                    int width;
                    // ReSharper disable once InlineOutVariableDeclaration
                    int height;
                    GetSize(out width, out height);

                    NativeMethods.SetWindowPos(_browserWindowHandle, IntPtr.Zero, 0, 0, width, height);
                }
            }
        }

    }
}
