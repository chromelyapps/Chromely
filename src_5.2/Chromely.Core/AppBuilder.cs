// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core
{
    public sealed class AppBuilder : AppBuilderBase
    {
        private ChromelyServiceProviderFactory _serviceProviderFactory;

        private AppBuilder(string[] args) 
            : base(args)
        {
        }

        public static AppBuilderBase Create(string[] args)
        {
            var appBuilder = new AppBuilder(args);
            return appBuilder;
        }

        public AppBuilderBase UseServices(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            return this;
        }

        public AppBuilderBase UseServiceProviderFactory(ChromelyServiceProviderFactory serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory;
            return this;
        }

        public override AppBuilderBase Build()
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
            _chromelyApp.RegisterChromelyControllerRoutes(_serviceProvider);

            _stepCompleted = 2;
            return this;
        }

        public override void Run()
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
                    windowController.Run(_args);
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
    }
}