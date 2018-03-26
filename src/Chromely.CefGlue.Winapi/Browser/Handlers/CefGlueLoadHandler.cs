// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueLoadHandler.cs" company="Chromely">
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
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using Chromely.CefGlue.Winapi.Browser.EventParams;
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    /// <summary>
    /// The cef glue load handler.
    /// </summary>
    public class CefGlueLoadHandler : CefLoadHandler
    {
        /// <summary>
        /// The m browser.
        /// </summary>
        private readonly CefGlueBrowser mBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueLoadHandler"/> class.
        /// </summary>
        public CefGlueLoadHandler()
        {
            this.mBrowser = CefGlueBrowser.BrowserCore;
        }

        /// <summary>
        /// The on load end.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="httpStatusCode">
        /// The http status code.
        /// </param>
        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnLoadEnd(new LoadEndEventArgs(frame, httpStatusCode)));
        }

        /// <summary>
        /// The on load error.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="errorText">
        /// The error text.
        /// </param>
        /// <param name="failedUrl">
        /// The failed url.
        /// </param>
        protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
        }

        /// <summary>
        /// The on load start.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="transitionType">
        /// The transition type.
        /// </param>
        protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
        {
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnLoadStart(new LoadStartEventArgs(frame)));
        }

        /// <summary>
        /// The on loading state change.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="isLoading">
        /// The is loading.
        /// </param>
        /// <param name="canGoBack">
        /// The can go back.
        /// </param>
        /// <param name="canGoForward">
        /// The can go forward.
        /// </param>
        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnLoadingStateChange(new LoadingStateChangeEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}
