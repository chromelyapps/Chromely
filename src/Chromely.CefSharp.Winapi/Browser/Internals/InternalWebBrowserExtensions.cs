// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalWebBrowserExtensions.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    using global::CefSharp.Internals;

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