using Chromely.Core.Infrastructure;
using System;
using System.Reflection;

namespace Chromely.Core
{
    public sealed class AppBuilder
    {
        private IChromelyContainer _container;
        private IChromelyConfiguration _config;
        private IChromelyLogger _logger;
        private ChromelyApp _chromelyApp;
        private int _stepCompleted;

        private AppBuilder()
        {
            _stepCompleted = 1;
        }

        public static AppBuilder Create()
        {
            var appBuilder = new AppBuilder();
            return appBuilder;
        }
       
        public AppBuilder UseContainer<T>(IChromelyContainer container = null) where T : IChromelyContainer
        {
            _container = container;
            if (_container == null)
            {
                _container = (T)Activator.CreateInstance(typeof(T));
            }

            return this;
        }

        public AppBuilder UseLogger<T>(IChromelyLogger logger = null) where T : IChromelyLogger
        {
            _logger = logger;
            if (_logger == null)
            {
                _logger = (T)Activator.CreateInstance(typeof(T));
            }

            return this;
        }

        public AppBuilder UseConfiguration<T>(IChromelyConfiguration config = null) where T : IChromelyConfiguration
        {
            _config = config;
            if (_config == null)
            {
                _config = (T)Activator.CreateInstance(typeof(T));
            }

            return this;
        }

        public AppBuilder UseApp<T>(ChromelyApp chromelyApp = null) where T : ChromelyApp
        {
            if (_stepCompleted != 1)
            {
                throw new Exception("Step 1 must be completed before step 2.");
            }

            if (typeof(T).FullName.Equals(typeof(ChromelyApp).FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Type {typeof(T).Name} must implement ChromelyApp.");
            }

            _chromelyApp = chromelyApp;
            if (_chromelyApp == null)
            {
                _chromelyApp = (T)Activator.CreateInstance(typeof(T));
            }

            _stepCompleted = 2;
            return this;
        }

        public AppBuilder Build()
        {
            if (_stepCompleted != 2)
            {
                throw new Exception("Step 2 must be completed before step 3.");
            }

            if (_chromelyApp == null)
            {
                throw new Exception($"ChromelyApp {nameof(_chromelyApp)} cannot be null.");
            }

            _chromelyApp.Initialize(_container, _config, _logger);
            _container = _chromelyApp.Container;
            _config = _chromelyApp.Configuration;
            _chromelyApp.Configure(_container);
            _chromelyApp.RegisterEvents(_container);

            _stepCompleted = 3;
            return this;
        }

        public void Run(string[] args)
        {
            if (_stepCompleted != 3)
            {
                throw new Exception("Step 1 must be completed before step 4.");
            }

            try
            {
                using (var window = _chromelyApp.CreateWindow())
                {
                    try
                    {
                        window.ScanAssemblies();
                        window.RegisterRoutes();
                        window.Run(args);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.Log.Error(exception);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }
    }
}
