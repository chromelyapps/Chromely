// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyController.cs" company="Chromely">
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
    using System.Reflection;
    using System.Threading.Tasks;

    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The chromely controller.
    /// </summary>
    public abstract class ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyController"/> class.
        /// </summary>
        protected ChromelyController()
        {
            this.RouteDictionary = new Dictionary<string, Route>();
        }

        /// <summary>
        /// Gets the route dictionary.
        /// </summary>
        public Dictionary<string, Route> RouteDictionary { get; }

        /// <summary>
        /// Gets the route name.
        /// </summary>
        public string RouteName
        {
            get
            {
                var attributeInfo = this.GetAttributeInfo();
                if (attributeInfo != null)
                {
                    return attributeInfo.Item1;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the route path.
        /// </summary>
        public string Path
        {
            get
            {
                var attributeInfo = this.GetAttributeInfo();
                if (attributeInfo != null)
                {
                    return attributeInfo.Item2;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// The register get request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterGetRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.GET, path, new Route(Method.GET, path, action));
        }

        /// <summary>
        /// The register post request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPostRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.POST, path, new Route(Method.POST, path, action));
        }

        /// <summary>
        /// The register put request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPutRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.PUT, path, new Route(Method.PUT, path, action));
        }

        /// <summary>
        /// The register delete request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterDeleteRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.DELETE, path, new Route(Method.DELETE, path, action));
        }

        /// <summary>
        /// The register head request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterHeadRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.HEAD, path, new Route(Method.HEAD, path, action));
        }

        /// <summary>
        /// The register options request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterOptionsRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.OPTIONS, path, new Route(Method.OPTIONS, path, action));
        }

        /// <summary>
        /// The register patch request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPatchRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.PATCH, path, new Route(Method.PATCH, path, action));
        }

        /// <summary>
        /// The register merge request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterMergeRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            this.AddRoute(Method.MERGE, path, new Route(Method.MERGE, path, action));
        }

        /// <summary>
        /// The add route.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        protected void AddRoute(Method method, string path, Route route)
        {
            var routhPath = new RoutePath(method, path);
            this.RouteDictionary[routhPath.Key] = route;
        }

        /// <summary>
        /// The add route.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        protected void AddRoute(string method, string path, Route route)
        {
            var routhPath = new RoutePath(method, path);
            this.RouteDictionary[routhPath.Key] = route;
        }

        /// <summary>
        /// The get attribute info.
        /// </summary>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<string, string> GetAttributeInfo()
        {
            try
            {
                var attribute = this.GetType().GetCustomAttribute<ControllerPropertyAttribute>(true);
                if (attribute != null)
                {
                    return new Tuple<string, string>(attribute.Name, attribute.Route);
                }

                return null;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return null;
        }
    }
}
