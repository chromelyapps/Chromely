// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Route.cs" company="Chromely">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The route.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public Route(Method method, string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.Method = method;
            this.Path = path;
            this.Action = action;
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public Func<ChromelyRequest, ChromelyResponse> Action { get; set; }

        /// <summary>
        /// Gets or sets the action async.
        /// </summary>
        public Func<ChromelyRequest, Task<ChromelyResponse>> ActionAsync { get; set; }

        /// <summary>
        /// Invokes the registered action.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
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
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        public ChromelyResponse Invoke(string requestId, RoutePath routePath, IDictionary<string, object> parameters, object postData)
        {
            ChromelyRequest request = new ChromelyRequest(requestId, routePath, parameters, postData);
            return this.Action.Invoke(request);
        }

        /// <summary>
        /// Invokes the registered action.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        public ChromelyResponse Invoke(ChromelyRequest request)
        {
            return this.Action.Invoke(request);
        }
    }
}
