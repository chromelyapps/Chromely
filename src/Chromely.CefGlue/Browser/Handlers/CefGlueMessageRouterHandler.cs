// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueMessageRouterHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Chromely.Core.Network;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue message router handler.
    /// </summary>
    public class CefGlueMessageRouterHandler : CefMessageRouterBrowserSide.Handler
    {
        private readonly IChromelyRequestTaskRunner _requestTaskRunner;


        public CefGlueMessageRouterHandler(IChromelyRequestTaskRunner requestTaskRunner)
        {
            _requestTaskRunner = requestTaskRunner;
        }

        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            var options = new JsonSerializerOptions();
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.AllowTrailingCommas = true;
            var requestData = JsonSerializer.Deserialize<request>(request, options);

            var method = requestData.method ?? string.Empty;

            if (RoutePath.ValidMethod(method))
            {
                new Task(() =>
                {
                    var id = requestData.id ??  string.Empty;
                    var path = requestData.url ?? string.Empty;
                    var parameters = requestData.parameters;
                    var postData = requestData.postData;

                    var routePath = new RoutePath(method, path);
                    var response = _requestTaskRunner.Run(id, routePath, parameters, postData, request);
                    var jsonResponse = response.ToJson();

                    callback.Success(jsonResponse);
                }).Start();

                return true;
            }

            callback.Failure(100, "Request is not valid.");
            return false;
        }

        /// <summary>
        /// The on query canceled.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="queryId">
        /// The query id.
        /// </param>
        public override void OnQueryCanceled(CefBrowser browser, CefFrame frame, long queryId)
        {
        }

        private class request
        {
            public string id { get; set; }
            public string method { get; set; }
            public string url { get; set; }
            public IDictionary<string, string> parameters { get; set; }
            public object postData { get; set; }
        }
    }
}
