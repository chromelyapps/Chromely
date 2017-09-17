#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Core.CefGlueBrowser
{
    using Xilium.CefGlue;

    internal sealed class CefWebLoadHandler : CefLoadHandler
	{
		private readonly CefWebBrowser m_core;

		public CefWebLoadHandler(CefWebBrowser core)
		{
			m_core = core;
		}

		protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
		{
			m_core.InvokeAsyncIfPossible(() => m_core.OnLoadEnd(new LoadEndEventArgs(frame, httpStatusCode)));
		}

		protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
		{
			m_core.InvokeAsyncIfPossible(() => m_core.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
		}

		protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
		{
			m_core.InvokeAsyncIfPossible(() => m_core.OnLoadStart(new LoadStartEventArgs(frame)));
		}

        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            m_core.InvokeAsyncIfPossible(() => m_core.OnLoadingStateChange(new LoadingStateChangeEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}
