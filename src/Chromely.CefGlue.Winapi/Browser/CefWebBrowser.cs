#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using Chromely.Core.Host;
    using System;
    using System.ComponentModel;
    using Xilium.CefGlue;

    internal class CefWebBrowser : WebBrowserBase
    {
        private CefBrowserConfig m_browserConfig;
        private CefBrowser m_browser;
        private IntPtr m_browserWindowHandle;

        public CefWebBrowser(CefBrowserConfig browserConfig)
        {
            m_browserConfig = browserConfig;
            StartUrl = string.IsNullOrEmpty(browserConfig.StartUrl) ? "about:blank" : browserConfig.StartUrl;

            CreateBrowser();
        }

        [DefaultValue("about:blank")]
        public string StartUrl { get; set; }

        [Browsable(false)]
        public CefBrowserSettings BrowserSettings { get; set; }

        protected virtual CefWebClient CreateWebClient()
        {
            return new CefWebClient(this);
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

        public event EventHandler BrowserCreated;

        internal protected virtual void OnBrowserAfterCreated(CefBrowser browser)
        {
            m_browser = browser;
            m_browserWindowHandle = m_browser.GetHost().GetWindowHandle();

            BrowserCreated?.Invoke(this, EventArgs.Empty);
        }

        internal protected virtual void OnTitleChanged(TitleChangedEventArgs e)
        {
            Title = e.Title;
            TitleChanged?.Invoke(this, e);
        }

        public string Title { get; private set; }

        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        internal protected virtual void OnAddressChanged(AddressChangedEventArgs e)
        {
            Address = e.Address;
            AddressChanged?.Invoke(this, e);
        }

        public string Address { get; private set; }

        public event EventHandler<AddressChangedEventArgs> AddressChanged;

        internal protected virtual void OnStatusMessage(StatusMessageEventArgs e)
        {
            StatusMessage?.Invoke(this, e);
        }

        public event EventHandler<StatusMessageEventArgs> StatusMessage;

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

        public CefBrowser Browser { get { return m_browser; } }

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        internal protected virtual void OnConsoleMessage(ConsoleMessageEventArgs e)
        {
            if (ConsoleMessage != null)
                ConsoleMessage(this, e);
            else
                e.Handled = false;
        }

        public event EventHandler<LoadingStateChangeEventArgs> LoadingStateChange;

        internal protected virtual void OnLoadingStateChange(LoadingStateChangeEventArgs e)
        {
            LoadingStateChange?.Invoke(this, e);
        }

        public event EventHandler<TooltipEventArgs> Tooltip;

        internal protected virtual void OnTooltip(TooltipEventArgs e)
        {
            if (Tooltip != null)
                Tooltip(this, e);
            else
                e.Handled = false;
        }

        public event EventHandler BeforeClose;

        internal protected virtual void OnBeforeClose()
        {
            m_browserWindowHandle = IntPtr.Zero;
            BeforeClose?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<BeforePopupEventArgs> BeforePopup;

        internal protected virtual void OnBeforePopup(BeforePopupEventArgs e)
        {
            if (BeforePopup != null)
                BeforePopup(this, e);
            else
                e.Handled = false;
        }

        public event EventHandler<LoadEndEventArgs> LoadEnd;

        internal protected virtual void OnLoadEnd(LoadEndEventArgs e)
        {
            LoadEnd?.Invoke(this, e);
        }

        public event EventHandler<LoadErrorEventArgs> LoadError;

        internal protected virtual void OnLoadError(LoadErrorEventArgs e)
        {
            LoadError?.Invoke(this, e);
        }

        public event EventHandler<LoadStartEventArgs> LoadStarted;

        internal protected virtual void OnLoadStart(LoadStartEventArgs e)
        {
            LoadStarted?.Invoke(this, e);
        }

        public event EventHandler<PluginCrashedEventArgs> PluginCrashed;

        internal protected virtual void OnPluginCrashed(PluginCrashedEventArgs e)
        {
            PluginCrashed?.Invoke(this, e);
        }

        public event EventHandler<RenderProcessTerminatedEventArgs> RenderProcessTerminated;

        internal protected virtual void OnRenderProcessTerminated(RenderProcessTerminatedEventArgs e)
        {
            RenderProcessTerminated?.Invoke(this, e);
        }
    }
}
