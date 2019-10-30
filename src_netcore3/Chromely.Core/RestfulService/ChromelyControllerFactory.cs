// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyControllerFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// Factory class to create chromely controllers from given type.
    /// </summary>
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

    }
    
    
}