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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The route scanner.
    /// </summary>
    public class RouteScanner
    {
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
        public RouteScanner(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Scans all registered assemblies for controller classes of <see cref="ChromelyController"/> type.
        /// The classes must have the <see cref="ControllerPropertyAttribute"/> attribute.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public Tuple<Dictionary<string, Route>, Dictionary<string, Command>> Scan()
        {
            var routeDictionary = new Dictionary<string, Route>();
            var commandDictionary = new Dictionary<string, Command>();

            try
            {
                var types = from type in _assembly.GetLoadableTypes()
                    where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                    select type;

                foreach (var type in types)
                {
                    if (typeof(ChromelyController).IsAssignableFrom(type.BaseType))
                    {
                        var instance = ChromelyControllerFactory.CreateControllerInstance(type);
                        var currentRouteDictionary = instance.RouteDictionary;
                        var currentCommandDictionary = instance.CommandDictionary;

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
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return new Tuple<Dictionary<string, Route>, Dictionary<string, Command>>(routeDictionary, commandDictionary);
        }
    }

    /// <summary>
    /// The route scanner extensions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification =
        "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification =
        "Reviewed. Suppression is OK here.")]
    static class RouteScannerExtensions
    {
        /// <summary>
        /// Get loadable types.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
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