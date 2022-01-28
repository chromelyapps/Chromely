// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core.Defaults
{
    public class DefaultRequestTaskRunner : IChromelyRequestTaskRunner
    {
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly IChromelyInfo _chromelyInfo;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        public DefaultRequestTaskRunner(IChromelyRouteProvider routeProvider, IChromelyInfo chromelyInfo, IChromelyErrorHandler chromelyErrorHandler)
        {
            _routeProvider = routeProvider;
            _chromelyInfo = chromelyInfo;
            _chromelyErrorHandler = chromelyErrorHandler;
        }

        public IChromelyResponse Run(string routeUrl, IDictionary<string, string> parameters, object postData)
        {
            if (string.IsNullOrWhiteSpace(routeUrl))
            {

                return _chromelyErrorHandler.HandleRouteNotFound(string.Empty, routeUrl);
            }

            if (routeUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(string.Empty);
            }

            var route = _routeProvider.GetActionRoute(routeUrl);

            if (route == null)
            {
                throw new Exception($"Route for path = {routeUrl} is null or invalid.");
            }

            return ExecuteRoute(string.Empty, routeUrl, parameters, postData, string.Empty);
        }

        public IChromelyResponse Run(IChromelyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RouteUrl))
            {
                return _chromelyErrorHandler.HandleRouteNotFound(request.Id, request.RouteUrl);
            }

            if (request.RouteUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(request.Id);
            }

            var route = _routeProvider.GetActionRoute(request.RouteUrl);
            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(request.Id, request.RouteUrl);
            }

            var temp = request.Parameters ?? request.RouteUrl.GetParameters();
            var parameters = temp?.ToDictionary();
            var postData = request.PostData;

            return ExecuteRoute(request.Id, request.RouteUrl, parameters, postData, request.RawJson);
        }

        public IChromelyResponse Run(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData)
        {
            if (string.IsNullOrWhiteSpace(routeUrl))
            {
                return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
            }

            if (routeUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(requestId);
            }

            var route = _routeProvider.GetActionRoute(routeUrl);
            if (route == null)
            {
                throw new Exception($"Route for path = {routeUrl} is null or invalid.");
            }

            return ExecuteRoute(requestId, routeUrl, parameters, postData, requestData);
        }

        public async Task<IChromelyResponse> RunAsync(string routeUrl, IDictionary<string, string> parameters, object postData)
        {
            if (string.IsNullOrWhiteSpace(routeUrl))
            {
                return _chromelyErrorHandler.HandleRouteNotFound(string.Empty, routeUrl);
            }

            if (routeUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(string.Empty);
            }

            var route = _routeProvider.GetActionRoute(routeUrl);

            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(string.Empty, routeUrl);
            }

            return await ExecuteRouteAsync(string.Empty, routeUrl, parameters, postData, string.Empty);
        }

        public async Task<IChromelyResponse> RunAsync(IChromelyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RouteUrl))
            {
                return _chromelyErrorHandler.HandleRouteNotFound(request.Id, request.RouteUrl);
            }

            if (request.RouteUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(request.Id);
            }

            var route = _routeProvider.GetActionRoute(request.RouteUrl);
            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(request.Id, request.RouteUrl);
            }

            var temp = request.Parameters ?? request.RouteUrl.GetParameters();
            var parameters = temp?.ToDictionary();
            var postData = request.PostData;

            return await ExecuteRouteAsync(request.Id, request.RouteUrl, parameters, postData, request.RawJson);
        }

        public async Task<IChromelyResponse> RunAsync(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData)
        {
            if (string.IsNullOrWhiteSpace(routeUrl))
            {
                return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
            }

            if (routeUrl.ToLower().Equals("/info"))
            {
                return _chromelyInfo?.GetInfo(requestId);
            }

            var route = _routeProvider.GetActionRoute(routeUrl);
            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
            }

            return await ExecuteRouteAsync(requestId, routeUrl, parameters, postData, requestData);
        }

        private IChromelyResponse ExecuteRoute(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData)
        {
            var route = _routeProvider.GetActionRoute(routeUrl);

            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
            }

            var response = route.Invoke(requestId: requestId, routeUrl: routeUrl, parameters: parameters, postData: postData, rawJson: requestData);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
            response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

            return response;
        }

        private async Task<IChromelyResponse> ExecuteRouteAsync(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData)
        {
            var route = _routeProvider.GetActionRoute(routeUrl);

            if (route == null)
            {
                return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
            }

            IChromelyResponse response;
            if (route.IsAsync)
            {
                response = await route.InvokeAsync(requestId: requestId, routeUrl: routeUrl, parameters: parameters, postData: postData, rawJson: requestData);
            }
            else
            {
                response = route.Invoke(requestId: requestId, routeUrl: routeUrl, parameters: parameters, postData: postData, rawJson: requestData);
            }

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
            response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

            return response;
        }
    }
}