// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBoundObject.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using global::CefSharp;
using Chromely.CefSharp.Winapi.RestfulService;
using Chromely.Core.RestfulService;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
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
