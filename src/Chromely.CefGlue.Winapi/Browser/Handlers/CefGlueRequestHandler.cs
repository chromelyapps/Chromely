#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System;
    using System.Diagnostics;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    public class CefGlueRequestHandler : CefRequestHandler
    {
        private readonly CefGlueBrowser m_browser;

        public CefGlueRequestHandler()
        {
            m_browser = CefGlueBrowser.BrowserCore;
        }

        protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool isRedirect)
        {
            bool isUrlExternal = UrlSchemeProvider.IsUrlRegisteredExternal(request.Url);
            if (isUrlExternal)
            {
                // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
                try
                {
                    Process.Start(request.Url);
                }
                catch
                {
                    try
                    {
                        // hack because of this: https://github.com/dotnet/corefx/issues/10361
                        if (CefRuntime.Platform == CefRuntimePlatform.Windows)
                        {
                            string url = request.Url.Replace("&", "^&");
                            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                        }
                        else if (CefRuntime.Platform == CefRuntimePlatform.Linux)
                        {
                            Process.Start("xdg-open", request.Url);
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                    }
                }

                return true;
            }

            return false;
        }

        protected override void OnPluginCrashed(CefBrowser browser, string pluginPath)
        {
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnPluginCrashed(new PluginCrashedEventArgs(pluginPath)));
        }

        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnRenderProcessTerminated(new RenderProcessTerminatedEventArgs(status)));
        }
    }
}
