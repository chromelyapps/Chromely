#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Core.CefGlueBrowser
{
    using System;
    using Xilium.CefGlue;

    internal sealed class CefWebLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly CefWebBrowser m_core;

        public CefWebLifeSpanHandler(CefWebBrowser core)
        {
            m_core = core;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            m_core.InvokeAsyncIfPossible(() => m_core.OnBrowserAfterCreated(browser));
        }

        protected override bool DoClose(CefBrowser browser)
        {
            // TODO: ... dispose core
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            m_core.InvokeAsyncIfPossible((Action)m_core.OnBeforeClose);
        }

        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
            var e = new BeforePopupEventArgs(frame, targetUrl, targetFrameName, popupFeatures, windowInfo, client, settings,
                                 noJavascriptAccess);

            m_core.InvokeAsyncIfPossible(() => m_core.OnBeforePopup(e));

            client = e.Client;
            noJavascriptAccess = e.NoJavascriptAccess;

            return e.Handled;
        }
    }
}
