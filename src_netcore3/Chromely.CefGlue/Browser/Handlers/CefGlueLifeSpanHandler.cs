// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueLifeSpanHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue life span handler.
    /// </summary>
    public class CefGlueLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyCommandTaskRunner _commandTaskRunner;
        private readonly CefGlueBrowser _browser;

        public CefGlueLifeSpanHandler(IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefGlueBrowser browser)
        {
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browser = browser;
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
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforeClose(new BeforeCloseEventArgs()));
        }

        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
        {
            var isUrlExternal = _config?.UrlSchemes?.IsUrlRegisteredExternalScheme(targetUrl);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                RegisteredExternalUrl.Launch(targetUrl);
            }

            var isUrlCommand = _config?.UrlSchemes?.IsUrlRegisteredCommandScheme(targetUrl);
            if (isUrlCommand.HasValue && isUrlCommand.Value)
            {
                _commandTaskRunner.RunAsync(targetUrl);
            }

            return true;
        }
    }
}
