// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// The CefGlue load handler.
    /// </summary>
    public class DefaultLoadHandler : CefLoadHandler
    {
        protected readonly IChromelyConfiguration _config;

        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        protected ChromiumBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLoadHandler"/> class.
        /// </summary>
        public DefaultLoadHandler(IChromelyConfiguration config, IChromelyWindow window)
        {
            _config = config;
            _browser = window as ChromiumBrowser;
        }

        public ChromiumBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
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
            _browser.InvokeAsyncIfPossible(() => _browser.OnFrameLoadEnd(new FrameLoadEndEventArgs(frame, httpStatusCode)));
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
            _browser.InvokeAsyncIfPossible(() => _browser.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
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
            _browser.InvokeAsyncIfPossible(() => _browser.OnFrameLoadStart(new FrameLoadStartEventArgs(frame)));
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
            _browser.InvokeAsyncIfPossible(() => _browser.OnLoadingStateChange(new LoadingStateChangedEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}
