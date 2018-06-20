// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestTaskRunner.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xilium.CefGlue;

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
            string path = uri.LocalPath;

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

            return ExcuteRoute(string.Empty, routePath, parameters, postData);
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
            var response = new ChromelyResponse();
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

            return ExcuteRoute(string.Empty, request.RoutePath, parameters, postData);
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
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        public static ChromelyResponse Run(string requestId, RoutePath routePath, object parameters, object postData)
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

            return ExcuteRoute(requestId, routePath, parameters, postData);
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
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route path not valid.
        /// </exception>
        private static ChromelyResponse ExcuteRoute(string requestId, RoutePath routePath, object parameters, object postData)
        {
            var route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            var response = route.Invoke(requestId: requestId, routePath: routePath, parameters: parameters?.ToObjectDictionary(), postData: postData);
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
            CefPostDataElement[] postDataElements = request?.PostData?.GetElements();

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
