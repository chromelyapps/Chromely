/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Chromely.Core.Infrastructure;

    public class RouteScanner
    {
        private Assembly m_assembly { get; set; }

        public RouteScanner(Assembly assembly)
        {
            m_assembly = assembly;
        }

        public Dictionary<string, Route> Scan()
        {
            Dictionary<string, Route> routeDictionary = new Dictionary<string, Route>();

            try
            {
                var types = from type in m_assembly.GetLoadableTypes()
                            where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                            select type;

                foreach (var type in types)
                {
                    if (type.BaseType == typeof(ChromelyController))
                    {
                        ChromelyController instance = (ChromelyController)Activator.CreateInstance(type);
                        Dictionary<string, Route> currentRouteDictionary = instance.RouteDictionary;

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
