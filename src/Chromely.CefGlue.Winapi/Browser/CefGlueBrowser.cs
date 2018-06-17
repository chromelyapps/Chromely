// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowser.cs" company="Chromely">
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
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using Chromely.CefGlue.Winapi.Browser.EventParams;
    using Chromely.CefGlue.Winapi.Browser.FrameHandlers;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue browser.
    /// </summary>
    public class CefGlueBrowser : WebBrowserBase
    {
        /// <summary>
        /// The CefBrowserConfig object.
        /// </summary>
        private readonly CefBrowserConfig mBrowserConfig;

        /// <summary>
        /// The CefBrowser object.
        /// </summary>
        private CefBrowser mBrowser;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr mBrowserWindowHandle;

        /// <summary>
        /// The m websocket started.
        /// </summary>
        private bool mWebsocketStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowser"/> class.
        /// </summary>
        /// <param name="browserConfig">
        /// The browser config.
        /// </param>
        public CefGlueBrowser(CefBrowserConfig browserConfig)
        {
            this.mBrowserConfig = browserConfig;
            this.StartUrl = string.IsNullOrEmpty(browserConfig.StartUrl) ? "about:blank" : browserConfig.StartUrl;

            this.CreateBrowser();
        }

        #region Events Handling Properties

        /// <summary>
        /// The browser created.
        /// </summary>
        public event EventHandler BrowserCreated;

        /// <summary>
        /// The title changed.
        /// </summary>
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        /// <summary>
        /// The address changed.
        /// </summary>
        public event EventHandler<AddressChangedEventArgs> AddressChanged;

        /// <summary>
        /// The status message.
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessage;

        /// <summary>
        /// The console message.
        /// </summary>
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        /// <summary>
        /// The loading state change.
        /// </summary>
        public event EventHandler<LoadingStateChangeEventArgs> LoadingStateChange;

        /// <summary>
        /// The tooltip.
        /// </summary>
        public event EventHandler<TooltipEventArgs> Tooltip;

        /// <summary>
        /// The before close.
        /// </summary>
        public event EventHandler BeforeClose;

        /// <summary>
        /// The before popup.
        /// </summary>
        public event EventHandler<BeforePopupEventArgs> BeforePopup;

        /// <summary>
        /// The load end.
        /// </summary>
        public event EventHandler<LoadEndEventArgs> LoadEnd;

        /// <summary>
        /// The load error.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;

        /// <summary>
        /// The load started.
        /// </summary>
        public event EventHandler<LoadStartEventArgs> LoadStarted;

        /// <summary>
        /// The plugin crashed.
        /// </summary>
        public event EventHandler<PluginCrashedEventArgs> PluginCrashed;

        /// <summary>
        /// The render process terminated.
        /// </summary>
        public event EventHandler<RenderProcessTerminatedEventArgs> RenderProcessTerminated;

        #endregion Events Handling Properties

        /// <summary>
        /// Gets the browser core.
        /// </summary>
        public static CefGlueBrowser BrowserCore
        {
            get
            {
                if (IoC.IsRegistered(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName))
                {
                    var instance = IoC.GetInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName);
                    if (instance is CefGlueBrowser)
                    {
                        return (CefGlueBrowser)instance;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the start url.
        /// </summary>
        public string StartUrl { get; set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        public CefBrowserSettings BrowserSettings { get; set; }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public CefBrowser Browser => this.mBrowser;

        /// <summary>
        /// The resize window.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public virtual void ResizeWindow(int width, int height)
        {
            this.ResizeWindow(this.mBrowserWindowHandle, width, height);
        }

        #region Events Handling

        /// <summary>
        /// The on browser after created.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public virtual void OnBrowserAfterCreated(CefBrowser browser)
        {
            this.mBrowser = browser;
            this.mBrowserWindowHandle = this.mBrowser.GetHost().GetWindowHandle();

            // Register browser 
            CefGlueFrameHandler frameHandler = new CefGlueFrameHandler(browser);
            IoC.RegisterInstance(typeof(CefGlueFrameHandler), typeof(CefGlueFrameHandler).FullName, frameHandler);

            this.StartWebsocket();

            this.BrowserCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The on title changed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnTitleChanged(TitleChangedEventArgs eventArgs)
        {
            this.Title = eventArgs.Title;
            this.TitleChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on address changed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnAddressChanged(AddressChangedEventArgs eventArgs)
        {
            this.Address = eventArgs.Address;
            this.AddressChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on status message.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnStatusMessage(StatusMessageEventArgs eventArgs)
        {
            this.StatusMessage?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on console message.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnConsoleMessage(ConsoleMessageEventArgs eventArgs)
        {
            this.ConsoleMessage?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on loading state change.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadingStateChange(LoadingStateChangeEventArgs eventArgs)
        {
            this.LoadingStateChange?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on tooltip.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnTooltip(TooltipEventArgs eventArgs)
        {
            this.Tooltip?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on before close.
        /// </summary>
        public virtual void OnBeforeClose()
        {
            this.mBrowserWindowHandle = IntPtr.Zero;
            this.BeforeClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The on before popup.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnBeforePopup(BeforePopupEventArgs eventArgs)
        {
            this.BeforePopup?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on load end.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadEnd(LoadEndEventArgs eventArgs)
        {
            this.LoadEnd?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on load error.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadError(LoadErrorEventArgs eventArgs)
        {
            this.LoadError?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on load start.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadStart(LoadStartEventArgs eventArgs)
        {
            this.LoadStarted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on plugin crashed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnPluginCrashed(PluginCrashedEventArgs eventArgs)
        {
            this.PluginCrashed?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on render process terminated.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnRenderProcessTerminated(RenderProcessTerminatedEventArgs eventArgs)
        {
            this.RenderProcessTerminated?.Invoke(this, eventArgs);
        }

        #endregion Events Handling

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.mWebsocketStarted)
            {
                WebsocketServerRunner.StopServer();
            }

            if (this.mBrowser != null && disposing)
            {
                var host = this.mBrowser.GetHost();
                if (host != null)
                {
                    host.CloseBrowser();
                    host.Dispose();
                }

                this.mBrowser.Dispose();
                this.mBrowser = null;
                this.mBrowserWindowHandle = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        #endregion Dispose

        /// <summary>
        /// The create browser.
        /// </summary>
        private void CreateBrowser()
        {
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(this.mBrowserConfig.ParentHandle, this.mBrowserConfig.CefRectangle);

            var client = this.CreateWebClient();

            var settings = this.BrowserSettings ?? new CefBrowserSettings();

            settings.DefaultEncoding = "UTF-8";
            settings.FileAccessFromFileUrls = CefState.Enabled;
            settings.UniversalAccessFromFileUrls = CefState.Enabled;
            settings.WebSecurity = CefState.Enabled;

            CefBrowserHost.CreateBrowser(windowInfo, client, settings, this.StartUrl);
        }

        /// <summary>
        /// The create web client.
        /// </summary>
        /// <returns>
        /// The <see cref="CefGlueClient"/>.
        /// </returns>
        private CefGlueClient CreateWebClient()
        {
            IoC.RegisterInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName, this);
            var client = new CefGlueClient(CefGlueClientParams.Create(this));
            return client;
        }

        /// <summary>
        /// The resize window.
        /// </summary>
        /// <param name="handle">
        /// The handle.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        private void ResizeWindow(IntPtr handle, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.SetWindowPos(
                    handle,
                    IntPtr.Zero,
                    0,
                    0,
                    width,
                    height,
                    WinapiConstants.NoZOrder);
            }
        }

        /// <summary>
        /// The start websocket.
        /// </summary>
        private void StartWebsocket()
        {
            try
            {
                if (this.mBrowserConfig.StartWebSocket)
                {
                    WebsocketServerRunner.StartServer(this.mBrowserConfig.WebsocketAddress, this.mBrowserConfig.WebsocketPort);
                    this.mWebsocketStarted = true;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }
    }
}
