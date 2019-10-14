// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueLifeSpanHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue life span handler.
    /// </summary>
    public class CefGlueLifeSpanHandler : CefLifeSpanHandler
    {
        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueLifeSpanHandler"/> class.
        /// </summary>
        public CefGlueLifeSpanHandler()
        {
            _browser = CefGlueBrowser.BrowserCore;
        }

        /// <summary>
        /// The on after created.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);
            _browser.InvokeAsyncIfPossible(() => _browser.OnBrowserAfterCreated(browser));
        }

        /// <summary>
        /// The do close.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool DoClose(CefBrowser browser)
        {
            return false;
        }

        /// <summary>
        /// The on before close.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        protected override void OnBeforeClose(CefBrowser browser)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnBeforeClose(new BeforeCloseEventArgs()));
        }

        /// <summary>
        /// The on before popup.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="targetUrl">
        /// The target url.
        /// </param>
        /// <param name="targetFrameName">
        /// The target frame name.
        /// </param>
        /// <param name="targetDisposition">
        /// The target disposition.
        /// </param>
        /// <param name="userGesture">
        /// The user gesture.
        /// </param>
        /// <param name="popupFeatures">
        /// The popup features.
        /// </param>
        /// <param name="windowInfo">
        /// The window info.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="noJavascriptAccess">
        /// The no javascript access.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
            var isUrlExternal = UrlSchemeProvider.IsUrlRegisteredExternal(targetUrl);
            if (isUrlExternal)
            {
                RegisteredExternalUrl.Launch(targetUrl);
            }

            var isUrlCommand = UrlSchemeProvider.IsUrlRegisteredCommand(targetUrl);
            if (isUrlCommand)
            {
                CommandTaskRunner.RunAsync(targetUrl);
            }

            return true;
        }
    }
}
