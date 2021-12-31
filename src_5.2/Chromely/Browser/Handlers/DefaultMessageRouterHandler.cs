// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
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
        protected readonly IChromelyRequestHandler _requestHandler;
        protected readonly IChromelyDataTransferOptions _dataTransferOptions;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        public DefaultMessageRouterHandler(IChromelyRouteProvider routeProvider, IChromelyRequestHandler requestHandler, IChromelyDataTransferOptions dataTransferOptions, IChromelyErrorHandler chromelyErrorHandler)
        {
            _routeProvider = routeProvider;
            _requestHandler = requestHandler;
            _dataTransferOptions = dataTransferOptions;
            _chromelyErrorHandler = chromelyErrorHandler;
        }

        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            request requestData = null;

            try
            {
                requestData = JsonSerializer.Deserialize<request>(request, _dataTransferOptions.SerializerOptions as JsonSerializerOptions);

                if (requestData != null)
                {
                    var id = requestData.id ?? string.Empty;
                    var path = requestData.url ?? string.Empty;

                    bool isRequestAsync = _routeProvider.IsRouteAsync(path);

                    if (isRequestAsync)
                    {
                        Task.Run(async () =>
                        {
                            var parameters = requestData.parameters;
                            var postData = requestData.postData;

                            var response = await _requestHandler.ExecuteAsync(id, path, parameters, postData, request);
                            var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                            callback.Success(jsonResponse);
                        });
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            var parameters = requestData.parameters;
                            var postData = requestData.postData;

                            var response = _requestHandler.Execute(id, path, parameters, postData, request);
                            var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                            callback.Success(jsonResponse);
                        });
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                var response = _chromelyErrorHandler.HandleError(requestData?.ToRequest(), exception);
                var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);
                callback.Failure(100, jsonResponse);
                return false;
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
            public IDictionary<string, object> parameters { get; set; }
            public object postData { get; set; }

            public IChromelyRequest ToRequest()
            {
                return new ChromelyRequest(id, url, parameters, postData, null);
            }
        }
    }
}
