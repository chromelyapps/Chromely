#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser
{
    using System;
    using System.Diagnostics;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    internal sealed class CefWebRequestHandler : CefRequestHandler
    {
        private readonly CefWebBrowser m_core;

        public CefWebRequestHandler(CefWebBrowser core)
        {
            m_core = core;
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
          //  m_core.InvokeAsyncIfPossible(() => m_core.OnPluginCrashed(new PluginCrashedEventArgs(pluginPath)));
        }

        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
          //  m_core.InvokeAsyncIfPossible(() => m_core.OnRenderProcessTerminated(new RenderProcessTerminatedEventArgs(status)));
        }
    }
}
