// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBoundObject.cs" company="Chromely">
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
namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    using System.Threading.Tasks;
    using Chromely.CefSharp.Winapi.RestfulService;
    using Chromely.Core.RestfulService;
    using global::CefSharp;

    /// <summary>
    /// The CefSharp bound object.
    /// </summary>
    public class CefSharpBoundObject
    {
        /// <summary>
        /// The get json.
        /// </summary>
        /// <param name="path">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="javascriptCallback">
        /// The javascript callback.
        /// </param>
        public void GetJson(string path, object parameters, IJavascriptCallback javascriptCallback)
        {
            Task.Run(async () =>
            {
                using (javascriptCallback)
                {
                    var routePath = new RoutePath(Method.GET, path);
                    var chromelyResponse = await RequestTaskRunner.RunAsync(string.Empty, routePath, parameters, null);
                    string jsonResponse = chromelyResponse.EnsureJson();
                    var response = new CallbackResponseStruct(jsonResponse);
                    await javascriptCallback.ExecuteAsync(response);
                }
            });
        }

        /// <summary>
        /// The get json.
        /// </summary>
        /// <param name="path">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetJson(string path, object parameters)
        {
            var routePath = new RoutePath(Method.GET, path);
            var chromelyResponse = RequestTaskRunner.Run(string.Empty, routePath, parameters, null);
            string jsonResponse = chromelyResponse.EnsureJson();
            return jsonResponse;
        }

        /// <summary>
        /// The post json.
        /// </summary>
        /// <param name="path">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <param name="javascriptCallback">
        /// The javascript callback.
        /// </param>
        public void PostJson(string path, object parameters, object postData, IJavascriptCallback javascriptCallback)
        {
            Task.Run(async () =>
            {
                using (javascriptCallback)
                {
                    var routePath = new RoutePath(Method.POST, path);
                    var chromelyResponse = await RequestTaskRunner.RunAsync(string.Empty, routePath, parameters, postData);
                    string jsonResponse = chromelyResponse.EnsureJson();
                    var response = new CallbackResponseStruct(jsonResponse);
                    await javascriptCallback.ExecuteAsync(response);
                }
            });
        }

        /// <summary>
        /// The post json.
        /// </summary>
        /// <param name="path">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string PostJson(string path, object parameters, object postData)
        {
            var routePath = new RoutePath(Method.POST, path);
            var chromelyResponse = RequestTaskRunner.Run(string.Empty, routePath, parameters, postData);
            string jsonResponse = chromelyResponse.EnsureJson();
            return jsonResponse;
        }
    }
}
