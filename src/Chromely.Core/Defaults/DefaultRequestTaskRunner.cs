using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core.Defaults
{
    public class DefaultRequestTaskRunner : IChromelyRequestTaskRunner
    {
        private readonly IChromelyContainer _container;
        private readonly IChromelyConfiguration _config;

        public DefaultRequestTaskRunner(IChromelyContainer container, IChromelyConfiguration config)
        {
            _container = container;
            _config = config;
        }

        public ChromelyResponse Run(string method, string path, IDictionary<string, string> parameters, object postData)
        {
            var routePath = new RoutePath(method, path);
            if (routePath == null || string.IsNullOrWhiteSpace(routePath?.Path))
            {
                return GetBadRequestResponse(null);
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(string.Empty);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath.Path} is null or invalid.");
            }

            return ExecuteRoute(string.Empty, routePath, parameters, postData, string.Empty);
        }

        public ChromelyResponse Run(ChromelyRequest request)
        {
            if (request.RoutePath == null)
            {
                return GetBadRequestResponse(request.Id);
            }

            if (string.IsNullOrEmpty(request.RoutePath.Path))
            {
                return GetBadRequestResponse(request.Id);
            }

            if (request.RoutePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(request.Id);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, request.RoutePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {request.RoutePath} is null or invalid.");
            }

            var parameters = request.Parameters ?? request.RoutePath.Path.GetParameters()?.ToObjectDictionary();
            var postData = request.PostData;

            return ExecuteRoute(request.Id, request.RoutePath, parameters, postData, request.RawJson);
        }

        public ChromelyResponse Run(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
        {
            if (string.IsNullOrEmpty(routePath.Path))
            {
                return GetBadRequestResponse(requestId);
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(requestId);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            return ExecuteRoute(requestId, routePath, parameters, postData, requestData);
        }

        public async Task<ChromelyResponse> RunAsync(string method, string path, IDictionary<string, string> parameters, object postData)
        {
            var routePath = new RoutePath(method, path);
            if (routePath == null || string.IsNullOrWhiteSpace(routePath?.Path))
            {
                return GetBadRequestResponse(null);
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(string.Empty);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath.Path} is null or invalid.");
            }

            return await ExecuteRouteAsync(string.Empty, routePath, parameters, postData, string.Empty);
        }

        public async Task<ChromelyResponse> RunAsync(ChromelyRequest request)
        {
            if (request.RoutePath == null)
            {
                return GetBadRequestResponse(request.Id);
            }

            if (string.IsNullOrEmpty(request.RoutePath.Path))
            {
                return GetBadRequestResponse(request.Id);
            }

            if (request.RoutePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(request.Id);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, request.RoutePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {request.RoutePath} is null or invalid.");
            }

            var parameters = request.Parameters ?? request.RoutePath.Path.GetParameters()?.ToObjectDictionary();
            var postData = request.PostData;

            return await ExecuteRouteAsync(request.Id, request.RoutePath, parameters, postData, request.RawJson);
        }

        public async Task<ChromelyResponse> RunAsync(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
        {
            if (string.IsNullOrEmpty(routePath.Path))
            {
                return GetBadRequestResponse(requestId);
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                return GetInfo(requestId);
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            return await ExecuteRouteAsync(requestId, routePath, parameters, postData, requestData);
        }

        private ChromelyResponse ExecuteRoute(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
        {
            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            var response = route.Invoke(requestId: requestId, routePath: routePath, parameters: parameters, postData: postData, rawJson: requestData);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";

            return response;
        }

        private async Task<ChromelyResponse> ExecuteRouteAsync(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
        {
            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            ChromelyResponse response;
            if (route.IsAsync)
            {
                response = await route.InvokeAsync(requestId: requestId, routePath: routePath, parameters: parameters, postData: postData, rawJson: requestData);
            }
            else
            {
                response = route.Invoke(requestId: requestId, routePath: routePath, parameters: parameters, postData: postData, rawJson: requestData);
            }

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";

            return response;
        }

        private ChromelyResponse GetInfo(string requestId)
        {
            var response = new ChromelyResponse(requestId);
            var infoItemDic = new Dictionary<string, string>
            {
                {
                    "divObjective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows, Linux and MacOS native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via CEF Message Router, Ajax HTTP/XHR requests using custom schemes and domains."
                },
                {
                    "divPlatform",
                    "Cross-platform - Windows, Linux, MacOS. Built on CefGlue, NET Standard 2.0, .NET Core 3.0, .NET Framework 4.61 and above."
                },
                { "divVersion", _config.ChromelyVersion }
            };

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;

            return response;
        }

        private ChromelyResponse GetBadRequestResponse(string requestId)
        {
            return new ChromelyResponse
            {
                RequestId = requestId,
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.BadRequest,
                StatusText = "Bad Request"
            };
        }
    }
}