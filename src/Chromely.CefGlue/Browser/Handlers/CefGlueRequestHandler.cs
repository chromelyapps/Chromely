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
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue request handler.
    /// </summary>
    public class CefGlueRequestHandler : CefRequestHandler
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyCommandTaskRunner _commandTaskRunner;

        /// <summary>
        /// The m_browser.
        /// </summary>
        private readonly CefGlueBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueRequestHandler"/> class.
        /// </summary>
        public CefGlueRequestHandler(IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefGlueBrowser browser)
        {
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browser = browser;
        }

        protected override CefResourceRequestHandler GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
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
            var isUrlExternal = _config?.UrlSchemes?.IsUrlRegisteredExternalScheme(request.Url);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                BrowserLauncher.Open(_config.Platform, request.Url);
                return true;
            }

            var isUrlCommand = _config?.UrlSchemes?.IsUrlRegisteredCommandScheme(request.Url);
            if (isUrlCommand.HasValue && isUrlCommand.Value)
            {
                _commandTaskRunner.RunAsync(request.Url);
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
