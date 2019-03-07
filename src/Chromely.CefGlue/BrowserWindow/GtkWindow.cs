// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GtkWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.Browser;
using Chromely.CefGlue.BrowserWindow;

namespace Chromely.CefGlue.Gtk.BrowserWindow
{
    using System;
    using Chromely.CefGlue.Browser;
    using Chromely.Core;
    using Xilium.CefGlue;

    /// <summary>
    /// The window.
    /// </summary>
    public class GtkWindow : GtkNativeWindow, IWindow
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

        /// <summary>
        /// Initializes a new instance of the <see cref="GtkWindow"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public GtkWindow(HostBase application, ChromelyConfiguration hostConfig)
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
            //TODO: Implement
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
                    var parentHandle = HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, mHostConfig.HostWidth, mHostConfig.HostHeight)); 
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(HostXid, new CefRectangle(0, 0, mHostConfig.HostWidth, mHostConfig.HostHeight));
                    break;

                case CefRuntimePlatform.MacOSX:
                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }

            mBrowser.Create(windowInfo);
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
                if (mBrowserWindowHandle != IntPtr.Zero)
                {
                    // ReSharper disable once InlineOutVariableDeclaration
                    int width;
                    // ReSharper disable once InlineOutVariableDeclaration
                    int height;
                    GetSize(out width, out height);

                    GtkNativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height);
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
            mApplication.Quit();
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
            mBrowserWindowHandle = mBrowser.CefBrowser.GetHost().GetWindowHandle();
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                if (mBrowserWindowHandle != IntPtr.Zero)
                {
                    // ReSharper disable once InlineOutVariableDeclaration
                    int width;
                    // ReSharper disable once InlineOutVariableDeclaration
                    int height;
                    GetSize(out width, out height);

                    GtkNativeMethods.SetWindowPos(mBrowserWindowHandle, IntPtr.Zero, 0, 0, width, height);
                }
            }
        }

    }
}
