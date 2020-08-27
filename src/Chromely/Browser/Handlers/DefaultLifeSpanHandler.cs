// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// Default CEF lifespanhandler.
    /// </summary>
    public class DefaultLifeSpanHandler : CefLifeSpanHandler
    {
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyCommandTaskRunner _commandTaskRunner;
        protected ChromiumBrowser _browser;

        public DefaultLifeSpanHandler(IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, IChromelyWindow window)
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

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);
            _browser.InvokeAsyncIfPossible(() => _browser.OnBrowserAfterCreated(browser));
        }

        protected override bool DoClose(CefBrowser browser)
        {
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforeClose());
        }

        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforePopup(new BeforePopupEventArgs(frame, targetUrl, targetFrameName)));

            var isUrlExternal = _config?.UrlSchemes?.IsUrlRegisteredExternalBrowserScheme(targetUrl);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                BrowserLauncher.Open(_config.Platform, targetUrl);
                return true;
            }

            var isUrlCommand = _config?.UrlSchemes?.IsUrlRegisteredCommandScheme(targetUrl);
            if (isUrlCommand.HasValue && isUrlCommand.Value)
            {
                _commandTaskRunner.RunAsync(targetUrl);
                return true;
            }

            return false;
        }
    }
}
