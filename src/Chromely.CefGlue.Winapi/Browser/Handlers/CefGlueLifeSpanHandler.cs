#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System;
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    public  class CefGlueLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly CefGlueBrowser m_browser;

        public CefGlueLifeSpanHandler()
        {
            m_browser = CefGlueBrowser.BrowserCore;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            m_browser.InvokeAsyncIfPossible(() => m_browser.OnBrowserAfterCreated(browser));
        }

        protected override bool DoClose(CefBrowser browser)
        {
            // TODO: ... dispose core
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            m_browser.InvokeAsyncIfPossible((Action)m_browser.OnBeforeClose);
        }

        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
            var eventArgs = new BeforePopupEventArgs(frame, targetUrl, targetFrameName, popupFeatures, windowInfo, client, settings,
                                 noJavascriptAccess);

            m_browser.InvokeAsyncIfPossible(() => m_browser.OnBeforePopup(eventArgs));

            client = eventArgs.Client;
            noJavascriptAccess = eventArgs.NoJavascriptAccess;

            return eventArgs.Handled;
        }
    }
}
