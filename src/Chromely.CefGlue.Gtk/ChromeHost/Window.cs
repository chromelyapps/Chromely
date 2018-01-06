namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using Chromely.CefGlue.Gtk.Browser;
    using Chromely.Core;
    using System;
    using Xilium.CefGlue;

    public sealed class Window : NativeWindow
    {
        private CefWebBrowser m_core;
        private IntPtr m_browserWindowHandle;
        private ChromelyConfiguration m_hostConfig;
        private readonly HostBase m_application;

        public Window(HostBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig.CefTitle, hostConfig.CefHostWidth, hostConfig.CefHostHeight, hostConfig.CefIconFile)
        {
            m_hostConfig = hostConfig;
            m_core = new CefWebBrowser(this, new CefBrowserSettings(), hostConfig.CefStartUrl);
            m_core.Created += new EventHandler(BrowserCreated);
            m_application = application;

            ShowWindow();
        }

        public void Close()
        {
            Dispose();
        }

        public CefWebBrowser WebBrowser { get { return m_core; } }

        public CefBrowser CurrentBrowser => throw new NotImplementedException();

        protected override void OnRealized(object sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    var parentHandle = HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, m_hostConfig.CefHostWidth, m_hostConfig.CefHostHeight)); 
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(HostXid, new CefRectangle(0, 0, m_hostConfig.CefHostWidth, m_hostConfig.CefHostHeight));
                    break;

                case CefRuntimePlatform.MacOSX:
                default:
                    throw new NotSupportedException();
            }

            m_core.Create(windowInfo);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            m_application.Quit();
        }

        private void BrowserCreated(object sender, EventArgs e)
        {
            m_browserWindowHandle = m_core.CefBrowser.GetHost().GetWindowHandle();
        }

        public void Dispose()
        {
            if (m_core != null)
            {
                var browser = m_core.CefBrowser;
                var host = browser.GetHost();
                host.CloseBrowser();
                host.Dispose();
                browser.Dispose();
                browser = null;
                m_browserWindowHandle = IntPtr.Zero;
            }
        }
    }
}
