// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoXFrameOptionsHttpSchemeHandlerFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// Class that creates NoXFrameOptionsHttpSchemeHandler instances for handling scheme requests.
    /// The methods of this class will always be called on the IO thread.
    /// </summary>
    public class NoXFrameOptionsHttpSchemeHandlerFactory: CefSchemeHandlerFactory
    {
        //TODO: use httpFactory?
        static readonly HttpClient _sharedClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }, true);

        /// <inheritdoc/>
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new NoXFrameOptionsHttpSchemeHandler(_sharedClient);
        }
    }
}
