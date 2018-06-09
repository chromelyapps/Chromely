// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProvider.cs" company="Chromely">
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

namespace Chromely.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Chromely.Core.RestfulService;

    /// <summary>
    /// The service route provider.
    /// </summary>
    public static class ServiceRouteProvider
    {
        /// <summary>
        /// Adds route.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        public static void AddRoute(string key, Route route)
        {
            IoC.RegisterInstance(typeof(Route), key, route);
        }

        /// <summary>
        /// Merge routes.
        /// </summary>
        /// <param name="newRouteDictionary">
        /// The new route dictionary.
        /// </param>
        public static void MergeRoutes(Dictionary<string, Route> newRouteDictionary)
        {
            if ((newRouteDictionary != null) && (newRouteDictionary.Count > 0))
            {
                foreach (var item in newRouteDictionary)
                {
                    IoC.RegisterInstance(typeof(Route), item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// The get route.
        /// </summary>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <returns>
        /// The <see cref="Route"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route Not found.
        /// </exception>
        public static Route GetRoute(RoutePath routePath)
        {
            object routeObj = IoC.GetInstance(typeof(Route), routePath.Key);
            if ((routeObj == null) || !(routeObj is Route))
            {
                throw new Exception($"No route found for method:{routePath.Method} route path:{routePath.Path}.");
            }

            return (Route)routeObj;
        }
    }
}