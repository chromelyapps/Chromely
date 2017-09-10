#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Browser
{
    using Chromely.Browser.EventParams;
    using Chromely.Infrastructure;
    using Xilium.CefGlue;

    sealed class CefWebRequestHandler : CefRequestHandler
    {
        private readonly CefWebBrowser m_core;

        public CefWebRequestHandler(CefWebBrowser core)
        {
            m_core = core;
        }

        protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool isRedirect)
        {
            bool isUrlExternal = ExternalUrlSchemeFactory.IsUrlRegisteredExternal(request.Url);
            if (isUrlExternal)
            {
                System.Diagnostics.Process.Start(request.Url);
                return true;
            }

            return false;
        }

        protected override void OnPluginCrashed(CefBrowser browser, string pluginPath)
        {
            m_core.InvokeAsyncIfPossible(() => m_core.OnPluginCrashed(new PluginCrashedEventArgs(pluginPath)));
        }

        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
            m_core.InvokeAsyncIfPossible(() => m_core.OnRenderProcessTerminated(new RenderProcessTerminatedEventArgs(status)));
        }
    }
}
