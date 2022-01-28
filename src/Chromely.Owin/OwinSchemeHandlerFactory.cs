// Copyright (c) Alex Maitland. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Chromely.Owin;

/// <summary>
/// Default OWIN scheme handler factory.
/// </summary>
public class OwinSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected readonly IOwinPipeline _owinPipeline;
    protected readonly IChromelyErrorHandler _errorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="OwinSchemeHandlerFactory"/>.
    /// </summary>
    /// <param name="owinPipeline">Instance of <see cref="IOwinPipeline"/>.</param>
    /// <param name="errorHandler">Instance of <see cref="IChromelyErrorHandler"/>.</param>
    public OwinSchemeHandlerFactory(IOwinPipeline owinPipeline, IChromelyErrorHandler errorHandler)
    {
        _owinPipeline = owinPipeline;
        _errorHandler = errorHandler;
    }

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new OwinSchemeHandler(_owinPipeline, _errorHandler);
    }
}