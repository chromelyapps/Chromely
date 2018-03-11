#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser
{
    using Chromely.Core.Infrastructure;
    using System;
    using Xilium.CefGlue;

    public class CefGlueBrowser
    {
        private readonly object m_owner;
        private readonly CefBrowserSettings m_settings;
        private string m_startUrl;
        private CefGlueClient m_client;
        private CefBrowser m_browser;

        private bool m_created;

        public CefGlueBrowser(object owner, CefBrowserSettings settings, string startUrl)
        {
            m_owner = owner;
            m_settings = settings;
            m_startUrl = startUrl;
        }

        public string StartUrl
        {
            get { return m_startUrl; }
            set { m_startUrl = value; }
        }

        public string Title { get; private set; }
        public string Address { get; private set; }

        public CefBrowser CefBrowser
        {
            get { return m_browser; }
        }

        public static CefGlueBrowser BrowserCore
        {
            get
            {
                if (IoC.IsRegistered(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName))
                {
                    object instance = IoC.GetInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName);
                    if ((instance != null) && (instance is CefGlueBrowser))
                    {
                        return (CefGlueBrowser)instance;
                    }
                }

                return null;
            }
        }

        public void Create(CefWindowInfo windowInfo)
        {
            if (m_client == null)
            {
                IoC.RegisterInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName, this);
                m_client = new CefGlueClient(CefGlueClientParams.Create(this));
            }

            CefBrowserHost.CreateBrowser(windowInfo, m_client, m_settings, StartUrl);
        }

        public void Close()
        {
            if (m_browser != null)
            {
                var host = m_browser.GetHost();
                host.CloseBrowser(true);
                host.Dispose();
                m_browser.Dispose();
                m_browser = null;
            }
        }

        #region Events Handling

        public event EventHandler Created;
        internal void OnCreated(CefBrowser browser)
        {
            m_created = true;
            m_browser = browser;

            Created?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        internal protected virtual void OnTitleChanged(TitleChangedEventArgs eventArgs)
        {
            Title = eventArgs.Title;
            TitleChanged?.Invoke(this, eventArgs);
        }

        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        internal protected virtual void OnAddressChanged(AddressChangedEventArgs eventArgs)
        {
            Address = eventArgs.Address;
            AddressChanged?.Invoke(this, eventArgs);
        }

        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        internal protected virtual void OnStatusMessage(StatusMessageEventArgs eventArgs)
        {
            StatusMessage?.Invoke(this, eventArgs);
        }

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        internal protected virtual void OnConsoleMessage(ConsoleMessageEventArgs eventArgs)
        {
            ConsoleMessage?.Invoke(this, eventArgs);
        }

        public event EventHandler<LoadingStateChangeEventArgs> LoadingStateChange;
        internal protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs eventArgs)
        {
            LoadingStateChange?.Invoke(this, eventArgs);
        }

        public event EventHandler<TooltipEventArgs> Tooltip;
        internal protected virtual void OnTooltip(TooltipEventArgs eventArgs)
        {
            Tooltip?.Invoke(this, eventArgs);
        }

        public event EventHandler BeforeClose;
        internal protected virtual void OnBeforeClose()
        {
            BeforeClose?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<BeforePopupEventArgs> BeforePopup;
        internal protected virtual void OnBeforePopup(BeforePopupEventArgs eventArgs)
        {
            BeforePopup?.Invoke(this, eventArgs);
        }

        public event EventHandler<LoadEndEventArgs> LoadEnd;
        internal protected virtual void OnLoadEnd(LoadEndEventArgs eventArgs)
        {
            LoadEnd?.Invoke(this, eventArgs);
        }

        public event EventHandler<LoadErrorEventArgs> LoadError;
        internal protected virtual void OnLoadError(LoadErrorEventArgs eventArgs)
        {
            LoadError?.Invoke(this, eventArgs);
        }

        public event EventHandler<LoadStartEventArgs> LoadStarted;
        internal protected virtual void OnLoadStart(LoadStartEventArgs eventArgs)
        {
            LoadStarted?.Invoke(this, eventArgs);
        }

        public event EventHandler<PluginCrashedEventArgs> PluginCrashed;
        internal protected virtual void OnPluginCrashed(PluginCrashedEventArgs eventArgs)
        {
            PluginCrashed?.Invoke(this, eventArgs);
        }

        public event EventHandler<RenderProcessTerminatedEventArgs> RenderProcessTerminated;
        internal protected virtual void OnRenderProcessTerminated(RenderProcessTerminatedEventArgs eventArgs)
        {
            RenderProcessTerminated?.Invoke(this, eventArgs);
        }

        #endregion Events Handling
    }
}
