// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalWebBrowserExtensions.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using global::CefSharp.Internals;

namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    /// <summary>
    /// The internal web browser extensions.
    /// </summary>
    internal static class InternalWebBrowserExtensions
    {
        /// <summary>
        /// The set handlers to null.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        internal static void SetHandlersToNull(this IWebBrowserInternal browser)
        {
            browser.DialogHandler = null;
            browser.RequestHandler = null;
            browser.DisplayHandler = null;
            browser.LoadHandler = null;
            browser.LifeSpanHandler = null;
            browser.KeyboardHandler = null;
            browser.JsDialogHandler = null;
            browser.DragHandler = null;
            browser.DownloadHandler = null;
            browser.MenuHandler = null;
            browser.FocusHandler = null;
            browser.ResourceHandlerFactory = null;
            browser.RenderProcessMessageHandler = null;
        }
    }
}