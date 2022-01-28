// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class RegisterAssembliesExtensions
{
    /// <summary>
    /// Register ChromelyController assembly.
    /// </summary>
    /// <remarks>
    /// The assembly (dll) are where atleast one custom controller class derived from <see cref="ChromelyController"/> is defined.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assemby">The assembly where controller classes are defined.</param>
    /// <param name="lifetime">The services lifetime.</param>
    public static void RegisterChromelyControllerAssembly(this IServiceCollection services, Assembly assemby, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var types = from type in assemby.GetLoadableTypes()
                    where Attribute.IsDefined(type, typeof(ChromelyControllerAttribute))
                    select type;

        foreach (var type in types)
        {
            if (typeof(ChromelyController).IsAssignableFrom(type.BaseType))
            {
                services.Add(new ServiceDescriptor(typeof(ChromelyController), type, lifetime));
            }
        }
    }

    /// <summary>
    /// Register ChromelyController assemblies.
    /// </summary>
    /// <remarks>
    /// The assemblies (dlls) are where each has atleast one custom controller class derived from <see cref="ChromelyController"/> is defined.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assemblies">The list of assemblies where controller classes are defined.</param>
    /// <param name="lifetime">The services lifetime.</param>
    public static void RegisterChromelyControllerAssemblies(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        foreach (var assembly in assemblies)
        {
            RegisterChromelyControllerAssembly(services, assembly, lifetime);
        }
    }

    public static IEnumerable<Type?> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t is not null);
        }
    }
}