// Copyright © 2017-2020 Chromely Projects. All rights reserved.
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
        protected readonly IChromelyCommandTaskRunner _commandTaskRunner;

        /// <summary>
        /// The m_browser.
        /// </summary>
        protected ChromiumBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRequestHandler"/> class.
        /// </summary>
        public DefaultRequestHandler(IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, IChromelyWindow window)
        {
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browser = window as ChromiumBrowser;
        }

        public ChromiumBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
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
            var isUrlExternal = _config?.UrlSchemes?.IsUrlRegisteredExternalBrowserScheme(request.Url);
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
