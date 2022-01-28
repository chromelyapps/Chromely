// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Chromely.Core
{
    public sealed class AppBuilder
    {
        private IServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;
        private ChromelyServiceProviderFactory _serviceProviderFactory;
        private ChromelyApp _chromelyApp;
        private IChromelyConfiguration _config;
        private IChromelyWindow _chromelyWindow;
        private IChromelyErrorHandler _chromelyErrorHandler;
        private Type _chromelyUseConfigType;
        private Type _chromelyUseWindowType;
        private Type _chromelyUseErrorHandlerType;
        private int _stepCompleted;

        private AppBuilder()
        {
            _config = null;
            _chromelyUseConfigType = null;
            _chromelyUseWindowType = null;
            _chromelyUseErrorHandlerType = null;
            _stepCompleted = -1;
        }

        public static AppBuilder Create()
        {
            var appBuilder = new AppBuilder();
            return appBuilder;
        }

        public AppBuilder UseServices(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            return this;
        }

        public AppBuilder UseServiceProviderFactory(ChromelyServiceProviderFactory serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory;
            return this;
        }

        public AppBuilder UseConfig<TService>(IChromelyConfiguration config = null) where TService : IChromelyConfiguration
        {
            if (config != null)
            {
                _config = config;
            }
            else
            {
                _chromelyUseConfigType = null;
                EnsureIsDerivedType(typeof(IChromelyConfiguration), typeof(TService));
                _chromelyUseConfigType = typeof(TService);
            }

            return this;
        }

        public AppBuilder UseWindow<TService>(IChromelyWindow chromelyWindow = null) where TService : IChromelyWindow
        {
            if (chromelyWindow != null)
            {
                _chromelyWindow = chromelyWindow;
            }
            else
            {
                _chromelyUseWindowType = null;
                EnsureIsDerivedType(typeof(IChromelyWindow), typeof(TService));
                _chromelyUseWindowType = typeof(TService);
            }

            return this;
        }

        public AppBuilder UseErrorHandler<TService>(IChromelyErrorHandler chromelyErrorHandler = null) where TService : IChromelyErrorHandler
        {
            if (chromelyErrorHandler != null)
            {
                _chromelyErrorHandler = chromelyErrorHandler;
            }
            else
            {
                _chromelyUseErrorHandlerType = null;
                EnsureIsDerivedType(typeof(IChromelyErrorHandler), typeof(TService));
                _chromelyUseErrorHandlerType = typeof(TService);
            }

            return this;
        }

        public AppBuilder UseApp<TApp>(ChromelyApp chromelyApp = null) where TApp : ChromelyApp
        {
            _chromelyApp = chromelyApp;
            if (_chromelyApp == null)
            {
                EnsureIsDerivedType(typeof(ChromelyApp), typeof(TApp));
                _chromelyApp = (TApp)Activator.CreateInstance(typeof(TApp));
            }

            _stepCompleted = 1;
            return this;
        }

        public AppBuilder Build()
        {
            if (_stepCompleted != 1)
            {
                throw new Exception("Invalid order: Step 1: UseApp must be completed before Step 2: Build.");
            }

            if (_chromelyApp == null)
            {
                throw new Exception($"ChromelyApp {nameof(_chromelyApp)} cannot be null.");
            }

            if (_serviceCollection == null)
            {
                _serviceCollection = new ServiceCollection();
            }

            _chromelyApp.ConfigureServices(_serviceCollection);

            // This must be done before registering core services
            RegisterUseComponents(_serviceCollection);

            _chromelyApp.ConfigureCoreServices(_serviceCollection);
            _chromelyApp.ConfigureServiceResolvers(_serviceCollection);
            _chromelyApp.ConfigureDefaultHandlers(_serviceCollection);

            if (_serviceProviderFactory != null)
            {
                _serviceProvider = _serviceProviderFactory.BuildServiceProvider(_serviceCollection);
            }
            else
            {
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            }

            _chromelyApp.Initialize(_serviceProvider);
            _chromelyApp.RegisterControllerRoutes(_serviceProvider);

            _stepCompleted = 2;
            return this;
        }

        public void Run(string[] args)
        {
            if (_stepCompleted != 2)
            {
                throw new Exception("Invalid order: Step 2: Build must be completed before Step 3: Run.");
            }

            if (_serviceProvider == null)
            {
                throw new Exception("ServiceProvider is not initialized.");
            }

            try
            {
                var appName = Assembly.GetEntryAssembly()?.GetName().Name;
                var windowController = _serviceProvider.GetService<ChromelyWindowController>();
                try
                {
                    Logger.Instance.Log.LogInformation($"Running application:{appName}.");
                    windowController.Run(args);
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
                }
                finally
                {
                    windowController.Dispose();
                    (_serviceProvider as ServiceProvider)?.Dispose();
                }

            }
            catch (Exception exception)
            {
                var appName = Assembly.GetEntryAssembly()?.GetName().Name;
                Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
            }
        }

        private void EnsureIsDerivedType(Type baseType, Type derivedType)
        {
            if (baseType == derivedType)
            {
                throw new Exception($"Cannot specify the base type {baseType.Name} itself as generic type parameter.");
            }

            if (!baseType.IsAssignableFrom(derivedType))
            {
                throw new Exception($"Type {derivedType.Name} must implement {baseType.Name}.");
            }

            if (derivedType.IsAbstract || derivedType.IsInterface)
            {
                throw new Exception($"Type {derivedType.Name} cannot be an interface or abstract class.");
            }
        }

        private void RegisterUseComponents(IServiceCollection services)
        {
            #region IChromelyConfiguration

            if (_config != null)
            {
                services.TryAddSingleton<IChromelyConfiguration>(_config);
            }
            else if (_chromelyUseConfigType != null)
            {
                services.TryAddSingleton(typeof(IChromelyConfiguration), _chromelyUseConfigType);
            }

            #endregion IChromelyConfiguration

            #region IChromelyWindow

            if (_chromelyWindow != null)
            {
                services.TryAddSingleton<IChromelyWindow>(_chromelyWindow);
            }
            else if (_chromelyUseWindowType != null)
            {
                services.TryAddSingleton(typeof(IChromelyWindow), _chromelyUseWindowType);
            }

            #endregion IChromelyWindow

            #region IChromelyErrorHandler

            if (_chromelyErrorHandler != null)
            {
                services.TryAddSingleton<IChromelyErrorHandler>(_chromelyErrorHandler);
            }
            else if (_chromelyUseErrorHandlerType != null)
            {
                services.TryAddSingleton(typeof(IChromelyErrorHandler), _chromelyUseErrorHandlerType);
            }

            #endregion IChromelyErrorHandler
        }
    }
}