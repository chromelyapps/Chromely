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
    /// <summary>
    /// The route path.
    /// </summary>
    public class RoutePath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePath"/> class.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public RoutePath(Method method, string path)
        {
            this.Method = method;
            string methodString = this.ConvertMethod(method);
            path = string.IsNullOrEmpty(path) ? string.Empty : path;
            string routeKey = $"{methodString}_{path}";
            this.Path = path;
            this.Key = routeKey.ToLower();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePath"/> class.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public RoutePath(string method, string path)
        {
            string methodString = string.IsNullOrEmpty(method) ? "get" : method;
            path = string.IsNullOrEmpty(path) ? string.Empty : path;
            string routeKey = $"{methodString}_{path}";
            this.Path = path;
            this.Key = routeKey.ToLower();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The valid method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ValidMethod(Method method)
        {
            switch (method)
            {
                case Method.GET:
                case Method.POST:
                case Method.PUT:
                case Method.DELETE:
                case Method.HEAD:
                case Method.OPTIONS:
                case Method.PATCH:
                case Method.MERGE:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The valid method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ValidMethod(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                return false;
            }

            switch (method.ToUpper())
            {
                case "GET":
                case "POST":
                case "PUT":
                case "DELETE":
                case "HEAD":
                case "OPTIONS":
                case "PATCH":
                case "MERGE":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The convert method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertMethod(Method method)
        {
            switch (method)
            {
                case Method.None:
                    return "None";
                case Method.GET:
                    return "GET";
                case Method.POST:
                    return "POST";
                case Method.PUT:
                    return "PUT";
                case Method.DELETE:
                    return "DELETE";
                case Method.HEAD:
                    return "HEAD";
                case Method.OPTIONS:
                    return "OPTIONS";
                case Method.PATCH:
                    return "PATCH";
                case Method.MERGE:
                    return "MERGE";
                default:
                    return "GET";
            }
        }
    }
}
