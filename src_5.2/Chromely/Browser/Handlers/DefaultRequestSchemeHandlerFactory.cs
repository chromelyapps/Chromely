// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

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

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultRequestSchemeHandlerFactory"/>.
    /// </summary>
    /// <param name="routeProvider">Instance of <see cref="IChromelyRouteProvider"/>.</param>
    /// <param name="requestSchemeProvider">Instance of <see cref="IChromelyRequestSchemeProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromelyRequestHandler"/>.</param>
    /// <param name="dataTransferOptions">Instance of <see cref="IChromelyDataTransferOptions"/>.</param>
    /// <param name="chromelyErrorHandler">Instance of <see cref="IChromelyErrorHandler"/>.</param>
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

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new DefaultRequestSchemeHandler(_routeProvider, _requestSchemeProvider, _requestHandler, _dataTransferOptions, _chromelyErrorHandler);
    }
}