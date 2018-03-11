#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using Chromely.CefGlue.Winapi.ChromeHost;
    using Xilium.CefGlue;

    public class CefGlueClient : CefClient
    {
        private readonly CefGlueBrowser m_core;

        private readonly CefLifeSpanHandler m_lifeSpanHandler;
        private readonly CefLoadHandler m_loadHandler;
        private readonly CefRequestHandler m_requestHandler;
        private readonly CefDisplayHandler m_displayHandler;
        private readonly CefContextMenuHandler m_contextMenuHandler;
        private readonly CefFocusHandler m_focusHandler;
        private readonly CefKeyboardHandler m_keyboardHandler;
        private readonly CefJSDialogHandler m_jsDialogHandler;
        private readonly CefDialogHandler m_dialogHandler;
        private readonly CefDragHandler m_dragHandler;
        private readonly CefGeolocationHandler m_geolocationHandler;
        private readonly CefDownloadHandler m_downloadHandler;
        private readonly CefFindHandler m_findHandler;

        public CefGlueClient(CefGlueClientParams clientParams)
        {
            m_core = clientParams.Browser;
            m_lifeSpanHandler = clientParams.LifeSpanHandler;
            m_loadHandler = clientParams.LoadHandler;
            m_requestHandler = clientParams.RequestHandler;
            m_displayHandler = clientParams.DisplayHandler;
            m_contextMenuHandler = clientParams.ContextMenuHandler;
            m_focusHandler = clientParams.FocusHandler;
            m_keyboardHandler = clientParams.KeyboardHandler;
            m_jsDialogHandler = clientParams.JSDialogHandler;
            m_dialogHandler = clientParams.DialogHandler;
            m_dragHandler = clientParams.DragHandler;
            m_geolocationHandler = clientParams.GeolocationHandler;
            m_downloadHandler = clientParams.DownloadHandler;
            m_findHandler = clientParams.FindHandler;
        }

        protected CefGlueBrowser Core
        {
            get
            {
                return m_core;
            }
        }

        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return m_lifeSpanHandler;
        }

        protected override CefLoadHandler GetLoadHandler()
        {
            return m_loadHandler;
        }

        protected override CefRequestHandler GetRequestHandler()
        {
            return m_requestHandler;
        }

        protected override CefDisplayHandler GetDisplayHandler()
        {
            return m_displayHandler;
        }

        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return m_contextMenuHandler;
        }

        protected override CefFocusHandler GetFocusHandler()
        {
            return m_focusHandler;
        }

        protected override CefKeyboardHandler GetKeyboardHandler()
        {
            return m_keyboardHandler;
        }

        protected override CefJSDialogHandler GetJSDialogHandler()
        {
            return m_jsDialogHandler;
        }

        protected override CefDialogHandler GetDialogHandler()
        {
            return m_dialogHandler;
        }

        protected override CefDragHandler GetDragHandler()
        {
            return m_dragHandler;
        }

        protected override CefGeolocationHandler GetGeolocationHandler()
        {
            return m_geolocationHandler;
        }

        protected override CefDownloadHandler GetDownloadHandler()
        {
            return m_downloadHandler;
        }

        protected override CefFindHandler GetFindHandler()
        {
            return m_findHandler;
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            return CefGlueBrowserHost.BrowserMessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        }
    }
}
