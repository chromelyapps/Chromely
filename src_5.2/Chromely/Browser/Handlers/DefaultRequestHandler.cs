// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// Default CEF request handler.
    /// </summary>
    public class DefaultRequestHandler : CefRequestHandler
    {
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyRequestHandler _requestHandler;
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly CefResourceRequestHandler _resourceRequestHandler;

        /// <summary>
        /// The m_browser.
        /// </summary>
        protected ChromiumBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRequestHandler"/> class.
        /// </summary>
        public DefaultRequestHandler(IChromelyConfiguration config, 
                                     IChromelyRequestHandler requestHandler,
                                     IChromelyRouteProvider routeProvider,
                                     IChromelyWindow window, 
                                     CefResourceRequestHandler resourceRequestHandler = null)
        {
            _config = config;
            _requestHandler = requestHandler;
            _routeProvider = routeProvider;
            _browser = window as ChromiumBrowser;
            _resourceRequestHandler = resourceRequestHandler;
        }

        public ChromiumBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        protected override CefResourceRequestHandler GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return _resourceRequestHandler;
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
            var isUrlExternal = _config?.UrlSchemes?.IsUrlRegisteredExternalBrowserScheme(request.Url);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                BrowserLauncher.Open(_config.Platform, request.Url);
                return true;
            }

            // Sample: http://chromely.com/democontroller/showdevtools 
            // Expected to execute controller route action without return value
            var route = _routeProvider.GetRoute(request.Url);
            if (route != null && !route.HasReturnValue)
            {
                _requestHandler.Execute(request.Url);
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
