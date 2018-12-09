// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueLoadHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using Chromely.CefGlue.Winapi.Browser.EventParams;
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue load handler.
    /// </summary>
    public class CefGlueLoadHandler : CefLoadHandler
    {
        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser mBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueLoadHandler"/> class.
        /// </summary>
        public CefGlueLoadHandler()
        {
            mBrowser = CefGlueBrowser.BrowserCore;
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnFrameLoadEnd(new FrameLoadEndEventArgs(frame, httpStatusCode)));
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnFrameLoadStart(new FrameLoadStartEventArgs(frame)));
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnLoadingStateChange(new LoadingStateChangeEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}
