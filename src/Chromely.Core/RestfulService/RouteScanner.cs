// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScanner.cs" company="Chromely">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The route scanner.
    /// </summary>
    public class RouteScanner
    {
        /// <summary>
        /// Gets or sets the mAssembly.
        /// </summary>
        private readonly Assembly mAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScanner"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public RouteScanner(Assembly assembly)
        {
            this.mAssembly = assembly;
        }

        /// <summary>
        /// Scans all registered assemblies for controller classes of <see cref="ChromelyController"/> type.
        /// The classes must have the <see cref="ControllerPropertyAttribute"/> attribute.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public Dictionary<string, Route> Scan()
        {
            var routeDictionary = new Dictionary<string, Route>();

            try
            {
                var types = from type in this.mAssembly.GetLoadableTypes()
                            where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                            select type;

                foreach (var type in types)
                {
                    if (type.BaseType == typeof(ChromelyController))
                    {
                        var instance = (ChromelyController)Activator.CreateInstance(type);
                        var currentRouteDictionary = instance.RouteDictionary;

                        // Merge with return route dictionary
                        if ((currentRouteDictionary != null) && (currentRouteDictionary.Count > 0))
                        {
                            foreach (var item in currentRouteDictionary)
                            {
                                if (!routeDictionary.ContainsKey(item.Key))
                                {
                                    routeDictionary.Add(item.Key, item.Value);
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

            return routeDictionary;
        }
    }

    /// <summary>
    /// The route scanner extensions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed. Suppression is OK here.")]
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
