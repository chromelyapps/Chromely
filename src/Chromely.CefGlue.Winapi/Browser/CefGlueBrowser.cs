// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowser.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using Chromely.CefGlue.Winapi.Browser.EventParams;
    using Chromely.CefGlue.Winapi.Browser.FrameHandlers;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue browser.
    /// </summary>
    public class CefGlueBrowser
    {
        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// The CefBrowserSettings object.
        /// </summary>
        private CefBrowserSettings mSettings;

        /// <summary>
        /// The CefGlueClient object.
        /// </summary>
        private CefGlueClient mClient;

        /// <summary>
        /// The m websocket started.
        /// </summary>
        private bool mWebsocketStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowser"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public CefGlueBrowser(object owner, ChromelyConfiguration hostConfig, CefBrowserSettings settings)
        {
            Owner = owner;
            mHostConfig = hostConfig;
            mSettings = settings;
            StartUrl = hostConfig.StartUrl;
        }

        #region Events Handling Properties

        /// <summary>
        /// The created.
        /// </summary>
        public event EventHandler Created;

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
        public event EventHandler<LoadingStateChangeEventArgs> LoadingStateChanged;

        /// <summary>
        /// The tooltip changed.
        /// </summary>
        public event EventHandler<TooltipEventArgs> TooltipChanged;

        /// <summary>
        /// The before close.
        /// </summary>
        public event EventHandler<BeforeCloseEventArgs> BeforeClose;

        /// <summary>
        /// The before popup.
        /// </summary>
        public event EventHandler<BeforePopupEventArgs> BeforePopup;

        /// <summary>
        /// The frame load started.
        /// </summary>
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;

        /// <summary>
        /// The frame load end.
        /// </summary>
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;

        /// <summary>
        /// The load error.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;

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
                    if (instance is CefGlueBrowser browser)
                    {
                        return browser;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        public object Owner { get; }

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
        /// Gets the cef browser.
        /// </summary>
        public CefBrowser CefBrowser { get; private set; }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="windowInfo">
        /// The window info.
        /// </param>
        public void Create(CefWindowInfo windowInfo)
        {
            if (mClient == null)
            {
                IoC.RegisterInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName, this);
                mClient = new CefGlueClient(CefGlueClientParams.Create(this));
            }

            mSettings = this.mSettings ?? new CefBrowserSettings();

            mSettings.DefaultEncoding = "UTF-8";
            mSettings.FileAccessFromFileUrls = CefState.Enabled;
            mSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            mSettings.WebSecurity = CefState.Enabled;

            CefBrowserHost.CreateBrowser(windowInfo, mClient, mSettings, StartUrl);
        }

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            if (mWebsocketStarted)
            {
                WebsocketServerRunner.StopServer();
            }

            if (CefBrowser != null)
            {
                var host = CefBrowser.GetHost();
                host.CloseBrowser(true);
                host.Dispose();
                CefBrowser.Dispose();
                CefBrowser = null;
            }
        }

        #region Events Handling

        /// <summary>
        /// The on created.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public virtual void OnBrowserAfterCreated(CefBrowser browser)
        {
            CefBrowser = browser;

            // Register browser 
            CefGlueFrameHandler frameHandler = new CefGlueFrameHandler(browser);
            IoC.RegisterInstance(typeof(CefGlueFrameHandler), typeof(CefGlueFrameHandler).FullName, frameHandler);

            StartWebsocket();
            Created?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The on title changed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnTitleChanged(TitleChangedEventArgs eventArgs)
        {
            Title = eventArgs.Title;
            TitleChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on address changed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnAddressChanged(AddressChangedEventArgs eventArgs)
        {
            Address = eventArgs.Address;
            AddressChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on status message.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnStatusMessage(StatusMessageEventArgs eventArgs)
        {
            StatusMessage?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on console message.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnConsoleMessage(ConsoleMessageEventArgs eventArgs)
        {
            ConsoleMessage?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on loading state change.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadingStateChange(LoadingStateChangeEventArgs eventArgs)
        {
            LoadingStateChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on tooltip.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnTooltip(TooltipEventArgs eventArgs)
        {
            TooltipChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on before close.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnBeforeClose(BeforeCloseEventArgs eventArgs)
        {
            BeforeClose?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on before popup.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnBeforePopup(BeforePopupEventArgs eventArgs)
        {
            BeforePopup?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on frame load start.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnFrameLoadStart(FrameLoadStartEventArgs eventArgs)
        {
            FrameLoadStart?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on load end.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnFrameLoadEnd(FrameLoadEndEventArgs eventArgs)
        {
            FrameLoadEnd?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on load error.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadError(LoadErrorEventArgs eventArgs)
        {
            LoadError?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on plugin crashed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnPluginCrashed(PluginCrashedEventArgs eventArgs)
        {
            PluginCrashed?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The on render process terminated.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnRenderProcessTerminated(RenderProcessTerminatedEventArgs eventArgs)
        {
            RenderProcessTerminated?.Invoke(this, eventArgs);
        }

        #endregion Events Handling

        /// <summary>
        /// The start websocket.
        /// </summary>
        private void StartWebsocket()
        {
            try
            {
                if (mHostConfig.StartWebSocket)
                {
                    WebsocketServerRunner.StartServer(mHostConfig.WebsocketAddress, mHostConfig.WebsocketPort);
                    mWebsocketStarted = true;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }
    }
}
