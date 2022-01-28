// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// The CefGlue resource scheme handler factory.
/// </summary>
public class DefaultResourceSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected readonly IChromelyConfiguration _config;
    protected readonly IChromelyErrorHandler _chromelyErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultResourceSchemeHandlerFactory"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="chromelyErrorHandler">Instance of <see cref="IChromelyErrorHandler"/>.</param>
    public DefaultResourceSchemeHandlerFactory(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler)
    {
        _config = config;
        _chromelyErrorHandler = chromelyErrorHandler;
    }

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new DefaultResourceSchemeHandler(_config, _chromelyErrorHandler);
    }
}