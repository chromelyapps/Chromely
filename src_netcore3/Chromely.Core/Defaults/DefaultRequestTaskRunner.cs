using System;
using System.Collections.Generic;
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
            var response = new ChromelyResponse();
            var routePath = new RoutePath(method, path);
            if (routePath == null || string.IsNullOrWhiteSpace(routePath?.Path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath.Path} is null or invalid.");
            }

            return ExcuteRoute(string.Empty, routePath, parameters, postData, string.Empty);
        }

        public ChromelyResponse Run(ChromelyRequest request)
        {
            var response = new ChromelyResponse(request.Id);
            if (request.RoutePath == null)
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (string.IsNullOrEmpty(request.RoutePath.Path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (request.RoutePath.Path.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, request.RoutePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {request.RoutePath} is null or invalid.");
            }

            var parameters = request.Parameters ?? request.RoutePath.Path.GetParameters()?.ToObjectDictionary();
            var postData = request.PostData;

            return ExcuteRoute(request.Id, request.RoutePath, parameters, postData, request.RawJson);
        }

        public ChromelyResponse Run(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
        {
            var response = new ChromelyResponse(requestId);
            if (string.IsNullOrEmpty(routePath.Path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.Path.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            var route = ServiceRouteProvider.GetActionRoute(_container, routePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            return ExcuteRoute(requestId, routePath, parameters, postData, requestData);
        }

        private ChromelyResponse ExcuteRoute(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData)
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

        /// <summary>
        /// The get info.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse GetInfo()
        {
            var response = new ChromelyResponse();
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

    }
}
