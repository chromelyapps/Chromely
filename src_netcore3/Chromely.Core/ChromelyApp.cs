using System;
using System.Collections.Generic;
using System.IO;
using Caliburn.Light;
using Chromely.Core.Defaults;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core
{
    public abstract class ChromelyApp
    {
        protected IChromelyContainer _container;

        public virtual IChromelyContainer Container 
        {
            get
            {
                EnsureContainerValid(_container);
                return _container;
            }
        }

        public virtual IChromelyConfiguration Configuration
        {
            get
            {
                EnsureContainerValid(Container);
                var config = Container.GetInstance(typeof(IChromelyConfiguration), typeof(IChromelyConfiguration).Name) as IChromelyConfiguration;
                return config; 
            }
        }

        public virtual IChromelyWindow Window
        {
            get
            {
                EnsureContainerValid(Container);
                var window = Container.GetInstance(typeof(IChromelyWindow), typeof(IChromelyWindow).Name) as IChromelyWindow;
                return window;
            }
        }

        public virtual void Initialize(IChromelyContainer container, IChromelyAppSettings appSettings, IChromelyConfiguration config, IChromelyLogger chromelyLogger)
        {
            EnsureExpectedWorkingDirectory();

            #region Container

            _container = container;
            if (_container == null)
            {
                _container = new SimpleContainer();
            }

            #endregion

            #region Configuration 

            if (config == null)
            {
                var configurator = new ConfigurationHandler();
                config = configurator.Parse<DefaultConfiguration>();
            }

            if (config == null)
            {
                config = DefaultConfiguration.CreateOSDefault(ChromelyRuntime.Platform);
            }

            InitConfiguration(config);
            config.Platform = ChromelyRuntime.Platform;

            #endregion

            #region Application/User Settings

            if (appSettings == null)
            {
                appSettings = new DefaultAppSettings();
            }

            var currentAppSettings = new CurrentAppSettings();
            currentAppSettings.Properties = appSettings;
            chromely.App = currentAppSettings;

            chromely.App.Properties.AppName = config.AppName;
            chromely.App.Properties.Read(config);
            chromely.App.Properties.Settings.Config = config.UrlSchemes;

            #endregion

            #region Logger

            if (chromelyLogger == null)
            {
                chromelyLogger = new SimpleLogger();
            }

            var defaultLogger = new DefaultLogger();
            defaultLogger.Log = chromelyLogger;
            Logger.Instance = defaultLogger;

            #endregion

            // Register all primary objects
            _container.RegisterInstance(typeof(IChromelyContainer), typeof(IChromelyContainer).Name, _container);
            _container.RegisterInstance(typeof(IChromelyAppSettings), typeof(IChromelyAppSettings).Name, appSettings);
            _container.RegisterInstance(typeof(IChromelyConfiguration), typeof(IChromelyConfiguration).Name, config);
            _container.RegisterInstance(typeof(IChromelyLogger), typeof(IChromelyLogger).Name, chromelyLogger);
        }

        public virtual void Configure(IChromelyContainer container)
        {
            EnsureContainerValid(container);

            container.RegisterSingleton(typeof(IChromelyRequestTaskRunner), typeof(IChromelyRequestTaskRunner).Name, typeof(DefaultRequestTaskRunner));
            container.RegisterSingleton(typeof(IChromelyCommandTaskRunner), typeof(IChromelyCommandTaskRunner).Name, typeof(DefaultCommandTaskRunner));
        }

        public abstract void RegisterEvents(IChromelyContainer container);

        public abstract IChromelyWindow CreateWindow();

        protected void InitConfiguration(IChromelyConfiguration config)
        {
            if (config == null)
            {
                throw new Exception("Configuration cannot be null.");
            }

            if (config.UrlSchemes == null) config.UrlSchemes = new List<UrlScheme>();
            if (config.ControllerAssemblies == null) config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
            if (config.CommandLineArgs == null) config.CommandLineArgs = new Dictionary<string, string>();
            if (config.CommandLineOptions == null) config.CommandLineOptions = new List<string>();
            if (config.CustomSettings == null) config.CustomSettings = new Dictionary<string, string>();
        }

        protected void EnsureContainerValid(IChromelyContainer container)
        {
            if (container == null)
            {
                throw new Exception("Container cannot be null. Initialize method must be called first.");
            }
        }

        /// <summary>
        /// Using local resource handling requires files to be relative to the 
        /// Expected working directory
        /// For example, if the app is launched via the taskbar the working directory gets changed to
        /// C:\Windows\system32
        /// This needs to be changed to the right one.
        /// </summary>
        protected static void EnsureExpectedWorkingDirectory()
        {
            try
            {
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Directory.SetCurrentDirectory(appDirectory);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }
    }
}
