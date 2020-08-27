// Copyright © 2017-2020 Chromely Projects. All rights reserved.
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
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;
        protected readonly IChromelySerializerUtil _serializerUtil;

        public DefaultRequestSchemeHandlerFactory(IChromelyRouteProvider routeProvider, IChromelyRequestSchemeProvider requestSchemeProvider, IChromelyRequestTaskRunner requestTaskRunner, IChromelySerializerUtil serializerUtil)
        {
            _routeProvider = routeProvider;
            _requestSchemeProvider = requestSchemeProvider;
            _requestTaskRunner = requestTaskRunner;
            _serializerUtil = serializerUtil;
        }

        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new DefaultRequestSchemeHandler(_routeProvider, _requestSchemeProvider, _requestTaskRunner, _serializerUtil);
        }
    }
}
