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
        public ChromelyRequest(string routePath, IDictionary<string, object> parameters, object postData)
        {
            this.RoutePath = routePath;
            this.Parameters = parameters;
            this.PostData = postData;
        }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public string RoutePath { get; set; }

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
