/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.RestfulService
{
    using CefSharp;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public static class CefSharpRequestTaskRunner
    {
        public static Task<ChromelyResponse> RunAsync(string routePath, object parameters, object postData)
        {
            ChromelyResponse response = new ChromelyResponse();
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

        public static ChromelyResponse Run(IRequest request)
        {
            ChromelyResponse response = new ChromelyResponse();
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);

            if (!isCustomScheme)
            {
                throw new Exception(string.Format("Url {0} is not of a registered custom scheme.", request.Url));
            }

            var uri = new Uri(request.Url);
            string routePath = uri.LocalPath;
            var parameters = GetParameters(request.Url);
            object postData = GetPostData(request);

            return Run(routePath, parameters, postData);
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
                return GetInfo();
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

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            string chromeVersion = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);

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

        private static string GetPostData(IRequest request)
        {
            if ((request == null) || (request.PostData == null))
            {
                return string.Empty;
            }

            IList<IPostDataElement> elements = request.PostData.Elements;

            if ((elements == null) || (elements.Count == 0))
            {
                return string.Empty;
            }

            IPostDataElement dataElement = elements[0];
            return dataElement.GetBody();
        }
    }
}