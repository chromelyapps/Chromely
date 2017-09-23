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

namespace Chromely.CefSharpBrowser.Handlers
{
    using CefSharp;
    using Chromely.Core.RestfulService;
    using Chromely.RestfulService;
    using LitJson;
    using System.Threading.Tasks;

    public class CefSharpBoundObject
    {
        public void GetJson(string routePath, object parameters, IJavascriptCallback javascriptCallback)
        {
            Task.Run(async () =>
            {
                using (javascriptCallback)
                {
                    ChromelyResponse chromelyResponse = await CefSharpRequestTaskRunner.RunAsync(routePath, parameters, null);
                    string jsonResponse = chromelyResponse.EnsureJson();
                    var response = new CallbackResponseStruct(jsonResponse);
                    await javascriptCallback.ExecuteAsync(response);
                }
            });
        }

        public string GetJson(string routePath, object parameters)
        {
            ChromelyResponse chromelyResponse = CefSharpRequestTaskRunner.Run(routePath, parameters, null);
            string jsonResponse = chromelyResponse.EnsureJson();
            return jsonResponse;
        }

        public void PostJson(string routePath, object parameters, object postData, IJavascriptCallback javascriptCallback)
        {
            Task.Run(async () =>
            {
                using (javascriptCallback)
                {
                    ChromelyResponse chromelyResponse = await CefSharpRequestTaskRunner.RunAsync(routePath, parameters, postData);
                    string jsonResponse = chromelyResponse.EnsureJson();
                    var response = new CallbackResponseStruct(jsonResponse);
                    await javascriptCallback.ExecuteAsync(response);
                }
            });
        }

        public string PostJson(string routePath, object parameters, object postData)
        {
            ChromelyResponse chromelyResponse = CefSharpRequestTaskRunner.Run(routePath, parameters, postData);
            string jsonResponse = chromelyResponse.EnsureJson();
            return jsonResponse;
        }
    }
}
