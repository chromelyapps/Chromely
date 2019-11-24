// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueHttpSchemeHandlerFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue http scheme handler factory.
    /// </summary>
    public class CefGlueHttpSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyRequestTaskRunner _requestTaskRunner;

        public CefGlueHttpSchemeHandlerFactory(IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner)
        {
            _config = config;
            _requestTaskRunner = requestTaskRunner;
        }

        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new CefGlueHttpSchemeHandler(_config, _requestTaskRunner);
        }
    }
}
