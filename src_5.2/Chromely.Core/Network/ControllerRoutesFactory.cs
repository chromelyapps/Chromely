// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class ControllerRoutesFactory
{
    /// <summary>
    /// Creates an instance of a controller of given ControllerBase type.
    /// Ctor dependency injection is done using the global IoC container.
    /// </summary>
    /// <param name="type">Controller type to be created.</param>
    /// <returns>Instance reference or null if failed.</returns>
    public ChromelyController CreateControllerInstance(Type type)
    {
        var instance = CreateType(type);
        return instance as ChromelyController;
    }

    public void CreateAndRegisterRoutes(IChromelyRouteProvider routeProvider, ChromelyController controller, IChromelyModelBinder routeParameterBinder, IChromelyDataTransferOptions dataTransferOptions)
    {
        if (routeProvider == null || controller == null)
        {
            return;
        }

        var methodInfos = controller.GetType().GetMethods()
         .Where(m => m.GetCustomAttributes(typeof(ChromelyRouteAttribute), false).Length > 0)
         .ToArray();

        foreach (var methodInfo in methodInfos)
        {
            var attribute = methodInfo.GetCustomAttribute<ChromelyRouteAttribute>();
            var key = RouteKeys.CreateActionKey(controller.RoutePath, attribute.Path);
            if (!routeProvider.RouteExists(key))
            {
                routeProvider.RegisterRoute(key, CreateDelegate(controller, methodInfo, routeParameterBinder, dataTransferOptions));
            }
        }
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
                Logger.Instance.Log.LogError(ex);
            }
        }

        return instance;
    }

    private static readonly Type VoidType = typeof(void);
    private ControllerRoute CreateDelegate(object instance, MethodInfo method, IChromelyModelBinder routeParameterBinder, IChromelyDataTransferOptions dataTransferOptions)
    {
        var args = method
            .GetParameters();

        var arguments = new List<RouteArgument>();

        int lenght = args.Length;

        for (int i = 0; i < lenght; i++)
        {
            arguments.Add(new RouteArgument(args[i].Name, args[i].ParameterType, i));
        }

        var argTypes = args
            .Select(p => p.ParameterType)
            .Concat(new[] { method.ReturnType })
            .ToArray();

        var newDelType = Expression.GetDelegateType(argTypes);
        var newDel = Delegate.CreateDelegate(newDelType, instance, method);

        bool isAsync = method.ReturnType.IsSubclassOf(typeof(Task));
        bool hasReturn = method.ReturnType != VoidType;

        // It is async method without return (void - System.Threading.Tasks.VoidTaskResult)
        if (method.ReturnType == typeof(Task))
        {
            isAsync = true;
            hasReturn = false;
        }

        return new ControllerRoute(method.Name, newDel, arguments, routeParameterBinder, dataTransferOptions, isAsync, hasReturn);
    }
}

