#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    public class CefGlueLoadHandler : CefLoadHandler
	{
		private readonly CefGlueBrowser m_browser;

		public CefGlueLoadHandler()
		{
            m_browser = CefGlueBrowser.BrowserCore;
        }

		protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
		{
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnLoadEnd(new LoadEndEventArgs(frame, httpStatusCode)));
		}

		protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
		{
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
		}

		protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
		{
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnLoadStart(new LoadStartEventArgs(frame)));
		}

        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnLoadingStateChange(new LoadingStateChangeEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}
