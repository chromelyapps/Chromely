// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueLifeSpanHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;

    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue life span handler.
    /// </summary>
    public class CefGlueLifeSpanHandler : CefLifeSpanHandler
    {
        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser mBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueLifeSpanHandler"/> class.
        /// </summary>
        public CefGlueLifeSpanHandler()
        {
            mBrowser = CefGlueBrowser.BrowserCore;
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

            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnBrowserAfterCreated(browser));
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
            mBrowser.InvokeAsyncIfPossible(mBrowser.OnBeforeClose);
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

            return true;
        }
    }
}
