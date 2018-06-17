// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyRequest.cs" company="Chromely">
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

namespace Chromely.Core.RestfulService
{
    using System.Collections.Generic;

    using LitJson;

    /// <summary>
    /// The chromely request.
    /// </summary>
    public class ChromelyRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        public ChromelyRequest(RoutePath routePath, IDictionary<string, object> parameters, object postData)
        {
            this.RoutePath = routePath;
            this.Parameters = parameters;
            this.PostData = postData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        public ChromelyRequest(string id, RoutePath routePath, IDictionary<string, object> parameters, object postData)
        {
            this.Id = id;
            this.RoutePath = routePath;
            this.Parameters = parameters;
            this.PostData = postData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyRequest"/> class.
        /// </summary>
        /// <param name="jsonData">
        /// The json data.
        /// </param>
        public ChromelyRequest(JsonData jsonData)
        {
            string method = jsonData.Keys.Contains("method") ? jsonData["method"].ToString() : "get";
            string url = jsonData.Keys.Contains("url") ? jsonData["url"].ToString() : string.Empty;
            this.RoutePath = new RoutePath(method, url);

            this.Id = jsonData.Keys.Contains("id") ? jsonData["id"].ToString() : this.Id;
            this.Parameters = jsonData.Keys.Contains("parameters") ? jsonData["parameters"]?.ObjectToDictionary() : this.Parameters;
            this.PostData = jsonData.Keys.Contains("postData") ? jsonData["postData"] : this.PostData;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public RoutePath RoutePath { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the post data.
        /// </summary>
        public object PostData { get; set; }
    }
}
