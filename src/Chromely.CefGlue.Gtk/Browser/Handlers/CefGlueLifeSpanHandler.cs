#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

 namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Xilium.CefGlue;

    internal sealed class CefGlueLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly CefGlueBrowser m_browser;

        public CefGlueLifeSpanHandler()
        {
            m_browser = CefGlueBrowser.BrowserCore;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            m_browser.OnCreated(browser);
        }

        protected override bool DoClose(CefBrowser browser)
        {
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
        }
    }
}