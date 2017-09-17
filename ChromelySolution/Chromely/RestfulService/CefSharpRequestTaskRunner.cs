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
    using System.Threading.Tasks;

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

            ChromelyRequest chromelyRequest = new ChromelyRequest();
            chromelyRequest.Parameters = null;
            chromelyRequest.Data = null;

            response = route.Invoke(chromelyRequest);
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
            infoItemDic.Add("divObjective", "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. Those who are interested can extend to WinForm or WPF. The primary focus of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue) and .NET/Javascript intregation (CefSharp).");
            infoItemDic.Add("divPlatform", "Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.");
            infoItemDic.Add("divVersion", chromeVersion);

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;
  
            return response; 
        }
    }
}