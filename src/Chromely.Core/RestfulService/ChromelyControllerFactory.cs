using System;
using System.Linq;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.RestfulService
{
    public static class ChromelyControllerFactory
    {
        public static ChromelyController CreateControllerInstance(Type type)
        {
            var instance = CreateType(type);
            return instance as ChromelyController;
        }
    
        
        private static object CreateType(Type type)
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

                    var iocInstance = IoC.GetAllInstances(parameterInfo.ParameterType).FirstOrDefault();
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
                    Log.Error(ex.Message);
                }
            }

            return instance;
        }

    }
    
    
}