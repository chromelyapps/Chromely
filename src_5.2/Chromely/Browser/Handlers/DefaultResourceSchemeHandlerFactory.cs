// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Configuration;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// The CefGlue resource scheme handler factory.
    /// </summary>
    public class DefaultResourceSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        public DefaultResourceSchemeHandlerFactory(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler)
        {
            _config = config;
            _chromelyErrorHandler = chromelyErrorHandler;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="CefResourceHandler"/>.
        /// </returns>
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new DefaultResourceSchemeHandler(_config, _chromelyErrorHandler);
        }
    }
}
