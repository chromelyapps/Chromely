﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProvider.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Chromely.Core.RestfulService;

namespace Chromely.Core.Infrastructure
{
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
        [Obsolete("Use IoC container extension method instead.")]
        public static void AddRoute(string key, Route route)
        {
            IoC.Container.AddRoute(key, route);
        }

        /// <summary>
        /// Merge routes.
        /// </summary>
        /// <param name="newRouteDictionary">
        /// The new route dictionary.
        /// </param>
        [Obsolete("Use IoC container extension method instead.")]
        public static void MergeRoutes(Dictionary<string, Route> newRouteDictionary)
        {
            IoC.Container.MergeRoutes(newRouteDictionary);
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
        [Obsolete("Use IoC container extension method instead.")]
        public static Route GetRoute(RoutePath routePath)
        {
            return IoC.Container.GetRoute(routePath);
        }


        /****  Extensions methods ****/

        /// <summary>
        /// Adds route.
        /// </summary>
        /// <param name="container">Container on which register routes</param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        public static void AddRoute(this IChromelyContainer container, string key, Route route)
        {
            container.RegisterInstance(typeof(Route), key, route);
        }

        /// <summary>
        /// Merge routes.
        /// </summary>
        /// <param name="container">Container on which register routes</param>
        /// <param name="newRouteDictionary">
        /// The new route dictionary.
        /// </param>
        public static void MergeRoutes(this IChromelyContainer container, Dictionary<string, Route> newRouteDictionary)
        {
            if (newRouteDictionary != null && newRouteDictionary.Any())
            {
                foreach (var item in newRouteDictionary)
                {
                    container.RegisterInstance(typeof(Route), item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// The get route.
        /// </summary>
        /// <param name="container">IOC container</param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <returns>
        /// The <see cref="Route"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route Not found.
        /// </exception>
        public static Route GetRoute(this IChromelyContainer container, RoutePath routePath)
        {
            object routeObj = container.GetInstance(typeof(Route), routePath.Key);
            if ((routeObj == null) || !(routeObj is Route))
            {
                throw new Exception($"No route found for method:{routePath.Method} route path:{routePath.Path}.");
            }

            return (Route)routeObj;
        }
    }
}