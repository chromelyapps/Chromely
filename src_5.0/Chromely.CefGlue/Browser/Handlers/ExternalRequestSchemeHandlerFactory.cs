// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalRequestSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Net.Http;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// ExternalRequestSchemeHandler handler factory object. Uses Shared HttpClient for all handlers
    /// </summary>
    public class ExternalRequestSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        static readonly HttpClient _sharedClient = new HttpClient(
            new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, true);


        /// <summary>
        /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.ExternalRequestSchemeHandlerFactory class.
        /// </summary>
        public ExternalRequestSchemeHandlerFactory()
        {

        }

        /// <inheritdoc/>
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new ExternalRequestSchemeHandler(_sharedClient);
        }
    }
}
