// Copyright (c) Alex Maitland. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Chromely.Owin;

public class OwinSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected readonly IOwinPipeline _owinPipeline;
    protected readonly IChromelyErrorHandler _errorHandler;

    public OwinSchemeHandlerFactory(IOwinPipeline owinPipeline, IChromelyErrorHandler errorHandler)
    {
        _owinPipeline = owinPipeline;
        _errorHandler = errorHandler;
    }

    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new OwinSchemeHandler(_owinPipeline, _errorHandler);
    }
}