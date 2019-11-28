using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.Network
{
    public class ChromelyControllerFactory
    {
        private readonly IChromelyContainer _container;

        public ChromelyControllerFactory(IChromelyContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates an instance of a chromely controller of given type.
        /// Ctor dependency injection is done using the global IoC container.
        /// </summary>
        /// <param name="type">Controller type to be created.</param>
        /// <returns>Instance reference or null if failed.</returns>
        public ChromelyController CreateControllerInstance(Type type)
        {
            var instance = CreateType(type);
            return instance as ChromelyController;
        }

        public IDictionary<string, ActionRoute> GetHttpAttributeRoutes(ChromelyController controller)
        {
            if (controller == null)
            {
                return null;
            }

            var result = new Dictionary<string, ActionRoute>();

            var methodInfos = controller.GetType().GetMethods()
             .Where(m => m.GetCustomAttributes(typeof(HttpAttribute), false).Length > 0)
             .ToArray();

            foreach (var item in methodInfos)
            {
                var httpAttributeDelegate = CreateDelegate(controller, item) as Func<ChromelyRequest, ChromelyResponse>;
                var attribute = item.GetCustomAttribute<HttpAttribute>();

                if (httpAttributeDelegate != null && attribute != null)
                {
                    var routhPath = new RoutePath(attribute.Method, attribute.Route);
                    result[routhPath.Key] = new ActionRoute(attribute.Method, attribute.Route, httpAttributeDelegate);
                }
            }

            return result;
        }

        public IDictionary<string, CommandRoute>  GetCommandAttributeRoutes(ChromelyController controller)
        {
            if (controller == null)
            {
                return null;
            }

            var result = new Dictionary<string, CommandRoute>();

            var methodInfos = controller.GetType().GetMethods()
             .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
             .ToArray();

            foreach (var item in methodInfos)
            {
                var customAttributeDelegate = CreateDelegate(controller, item) as Action<IDictionary<string, string>>;
                var attribute = item.GetCustomAttribute<CommandAttribute>();

                if (customAttributeDelegate != null && attribute != null)
                {
                    var commandRoute = new CommandRoute(attribute.Route, customAttributeDelegate);
                    result[commandRoute.Key] = commandRoute;
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

                    var iocInstance = _container.GetAllInstances(parameterInfo.ParameterType).FirstOrDefault();
                    var parameterInstance = iocInstance ?? CreateType(parameterInfo.ParameterType);

                    paramValues[ix] = parameterInstance;
                }

                try
                {
                    instance = Activator.CreateInstance(type, paramValues);
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log.Error(ex.Message);
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