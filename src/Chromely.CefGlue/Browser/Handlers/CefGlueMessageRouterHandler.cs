// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueMessageRouterHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using Chromely.CefGlue.RestfulService;
using Chromely.Core.RestfulService;
using LitJson;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.Browser.Handlers
{
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
                    var response = RequestTaskRunner.Run(id, routePath, parameters, postData, request);
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
