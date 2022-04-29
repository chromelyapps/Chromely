// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

public static class HandlerResolverHelper
{
    /// <summary>
    /// Gets CEF handler based on the type.
    /// </summary>
    /// <remarks>
    /// If a custom handler is registered, the handler is returned, otherwise the default handler is returned.
    /// </remarks>
    /// <param name="resolver">Instance of <see cref="ChromelyHandlersResolver"/>.</param>
    /// <param name="type">The handler type.</param>
    /// <returns>instance of handler object.</returns>
    public static object? GetCustomOrDefaultHandler(this ChromelyHandlersResolver resolver, Type type)
    {
        return resolver.GetFirstCustomOrDefaultHandler(type, typeof(IDefaultCustomHandler));
    }

    /// <summary>
    /// Gets CEF handler based on the type and the default type.
    /// </summary>
    /// <remarks>
    /// If multiple handlers are registered, the first custom handler is returned, otherwise the first default handler is returned.
    /// </remarks>
    /// <param name="resolver">Instance of <see cref="ChromelyHandlersResolver"/>.</param>
    /// <param name="type">The handler type.</param>
    /// <param name="defaultType">The default handler type.</param>
    /// <returns>instance of handler object.</returns>
    public static object? GetFirstCustomOrDefaultHandler(this ChromelyHandlersResolver resolver, Type type, Type defaultType)
    {
        var handlers = resolver?.Invoke(type)?.ToList();
        if (handlers is not null && handlers.Any())
        {
            if (handlers.Count == 1)
                return handlers[0];

            var customHandler = GetFirstCustomHandler(handlers);
            if (customHandler is not null)
            {
                return customHandler;
            }

            var defaultHandler = GetDefaultHandler(handlers, defaultType);
            if (defaultHandler is not null)
            {
                return defaultHandler;
            }

            return handlers[0];
        }

        return default;
    }

    /// <summary>
    /// Gets default CEF handler based on the type and the default type.
    /// </summary>
    /// <remarks>
    /// If multiple default handlers are registered, first default handler is returned.
    /// </remarks>
    /// <param name="resolver">Instance of <see cref="ChromelyHandlersResolver"/>.</param>
    /// <param name="type">The handler type.</param>
    /// <param name="defaultType">The default handler type.</param>
    /// <returns>instance of handler object.</returns>
    public static object? GetDefaultHandler(this ChromelyHandlersResolver resolver, Type type, Type defaultType)
    {
        var handlers = resolver?.Invoke(type)?.ToList();
        if (handlers is not null && handlers.Any())
        {
            if (handlers.Count == 1)
                return handlers[0];

            var defaultHandler = GetDefaultHandler(handlers, defaultType);
            if (defaultHandler is not null)
            {
                return defaultHandler;
            }

            return handlers[0];
        }

        return default;
    }

    private static object? GetDefaultHandler(List<object?> handlers, Type type)
    {
        if (handlers is not null && handlers.Any())
        {
            foreach (var handler in handlers)
            {
                if ((type == typeof(IDefaultCustomHandler)) && ((handler as IDefaultCustomHandler) is not null)) return handler;
                if ((type == typeof(IDefaultResourceCustomHandler)) && ((handler as IDefaultResourceCustomHandler) is not null)) return handler;
                if ((type == typeof(IDefaultAssemblyResourceCustomHandler)) && ((handler as IDefaultAssemblyResourceCustomHandler) is not null)) return handler;
                if ((type == typeof(IDefaultRequestCustomHandler)) && ((handler as IDefaultRequestCustomHandler) is not null)) return handler;
                if ((type == typeof(IDefaultExernalRequestCustomHandler)) && ((handler as IDefaultExernalRequestCustomHandler) is not null)) return handler;
                if ((type == typeof(IDefaultOwinCustomHandler)) && ((handler as IDefaultOwinCustomHandler) is not null)) return handler;
            }
        }

        return default;
    }

    private static object? GetFirstCustomHandler(List<object?> handlers)
    {
        if (handlers is not null && handlers.Any())
        {
            foreach (var handler in handlers)
            {
                if (((handler as IDefaultCustomHandler) is not null) ||
                    ((handler as IDefaultResourceCustomHandler) is not null) ||
                    ((handler as IDefaultAssemblyResourceCustomHandler) is not null) ||
                    ((handler as IDefaultRequestCustomHandler) is not null) ||
                    ((handler as IDefaultExernalRequestCustomHandler) is not null) ||
                    ((handler as IDefaultOwinCustomHandler) is not null))
                {
                    continue;
                }

                return handler;
            }
        }

        return default;
    }
}