// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpLifeSpanHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using global::CefSharp;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    /// <summary>
    /// The cef sharp life span handler.
    /// </summary>
    public class CefSharpLifeSpanHandler : ILifeSpanHandler
    {
        /// <summary>
        /// The do close.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        /// <summary>
        /// The on after created.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
        }

        /// <summary>
        /// The on before close.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        /// <summary>
        /// The on before popup.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
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
        /// <param name="browserSettings">
        /// The browser settings.
        /// </param>
        /// <param name="noJavascriptAccess">
        /// The no javascript access.
        /// </param>
        /// <param name="newBrowser">
        /// The new browser.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            var isUrlExternal = UrlSchemeProvider.IsUrlRegisteredExternal(targetUrl);
            if (isUrlExternal)
            {
                System.Diagnostics.Process.Start(targetUrl);
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
