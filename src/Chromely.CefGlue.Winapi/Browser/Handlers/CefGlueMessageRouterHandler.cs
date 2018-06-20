// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueMessageRouterHandler.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System.Threading.Tasks;
    using Chromely.CefGlue.Winapi.RestfulService;
    using Chromely.Core.RestfulService;
    using LitJson;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    /// <summary>
    /// The CefGlue message router handler.
    /// </summary>
    public class CefGlueMessageRouterHandler : CefMessageRouterBrowserSide.Handler
    {
        /// <summary>
        /// The on query.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="queryId">
        /// The query id.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="persistent">
        /// The persistent.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            var requestData = JsonMapper.ToObject(request);
            var method = requestData.Keys.Contains("method") ? requestData["method"].ToString() : string.Empty;
            method = string.IsNullOrWhiteSpace(method) ? string.Empty : method;

            if (RoutePath.ValidMethod(method))
            {
                new Task(() =>
                {
                    var id = requestData.Keys.Contains("id") ? requestData["id"].ToString() : string.Empty;
                    var path = requestData.Keys.Contains("url") ? requestData["url"].ToString() : string.Empty;
                    var parameters = requestData.Keys.Contains("parameters") ? requestData["parameters"] : null;
                    var postData = requestData.Keys.Contains("postData") ? requestData["postData"] : null;

                    var routePath = new RoutePath(method, path);
                    var response = RequestTaskRunner.Run(id, routePath, parameters, postData);
                    var jsonResponse = response.EnsureJson();

                    callback.Success(jsonResponse);
                }).Start();

                return true;
            }

            callback.Failure(100, "Request is not valid.");
            return false;
        }

        /// <summary>
        /// The on query canceled.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="queryId">
        /// The query id.
        /// </param>
        public override void OnQueryCanceled(CefBrowser browser, CefFrame frame, long queryId)
        {
        }
    }
}
