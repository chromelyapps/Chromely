// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueHttpSchemeHandlerFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue http scheme handler factory.
    /// </summary>
    public class CefGlueHttpSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected readonly IChromelyContainer _container;
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;

        public CefGlueHttpSchemeHandlerFactory(IChromelyContainer container, IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner)
        {
            _container = container;
            _config = config;
            _requestTaskRunner = requestTaskRunner;
        }

        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new CefGlueHttpSchemeHandler(_container, _config, _requestTaskRunner);
        }
    }
}
