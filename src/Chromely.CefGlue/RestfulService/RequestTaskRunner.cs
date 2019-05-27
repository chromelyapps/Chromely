// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestTaskRunner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;

namespace Chromely.CefGlue.RestfulService
{
    /// <summary>
    /// The request task runner.
    /// </summary>
    public static class RequestTaskRunner
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        public static ChromelyResponse Run(CefRequest request)
        {
            var uri = new Uri(request.Url);
            var path = uri.LocalPath;

            var response = new ChromelyResponse();
            if (string.IsNullOrEmpty(path))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (path.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            var routePath = new RoutePath(request.Method, path);
            var route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {path} is null or invalid.");
            }

            var parameters = request.Url.GetParameters();
            var postData = GetPostData(request);

            return ExcuteRoute(string.Empty, routePath, parameters, postData, string.Empty);
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        public static ChromelyResponse Run(ChromelyRequest request)
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

            var route = ServiceRouteProvider.GetRoute(request.RoutePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {request.RoutePath} is null or invalid.");
            }

            var parameters = request.Parameters ?? request.RoutePath.Path.GetParameters()?.ToObjectDictionary();
            var postData = request.PostData;

            return ExcuteRoute(request.Id, request.RoutePath, parameters, postData, request.RawJson);
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <param name="requestData">
        /// Raw json request data.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        public static ChromelyResponse Run(string requestId, RoutePath routePath, object parameters, object postData, string requestData)
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

            var route = ServiceRouteProvider.GetRoute(routePath);
            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            return ExcuteRoute(requestId, routePath, parameters, postData, requestData);
        }

        /// <summary>
        /// The excute route.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <param name="requestData">
        /// Raw json request data.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        private static ChromelyResponse ExcuteRoute(string requestId, RoutePath routePath, object parameters, object postData, string requestData)
        {
            var route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            var response = route.Invoke(requestId: requestId, routePath: routePath, parameters: parameters?.ToObjectDictionary(), postData: postData, rawJson: requestData);
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
        private static ChromelyResponse GetInfo()
        {
            var response = new ChromelyResponse();
            var chromeVersion = CefRuntime.ChromeVersion;

            var infoItemDic = new Dictionary<string, string>
            {
                {
                    "divObjective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp)."
                },
                {
                    "divPlatform",
                    "Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above."
                },
                { "divVersion", chromeVersion }
            };

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;

            return response;
        }

        /// <summary>
        /// The get post data.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetPostData(CefRequest request)
        {
            var postDataElements = request?.PostData?.GetElements();
            if (postDataElements == null || (postDataElements.Length == 0))
            {
                return string.Empty;
            }

            var dataElement = postDataElements[0];

            switch (dataElement.ElementType)
            {
                case CefPostDataElementType.Empty:
                    break;
                case CefPostDataElementType.File:
                    break;
                case CefPostDataElementType.Bytes:
                    return Encoding.UTF8.GetString(dataElement.GetBytes());
            }

            return string.Empty; 
        }
    }
}
