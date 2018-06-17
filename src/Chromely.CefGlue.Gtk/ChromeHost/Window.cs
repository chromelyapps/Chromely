// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.ChromeHost
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
        private readonly HostBase mApplication;

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
        public Window(HostBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig.HostTitle, hostConfig.HostWidth, hostConfig.HostHeight, hostConfig.HostIconFile)
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
        }
    }
}
