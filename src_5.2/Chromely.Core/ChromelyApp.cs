// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

public abstract class ChromelyApp
{
    protected bool _servicesConfigured;
    protected bool _coreServicesConfigured;
    protected bool _servicesInitialized;
    protected bool _resolversConfigured;
    protected bool _defaultHandlersConfigured;

    public virtual void ConfigureServices(IServiceCollection services)
    {
        _servicesConfigured = true;
    }

    public virtual void ConfigureCoreServices(IServiceCollection services)
    {
        if (!_servicesConfigured)
        {
            throw new Exception("Custom services must be configured before core default services are set.");
        }

        // Add core services if not already added.
        // Expected core services are -
        // IChromelyAppSettings, IChromelyConfiguration, IChromelyLogger, IChromelyRouteProvider, IChromelyErrorHandler
        // DefaultAppSettings  DefaultConfiguration, SimpleLogger, DefaultRouteProvider, DefaultErrorHandler
        // Logger is added in Initialize method

        services.TryAddSingleton<IChromelyConfiguration>(DefaultConfiguration.CreateForRuntimePlatform());
        services.TryAddSingleton<IChromelyAppSettings, DefaultAppSettings>();
        services.TryAddSingleton<IChromelyAppSettings, DefaultAppSettings>();
        services.TryAddSingleton<IChromelyErrorHandler, DefaultErrorHandler>();

        _coreServicesConfigured = true;
    }

    public virtual void ConfigureServiceResolvers(IServiceCollection services)
    {
        /*  Collection service resolvers for types: 
            IChromelyJsBindingHandler
            IChromelyCustomHandler
            IChromelyResourceHandlerFactory
            IChromelySchemeHandlerFactory
         */
        services.AddTransient<ChromelyHandlersResolver>(serviceProvider => (serviceType) =>
        {
            return serviceProvider.GetServices(serviceType);
        });

        _resolversConfigured = true;
    }

    public virtual void ConfigureDefaultHandlers(IServiceCollection services)
    {
        _defaultHandlersConfigured = true;
    }

    public virtual void Initialize(IServiceProvider serviceProvider)
    {
        if (!_servicesConfigured || !_coreServicesConfigured || !_resolversConfigured || !_defaultHandlersConfigured)
        {
            throw new Exception("Services must be configured before application is initialized.");
        }

        #region Configuration

        var config = serviceProvider.GetService<IChromelyConfiguration>();
        if (config is null)
        {
            config = DefaultConfiguration.CreateForRuntimePlatform();
        }

        ChromelyApp.InitConfiguration(config);

        #endregion Configuration

        #region Application/User Settings

        var appSettings = serviceProvider.GetService<IChromelyAppSettings>();
        if (appSettings is null)
        {
            appSettings = new DefaultAppSettings();
        }

        var currentAppSettings = new CurrentAppSettings
        {
            Properties = appSettings
        };

        ChromelyAppUser.App = currentAppSettings;
        ChromelyAppUser.App.Properties.Read(config);

        #endregion

        #region Logger

        var logger = GetCurrentLogger(serviceProvider);
        if (logger is null)
        {
            logger = new SimpleLogger();
        }

        var defaultLogger = new DefaultLogger
        {
            Log = logger
        };
        Logger.Instance = defaultLogger;

        #endregion

        EnsureExpectedWorkingDirectory();

        _servicesInitialized = true;
    }

    public virtual void RegisterChromelyControllerRoutes(IServiceProvider serviceProvider)
    {
        if (!_servicesInitialized)
        {
            throw new Exception("Services must be initialized before controller assemblies are scanned.");
        }

        var routeProvider = serviceProvider.GetService<IChromelyRouteProvider>();
        if (routeProvider is not null)
        {
            var controllers = serviceProvider.GetServices<ChromelyController>();
            routeProvider.RegisterAllRoutes(controllers?.ToList());
        }
    }

    public virtual void RegisterChromelyControllerAssembly(IServiceCollection services, string assemblyFullPath)
    {
        if (string.IsNullOrWhiteSpace(assemblyFullPath))
        {
            return;
        }

        try
        {
            if (File.Exists(assemblyFullPath))
            {
                var assembly = Assembly.LoadFrom(assemblyFullPath);
                RegisterChromelyControllerAssembly(services, assembly);
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception, "ChromelyApp:RegisterControllerAssembly");
        }

    }

    public virtual void RegisterChromelyControllerAssembly(IServiceCollection services, Assembly assembly)
    {
        if (assembly is null)
        {
            return;
        }

        try
        {
            services.RegisterChromelyControllerAssembly(assembly, ServiceLifetime.Singleton);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception, "ChromelyApp:RegisterControllerAssembly");
        }

    }

    protected virtual ILogger? GetCurrentLogger(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger>();
        if (logger is not null)
        {
            return logger;
        }

        var appName = Assembly.GetEntryAssembly()?.GetName().Name;
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        if (loggerFactory is not null)
        {
            return loggerFactory.CreateLogger(appName);
        }

        var loggerProvider = serviceProvider.GetService<ILoggerProvider>();
        if (loggerProvider is not null)
        {
            return loggerProvider.CreateLogger(appName);
        }

        return default;
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
            Logger.Instance.Log.LogError(exception, "ChromelyApp:EnsureExpectedWorkingDirectory");
        }
    }

    protected static void InitConfiguration(IChromelyConfiguration config)
    {
        if (config is null)
        {
            throw new Exception("Configuration cannot be null.");
        }

        if (config.UrlSchemes is null) config.UrlSchemes = new List<UrlScheme>();
        if (config.CommandLineArgs is null) config.CommandLineArgs = new Dictionary<string, string>();
        if (config.CommandLineOptions is null) config.CommandLineOptions = new List<string>();
        if (config.CustomSettings is null) config.CustomSettings = new Dictionary<string, string>();
        if (config.WindowOptions is null) config.WindowOptions = new Configuration.WindowOptions();
    }
}