// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// Default CEF http scheme handler factory.
    /// </summary>
    public class DefaultRequestSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
        protected readonly IChromelyRequestHandler _requestHandler;
        protected readonly IChromelyDataTransferOptions _dataTransferOptions;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        public DefaultRequestSchemeHandlerFactory(IChromelyRouteProvider routeProvider,
                                                  IChromelyRequestSchemeProvider requestSchemeProvider,
                                                  IChromelyRequestHandler requestHandler,
                                                  IChromelyDataTransferOptions dataTransferOptions,
                                                  IChromelyErrorHandler chromelyErrorHandler)
        {
            _routeProvider = routeProvider;
            _requestSchemeProvider = requestSchemeProvider;
            _requestHandler = requestHandler;
            _dataTransferOptions = dataTransferOptions;
            _chromelyErrorHandler = chromelyErrorHandler;
        }

        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new DefaultRequestSchemeHandler(_routeProvider, _requestSchemeProvider, _requestHandler, _dataTransferOptions, _chromelyErrorHandler);
        }
    }
}
