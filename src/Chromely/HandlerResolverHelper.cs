// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Browser;
using Chromely.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chromely
{
    public static class HandlerResolverHelper
    {
        public static object GetCustomOrDefaultHandler(this ChromelyHandlersResolver resolver, Type type)
        {
            return resolver.GetFirstCustomOrDefaultHandler(type, typeof(IDefaultCustomHandler));
        }

        public static object GetFirstCustomOrDefaultHandler(this ChromelyHandlersResolver resolver, Type type, Type defaultType)
        {
            var handlers = resolver?.Invoke(type)?.ToList();
            if (handlers != null && handlers.Any())
            {
                if (handlers.Count == 1)
                    return handlers[0];

                var customHandler = GetFirstCustomHandler(handlers);
                if (customHandler != null)
                {
                    return customHandler;
                }

                var defaultHandler = GetDefaultHandler(handlers, defaultType); 
                if (defaultHandler != null)
                {
                    return defaultHandler;
                }

                return handlers[0];
            }

            return null;
        }

        public static object GetDefaultHandler(this ChromelyHandlersResolver resolver, Type type, Type defaultType)
        {
            var handlers = resolver?.Invoke(type)?.ToList();
            if (handlers != null && handlers.Any())
            {
                if (handlers.Count == 1)
                    return handlers[0];

                var defaultHandler = GetDefaultHandler(handlers, defaultType);
                if (defaultHandler != null)
                {
                    return defaultHandler;
                }

                return handlers[0];
            }

            return null;
        }

        private static object GetDefaultHandler(List<object> handlers, Type type)
        {
            if (handlers != null && handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    if ((type == typeof(IDefaultCustomHandler)) && ((handler as IDefaultCustomHandler) != null)) return handler;
                    if ((type == typeof(IDefaultResourceCustomHandler)) && ((handler as IDefaultResourceCustomHandler) != null)) return handler;
                    if ((type == typeof(IDefaultAssemblyResourceCustomHandler)) && ((handler as IDefaultAssemblyResourceCustomHandler) != null)) return handler;
                    if ((type == typeof(IDefaultRequestCustomHandler)) && ((handler as IDefaultRequestCustomHandler) != null))return handler;
                    if ((type == typeof(IDefaultExernalRequestCustomHandler)) && ((handler as IDefaultExernalRequestCustomHandler) != null)) return handler;
                }
            }

            return null;
        }

        private static object GetFirstCustomHandler(List<object> handlers)
        {
            if (handlers != null && handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    if (((handler as IDefaultCustomHandler) != null)  ||
                        ((handler as IDefaultResourceCustomHandler) != null) ||
                        ((handler as IDefaultAssemblyResourceCustomHandler) != null) ||
                        ((handler as IDefaultRequestCustomHandler) != null) ||
                        ((handler as IDefaultExernalRequestCustomHandler) != null))
                    {
                        continue; 
                    }

                    return handler;
                }
            }

            return null;
        }
    }
}
