// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Network;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.Browser
{
    /// <summary>
    /// Default CEF message router handler.
    /// </summary>
    public class DefaultMessageRouterHandler : CefMessageRouterBrowserSide.Handler, IChromelyMessageRouter
    {
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;
        protected readonly IChromelySerializerUtil _serializerUtil;

        public DefaultMessageRouterHandler(IChromelyRouteProvider routeProvider, IChromelyRequestTaskRunner requestTaskRunner, IChromelySerializerUtil serializerUtil)
        {
            _routeProvider = routeProvider;
            _requestTaskRunner = requestTaskRunner;
            _serializerUtil = serializerUtil;
        }

        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            var options = new JsonSerializerOptions();
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.AllowTrailingCommas = true;
            var requestData =  JsonSerializer.Deserialize<request>(request, options);

            if (requestData != null)
            {
                var id = requestData.id ?? string.Empty;
                var path = requestData.url ?? string.Empty;

                bool isRequestAsync = _routeProvider.IsActionRouteAsync(path);

                if (isRequestAsync)
                {
                    Task.Run(async () =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = await _requestTaskRunner.RunAsync(id, path, parameters, postData, request);
                        var jsonResponse = _serializerUtil.ObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = _requestTaskRunner.Run(id, path, parameters, postData, request);
                        var jsonResponse = _serializerUtil.ObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }

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
