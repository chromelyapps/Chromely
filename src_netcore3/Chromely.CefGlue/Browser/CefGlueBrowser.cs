// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowser.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Network;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.Browser
{
    /// <summary>
    /// The CefGlue browser.
    /// </summary>
    public class CefGlueBrowser
    {
        private readonly IChromelyContainer _container;
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyCommandTaskRunner _commandTaskRunner;
        private readonly CefMessageRouterBrowserSide _browserMessageRouter;
        private CefBrowserSettings _settings;
        private CefGlueClient _client;

        public CefGlueBrowser(object owner, IChromelyContainer container, IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefMessageRouterBrowserSide browserMessageRouter, CefBrowserSettings settings)
        {
            Owner = owner;
            _container = container;
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browserMessageRouter = browserMessageRouter;
            _settings = settings;
            StartUrl = config.StartUrl;
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
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;

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
            if (_client == null)
            {
                var handlers = CefGlueCustomHandlers.Parse(_container, _config, _commandTaskRunner, this);
                _client = new CefGlueClient(this, _browserMessageRouter, handlers);
            }
            if (_settings == null)
            {
                _settings = new CefBrowserSettings();
            }

            _settings.DefaultEncoding = "UTF-8";
            _settings.FileAccessFromFileUrls = CefState.Enabled;
            _settings.UniversalAccessFromFileUrls = CefState.Enabled;
            _settings.WebSecurity = CefState.Disabled;

            CefBrowserHost.CreateBrowser(windowInfo, _client, _settings, StartUrl);
        }

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            unsafe
            {
                // due we don't want to change Xilium.CefGlue.CefBrowser
                // we check the internal _self property to see
                // if it is already destroyed
                if (CefBrowser != null 
                    && typeof(CefBrowser).GetField("_self", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(CefBrowser) is Pointer self 
                    && Pointer.Unbox(self) != null)
                {
                    var host = CefBrowser.GetHost();
                    host.CloseBrowser(true);
                    host.Dispose();
                    CefBrowser.Dispose();
                    CefBrowser = null;
                }
            }

        }

        #endregion

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

            // Register JavaScriptExecutor
            _config.JavaScriptExecutor = new JavaScriptExecutor(CefBrowser);

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
        public virtual void OnLoadingStateChange(LoadingStateChangedEventArgs eventArgs)
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
        /// The on load start.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        public virtual void OnLoadStart(FrameLoadStartEventArgs eventArgs)
        {
            FrameLoadStart?.Invoke(this, eventArgs);
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
    }
}
