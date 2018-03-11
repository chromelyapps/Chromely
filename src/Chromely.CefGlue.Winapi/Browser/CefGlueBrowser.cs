#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using System;
    using Xilium.CefGlue;

    public class CefGlueBrowser : WebBrowserBase
    {
        private CefBrowserConfig m_browserConfig;
        private CefBrowser m_browser;
        private IntPtr m_browserWindowHandle;

        public CefGlueBrowser(CefBrowserConfig browserConfig)
        {
            m_browserConfig = browserConfig;
            StartUrl = string.IsNullOrEmpty(browserConfig.StartUrl) ? "about:blank" : browserConfig.StartUrl;

            CreateBrowser();
        }

        public string StartUrl { get; set; }
        public string Title { get; private set; }
        public string Address { get; private set; }
        public CefBrowserSettings BrowserSettings { get; set; }
        public CefBrowser Browser { get { return m_browser; } }

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

        protected virtual CefGlueClient CreateWebClient()
        {
            IoC.RegisterInstance(typeof(CefGlueBrowser), typeof(CefGlueBrowser).FullName, this);
            CefGlueClient client = new CefGlueClient(CefGlueClientParams.Create(this));
            return client;
        }

        private void CreateBrowser()
        {
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(m_browserConfig.ParentHandle, m_browserConfig.CefRectangle);

            var client = CreateWebClient();

            var settings = BrowserSettings;
            if (settings == null) settings = new CefBrowserSettings { };

            settings.DefaultEncoding = "UTF-8";
            settings.FileAccessFromFileUrls = CefState.Enabled;
            settings.UniversalAccessFromFileUrls = CefState.Enabled;
            settings.WebSecurity = CefState.Enabled; 

            CefBrowserHost.CreateBrowser(windowInfo, client, settings, StartUrl);
        }

        public void ResizeWindow(int width, int height)
        {
            ResizeWindow(m_browserWindowHandle, width, height);
        }

        public void ResizeWindow(IntPtr handle, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.SetWindowPos(handle, IntPtr.Zero,
                    0, 0, width, height,
                     WinapiConstants.NoZOrder
                    );
            }
        }


        #region Events Handling

        public event EventHandler BrowserCreated;
        internal protected virtual void OnBrowserAfterCreated(CefBrowser browser)
        {
            m_browser = browser;
            m_browserWindowHandle = m_browser.GetHost().GetWindowHandle();

            BrowserCreated?.Invoke(this, EventArgs.Empty);
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
            m_browserWindowHandle = IntPtr.Zero;
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

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (m_browser != null && disposing)
            {
                var host = m_browser.GetHost();
                if (host != null)
                {
                    host.CloseBrowser();
                    host.Dispose();
                }
                m_browser.Dispose();
                m_browser = null;
                m_browserWindowHandle = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        #endregion Dispose
    }
}
