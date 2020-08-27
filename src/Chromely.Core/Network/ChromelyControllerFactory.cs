// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public class ChromelyControllerFactory
    {
        public IDictionary<string, RequestActionRoute> GetActionAttributeRoutes(ChromelyController controller)
        {
            if (controller == null)
            {
                return null;
            }

            var result = new Dictionary<string, RequestActionRoute>();

            var methodInfos = controller.GetType().GetMethods()
             .Where(m => m.GetCustomAttributes(typeof(RequestActionAttribute), false).Length > 0)
             .ToArray();

            foreach (var item in methodInfos)
            {
                var actionAttributeDelegate = CreateDelegate(controller, item) as Func<IChromelyRequest, IChromelyResponse>;
                var asyncActionAttributeDelegate = CreateDelegate(controller, item) as Func<IChromelyRequest, Task<IChromelyResponse>>;
                var attribute = item.GetCustomAttribute<RequestActionAttribute>();

                var key = RouteKey.CreateRequestKey(attribute.RouteKey);

                // Sync
                if (actionAttributeDelegate != null && attribute != null)
                {
                    result[key] = new RequestActionRoute(attribute.RouteKey, actionAttributeDelegate, attribute.Description);
                }

                // Async
                if (asyncActionAttributeDelegate != null && attribute != null)
                {
                    result[key] = new RequestActionRoute(attribute.RouteKey, asyncActionAttributeDelegate, attribute.Description);
                }
            }

            return result;
        }

        public IDictionary<string, CommandActionRoute> GetCommandAttributeRoutes(ChromelyController controller)
        {
            if (controller == null)
            {
                return null;
            }

            var result = new Dictionary<string, CommandActionRoute>();

            var methodInfos = controller.GetType().GetMethods()
             .Where(m => m.GetCustomAttributes(typeof(CommandActionAttribute), false).Length > 0)
             .ToArray();

            foreach (var item in methodInfos)
            {
                var customAttributeDelegate = CreateDelegate(controller, item) as Action<IDictionary<string, string>>;
                var attribute = item.GetCustomAttribute<CommandActionAttribute>();
                
                if (customAttributeDelegate != null && attribute != null)
                {
                    var key = RouteKey.CreateCommandKey(attribute.RouteKey);
                    var commandRoute = new CommandActionRoute(attribute.RouteKey, customAttributeDelegate, attribute.Description);
                    result[key] = commandRoute;
                }
            }

            return result;
        }

        private object CreateType(Type type)
        {
            object instance = null;
            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    instance = Activator.CreateInstance(type);
                    break;
                }

                var paramValues = new object[parameters.Length];

                for (var ix = 0; ix < parameters.Length; ix++)
                {
                    var parameterInfo = parameters[ix];
                    var parameterInstance = CreateType(parameterInfo.ParameterType);

                    paramValues[ix] = parameterInstance;
                }

                try
                {
                    instance = Activator.CreateInstance(type, paramValues);
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log.LogError(ex, ex.Message);
                }
            }

            return instance;
        }

        private Delegate CreateDelegate(object instance, MethodInfo method)
        {
            var parameters = method.GetParameters()
                       .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                        .ToArray();

            var call = Expression.Call(Expression.Constant(instance), method, parameters);
            return Expression.Lambda(call, parameters).Compile();
        }
    }
}