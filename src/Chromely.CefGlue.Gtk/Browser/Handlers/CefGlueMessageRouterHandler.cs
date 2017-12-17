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

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Chromely.CefGlue.Gtk.RestfulService;
    using Chromely.Core.RestfulService;
    using LitJson;
    using System;
    using System.Threading.Tasks;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    public class CefGlueMessageRouterHandler : CefMessageRouterBrowserSide.Handler
    {
        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            JsonData requestData = JsonMapper.ToObject(request);
            string method = requestData["method"].ToString();
            method = string.IsNullOrWhiteSpace(method) ? string.Empty : method;

            if (method.Equals("Get", StringComparison.InvariantCultureIgnoreCase) ||
                method.Equals("Post", StringComparison.InvariantCultureIgnoreCase))
            {
                new Task(() =>
                {
                    string routePath = requestData["url"].ToString();
                    object parameters = requestData["parameters"];
                    object postData = requestData["postData"];

                    ChromelyResponse response = RequestTaskRunner.Run(routePath, parameters, postData);
                    string jsonResponse = response.EnsureJson();

                    callback.Success(jsonResponse);
                }).Start();

                return true;
            }

            callback.Failure(100, "Request is not valid.");
            return false;
        }

        public override void OnQueryCanceled(CefBrowser browser, CefFrame frame, long queryId)
        {
        }
    }
}
