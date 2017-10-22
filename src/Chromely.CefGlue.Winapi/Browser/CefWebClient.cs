#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using Chromely.CefGlue.Winapi.Browser.Handlers;
    using Chromely.CefGlue.Winapi.ChromeHost;
    using Xilium.CefGlue;

    internal class CefWebClient : CefClient
    {
        private readonly CefWebBrowser m_core;
        private readonly CefWebLifeSpanHandler m_lifeSpanHandler;
        private readonly CefWebDisplayHandler m_displayHandler;
        private readonly CefWebLoadHandler m_loadHandler;
        private readonly CefWebRequestHandler m_requestHandler;
        private readonly CefContextMenuHandler m_menuHandler;

        public CefWebClient(CefWebBrowser core)
        {
            m_core = core;
            m_lifeSpanHandler = new CefWebLifeSpanHandler(m_core);
            m_displayHandler = new CefWebDisplayHandler(m_core);
            m_loadHandler = new CefWebLoadHandler(m_core);
            m_requestHandler = new CefWebRequestHandler(m_core);
            m_menuHandler = new CefGlueContextMenuHandler();
        }

        protected CefWebBrowser Core { get { return m_core; } }

        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return m_lifeSpanHandler;
        }

        protected override CefDisplayHandler GetDisplayHandler()
        {
            return m_displayHandler;
        }

        protected override CefLoadHandler GetLoadHandler()
        {
            return m_loadHandler;
        }

        protected override CefRequestHandler GetRequestHandler()
        {
            return m_requestHandler;
        }

        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return m_menuHandler;
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            return CefGlueBrowserHost.BrowserMessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        }
    }
}
