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

namespace Chromely.Core.RestfulService
{
    using Chromely.Core.Infrastructure;
    using LitJson;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Xilium.CefGlue;


    public static class RequestTaskRunner
    {
        public static void Run(CefRequest request, CefCallback callback, TaskData response)
        {
            var uri = new Uri(request.Url);
            string routePath = uri.LocalPath;

            if (string.IsNullOrEmpty(routePath))
            {
                return;
            }

            if (routePath.ToLower().Equals("/info"))
            {
                RunInfo(request, callback, response);
                return;
            }

            Task.Run(() =>
            {
                Route route = ServiceRouteFactory.GetRoute(routePath);

                if (route == null)
                {
                    throw new Exception(string.Format("Route for path = {0} is null or invalid.", routePath));
                }

                var parameters = GetParameters(request.Url);
                string postData = GetPostData(request);

                ChromelyRequest chromelyRequest = new ChromelyRequest();
                chromelyRequest.Parameters = parameters;
                chromelyRequest.Data = postData;

                ChromelyResponse chromelyResponse = route.Invoke(chromelyRequest);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.JsonData = chromelyResponse.JsonData;

                callback.Continue();
            });
        }

        private static void RunInfo(CefRequest request, CefCallback callback, TaskData response)
        {
            Task.Run(() =>
            {
                string chromeVersion = CefRuntime.ChromeVersion;

                Dictionary<string, string> infoItemDic = new Dictionary<string, string>();
                infoItemDic.Add("divObjective", "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. Those who are interested can extend to WinForm or WPF. The primary focus of communication with Chromium is via Ajax HTTP/XHR requests using custom schemes and domains.");
                infoItemDic.Add("divPlatform", "Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.");
                infoItemDic.Add("divVersion", chromeVersion);
                string jsonInfoData = JsonMapper.ToJson(infoItemDic);

                ChromelyResponse chromelyResponse = new ChromelyResponse();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.JsonData = jsonInfoData;

                callback.Continue();
            });
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
