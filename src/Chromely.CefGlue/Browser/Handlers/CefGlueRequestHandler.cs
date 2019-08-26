// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueRequestHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue request handler.
    /// </summary>
    public class CefGlueRequestHandler : CefRequestHandler
    {
        /// <summary>
        /// The m_browser.
        /// </summary>
        private readonly CefGlueBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueRequestHandler"/> class.
        /// </summary>
        public CefGlueRequestHandler()
        {
            _browser = CefGlueBrowser.BrowserCore;
        }

        /// <summary>
        /// The on before browse.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="userGesture">
        /// The user gesture.
        /// </param>
        /// <param name="isRedirect">
        /// The is redirect.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
        {
            var isUrlExternal = UrlSchemeProvider.IsUrlRegisteredExternal(request.Url);
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
                            var url = request.Url.Replace("&", "^&");
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

            var isUrlCommand = UrlSchemeProvider.IsUrlRegisteredCommand(request.Url);
            if (isUrlCommand)
            {
                CommandTaskRunner.RunAsync(request.Url);
                return true;
            }

            return false;
        }

        /// <summary>
        /// The on plugin crashed.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="pluginPath">
        /// The plugin path.
        /// </param>
        protected override void OnPluginCrashed(CefBrowser browser, string pluginPath)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnPluginCrashed(new PluginCrashedEventArgs(pluginPath)));
        }

        /// <summary>
        /// The on render process terminated.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnRenderProcessTerminated(new RenderProcessTerminatedEventArgs(status)));
        }
    }
}
