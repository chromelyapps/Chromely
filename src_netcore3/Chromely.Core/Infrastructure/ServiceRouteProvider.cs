// --------------------------------------------------------------------------------------------------------------------
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
        public static void AddRoute(string key, Route route)
        {
            IoC.RegisterInstance(typeof(Route), key, route);
        }

        /// <summary>
        /// Adds command.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        public static void AddCommand(string key, Command command)
        {
            IoC.RegisterInstance(typeof(Command), key, command);
        }

        /// <summary>
        /// Merge routes.
        /// </summary>
        /// <param name="newRouteDictionary">
        /// The new route dictionary.
        /// </param>
        public static void MergeRoutes(Dictionary<string, Route> newRouteDictionary)
        {
            if (newRouteDictionary != null && newRouteDictionary.Any())
            {
                foreach (var item in newRouteDictionary)
                {
                    IoC.RegisterInstance(typeof(Route), item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Merge routes.
        /// </summary>
        /// <param name="newCommandDictionary">
        /// The new command dictionary.
        /// </param>
        public static void MergeCommands(Dictionary<string, Command> newCommandDictionary)
        {
            if (newCommandDictionary != null && newCommandDictionary.Any())
            {
                foreach (var item in newCommandDictionary)
                {
                    IoC.RegisterInstance(typeof(Command), item.Key, item.Value);
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

        /// <summary>
        /// The get route.
        /// </summary>
        /// <param name="commandPath">
        /// The command path.
        /// </param>
        /// <returns>
        /// The <see cref="Command"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Route Not found.
        /// </exception>
        public static Command GetCommand(string commandPath)
        {
            var key = Command.GetKeyFromPath(commandPath);
            object commandObj = IoC.GetInstance(typeof(Command), key);
            if ((commandObj == null) || !(commandObj is Command))
            {
                throw new Exception($"No route found for command with key:{key}.");
            }

            return (Command)commandObj;
        }
    }
}