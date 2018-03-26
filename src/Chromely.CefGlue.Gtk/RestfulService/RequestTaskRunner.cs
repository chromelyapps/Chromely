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
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xilium.CefGlue;

    public static class RequestTaskRunner
    {
        public static ChromelyResponse Run(CefRequest request)
        {
            var uri = new Uri(request.Url);
            string routePath = uri.LocalPath;

            ChromelyResponse response = new ChromelyResponse();
            if (string.IsNullOrEmpty(routePath))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            Route route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception(string.Format("Route for path = {0} is null or invalid.", routePath));
            }

            var parameters = GetParameters(request.Url);
            string postData = GetPostData(request);

            return ExcuteRoute(routePath, parameters, postData);
        }

        public static ChromelyResponse Run(string routePath, object parameters, object postData)
        {
            ChromelyResponse response = new ChromelyResponse();
            if (string.IsNullOrEmpty(routePath))
            {
                response.ReadyState = (int)ReadyState.ResponseIsReady;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Bad Request";

                return response;
            }

            if (routePath.ToLower().Equals("/info"))
            {
                response = GetInfo();
                return response;
            }

            Route route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception(string.Format("Route for path = {0} is null or invalid.", routePath));
            }

            return ExcuteRoute(routePath, parameters, postData);
        }

        private static ChromelyResponse ExcuteRoute(string routePath, object parameters, object postData)
        {
            ChromelyResponse response = new ChromelyResponse();

            Route route = ServiceRouteProvider.GetRoute(routePath);

            if (route == null)
            {
                throw new Exception(string.Format("Route for path = {0} is null or invalid.", routePath));
            }

            response = route.Invoke(routePath, parameters: parameters?.ToObjectDictionary(), postData: postData);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";

            return response;
        }

        private static ChromelyResponse GetInfo()
        {
            ChromelyResponse response = new ChromelyResponse();

            string chromeVersion = CefRuntime.ChromeVersion;

            Dictionary<string, string> infoItemDic = new Dictionary<string, string>();
            infoItemDic.Add("divObjective", "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp).");
            infoItemDic.Add("divPlatform", "Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.");
            infoItemDic.Add("divVersion", chromeVersion);

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;

            return response;
        }

        private static IDictionary<string, string> GetParameters(string url)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();

            string querystring = string.Empty;
            int index = url.IndexOf('?');
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

        private static string GetPostData(CefRequest request)
        {
            if ((request == null) || (request.PostData == null))
            {
                return string.Empty;
            }

            CefPostDataElement[] postDataElements = request.PostData.GetElements();

            if ((postDataElements == null) || (postDataElements.Length == 0))
            {
                return string.Empty;
            }

            CefPostDataElement dataElement = postDataElements[0];

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
