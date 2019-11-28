// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScanner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.Network
{
    /// <summary>
    /// The route scanner.
    /// </summary>
    public class RouteScanner
    {
        private readonly IChromelyContainer _container;

        /// <summary>
        /// Gets or sets the _assembly.
        /// </summary>
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScanner"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public RouteScanner(Assembly assembly, IChromelyContainer container)
        {
            _assembly = assembly;
            _container = container;
        }

        /// <summary>
        /// Scans all registered assemblies for controller classes of <see cref="ChromelyController"/> type.
        /// The classes must have the <see cref="ControllerPropertyAttribute"/> attribute.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public Tuple<Dictionary<string, ActionRoute>, Dictionary<string, CommandRoute>> Scan()
        {
            var routeDictionary = new Dictionary<string, ActionRoute>();
            var commandDictionary = new Dictionary<string, CommandRoute>();

            try
            {
                var types = from type in _assembly.GetLoadableTypes()
                    where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                    select type;

                foreach (var type in types)
                {
                    if (typeof(ChromelyController).IsAssignableFrom(type.BaseType))
                    {
                        var controllerFactory = new ChromelyControllerFactory(_container);
                        var instance = controllerFactory.CreateControllerInstance(type);
                        var currentRouteDictionary = instance.ActionRouteDictionary;
                        var currentCommandDictionary = instance.CommandRouteDictionary;

                        // Merge with return route dictionary
                        if ((currentRouteDictionary != null) && currentRouteDictionary.Any())
                        {
                            foreach (var item in currentRouteDictionary)
                            {
                                if (!routeDictionary.ContainsKey(item.Key))
                                {
                                    routeDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }

                        // Merge with return command dictionary
                        if ((currentCommandDictionary != null) && currentCommandDictionary.Any())
                        {
                            foreach (var item in currentCommandDictionary)
                            {
                                if (!commandDictionary.ContainsKey(item.Key))
                                {
                                    commandDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }

                        // Add Http Attributes
                        var httpAttributeRoutes = controllerFactory.GetHttpAttributeRoutes(instance);
                        if ((httpAttributeRoutes != null) && httpAttributeRoutes.Any())
                        {
                            foreach (var item in httpAttributeRoutes)
                            {
                                if (!routeDictionary.ContainsKey(item.Key))
                                {
                                    routeDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }

                        // Add Custom Attributes
                        var customAttributeRoutes = controllerFactory.GetCommandAttributeRoutes(instance);
                        if ((customAttributeRoutes != null) && customAttributeRoutes.Any())
                        {
                            foreach (var item in customAttributeRoutes)
                            {
                                if (!commandDictionary.ContainsKey(item.Key))
                                {
                                    commandDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return new Tuple<Dictionary<string, ActionRoute>, Dictionary<string, CommandRoute>>(routeDictionary, commandDictionary);
        }
    }

    static class RouteScannerExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}