// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chromely.Core.Infrastructure
{
    public static class RegisterAssembliesExtension
    {
        public static void RegisterAssembly(this IServiceCollection services, Assembly assemby, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var types = from type in assemby.GetLoadableTypes()
                        where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                        select type;

            foreach (var type in types)
            {
                if (typeof(ChromelyController).IsAssignableFrom(type.BaseType))
                {
                    services.Add(new ServiceDescriptor(typeof(ChromelyController), type, lifetime));
                }
            }
        }

        public static void RegisterAssemblies(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var assembly in assemblies)
            {
                RegisterAssembly(services, assembly,lifetime);
            }
        }

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
