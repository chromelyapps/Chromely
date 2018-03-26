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

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    using global::CefSharp;

    /// <summary>
    /// The request task runner.
    /// </summary>
    public static class RequestTaskRunner
    {
        /// <summary>
        /// The run async.
        /// </summary>
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
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task<ChromelyResponse> RunAsync(string routePath, object parameters, object postData)
        {
            var response = new ChromelyResponse();
            if (string.IsNullOrEmpty(routePath))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return Task.FromResult(response);
            }

            if (routePath.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return Task.FromResult(response);
            }

            response = ExcuteRoute(routePath, parameters, postData);
            return Task.FromResult(response);
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
        public static ChromelyResponse Run(IRequest request)
        {
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);

            if (!isCustomScheme)
            {
                throw new Exception($"Url {request.Url} is not of a registered custom scheme.");
            }

            var uri = new Uri(request.Url);
            string routePath = uri.LocalPath;
            var parameters = GetParameters(request.Url);
            object postData = GetPostData(request);

            return Run(routePath, parameters, postData);
        }

        /// <summary>
        /// The run.
        /// </summary>
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
        public static ChromelyResponse Run(string routePath, object parameters, object postData)
        {
            var response = new ChromelyResponse();
            if (string.IsNullOrEmpty(routePath))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.ToLower().Equals("/info"))
            {
                return GetInfo();
            }

            return ExcuteRoute(routePath, parameters, postData);
        }

        /// <summary>
        /// The excute route.
        /// </summary>
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
        private static ChromelyResponse ExcuteRoute(string routePath, object parameters, object postData)
        {
            var route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception($"Route for path = {routePath} is null or invalid.");
            }

            var response = route.Invoke(routePath, parameters: parameters?.ToObjectDictionary(), postData: postData);
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

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            string chromeVersion =
                $"Chromium: {Cef.ChromiumVersion}, CEF: {Cef.CefVersion}, CefSharp: {Cef.CefSharpVersion}, Environment: {bitness}";

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
        /// The get parameters.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The name value collection.
        /// </returns>
        private static IDictionary<string, string> GetParameters(string url)
        {
            var nameValueCollection = new NameValueCollection();

            var querystring = string.Empty;
            var index = url.IndexOf('?');
            if (index > 0)
            {
                querystring = url.Substring(url.IndexOf('?'));
                nameValueCollection = HttpUtility.ParseQueryString(querystring);
            }

            if (string.IsNullOrEmpty(querystring))
            {
                return new Dictionary<string, string>();
            }

            return nameValueCollection.AllKeys.ToDictionary(x => x, x => nameValueCollection[x]);
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
        private static string GetPostData(IRequest request)
        {
            var elements = request?.PostData?.Elements;

            if (elements == null || (elements.Count == 0))
            {
                return string.Empty;
            }

            var dataElement = elements[0];
            return dataElement.GetBody();
        }
    }
}