// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

// Shorthand for Owin pipeline func
using AppFunc = Func<IDictionary<string, object>, Task>;

public sealed class OwinAppBuilder : AppBuilderBase
{
    private static IWebHost? _owinHost;
    private IOwinPipeline? _owinPipeline;
    private IOwinAppStartup? _owinAppStartup;
    private TaskCompletionSource<AppFunc>? _tcsAppFunc;

    /// <summary>
    /// Initializes a new instance of <see cref="OwinAppBuilder"/>.
    /// </summary>
    private OwinAppBuilder(string[] args) 
        : base(args)
    {
    }

    /// <summary>
    /// Create the <see cref="OwinAppBuilder"/> instance.
    /// </summary>
    /// <returns>Instance of the <see cref="OwinAppBuilder"/>.</returns>
    public static OwinAppBuilder Create(string[] args)
    {
        var appBuilder = new OwinAppBuilder(args);
        return appBuilder;
    }

    /// <summary>
    /// Builds and create the services.
    /// </summary>
    /// <returns>Instance of the <see cref="OwinAppBuilder"/>.</returns>
    public override AppBuilderBase Build()
    {
        if (_stepCompleted != 1)
        {
            throw new Exception("Invalid order: Step 1: UseApp must be completed before Step 2: Build.");
        }

        if (_chromelyApp == null)
        {
            throw new Exception($"Application {nameof(ChromelyApp)} cannot be null.");
        }

        if (_chromelyApp.IsOwinApp())
        {
            BuildOwinInternal();
        }
        else
        {
            BuildInternal();
        }

        if (_serviceProvider == null)
        {
            throw new Exception($"The service provider is not created.");
        }

        _chromelyApp.Initialize(_serviceProvider);
        _chromelyApp.RegisterChromelyControllerRoutes(_serviceProvider);

        _stepCompleted = 2;
        return this;
    }

    /// <summary>
    /// Runs the application after build.
    /// </summary>
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

                if (_tcsAppFunc != null && _owinHost != null)
                {
                    Task.Run(async () => await _owinHost.RunAsync());
                    _owinPipeline = _serviceProvider.GetService<IOwinPipeline>() ?? new OwinPipeline();
                    _owinPipeline.AppFunc = _tcsAppFunc.Task.Result;
                    _owinPipeline.ErrorHandlingPath = (_chromelyApp as ChromelyOwinApp)?.ErrorHandlingPath;
                    _owinPipeline.ParseRoutes(_serviceProvider);
                }

                windowController?.Run(_args);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
            }
            finally
            {
                windowController?.Dispose();
                (_serviceProvider as ServiceProvider)?.Dispose();

                if (_owinHost != null)
                {
                    Task.Run(async () => await _owinHost.StopAsync());
                }
            }

        }
        catch (Exception exception)
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name;
            Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
        }
    }

    #region Build

    private void BuildOwinInternal()
    {
        _owinAppStartup  = _chromelyApp as IOwinAppStartup;

        if (_owinAppStartup == null)
        {
            throw new Exception($"Application {nameof(IOwinAppStartup)} cannot be null.");
        }

        _tcsAppFunc = new TaskCompletionSource<AppFunc>();

        var environment = string.IsNullOrWhiteSpace(_owinAppStartup.Environment) ? "Development" : _owinAppStartup.Environment;
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

        var builder = new WebHostBuilder();

        builder
         .ConfigureAppConfiguration((hostingContext, config) =>
         {
             _owinAppStartup.Configure(config);
             _owinAppStartup.Configuration = config.Build();
         })
        .ConfigureServices(services =>
        {
            var server = new OwinServer();
            server.UseOwin(appFunc =>
            {
                _tcsAppFunc.SetResult(appFunc);
            });

            services.AddSingleton<IOwinAppStartup>(_owinAppStartup);
            services.AddSingleton<IServer>(server);

            _chromelyApp?.ConfigureServices(services);

            // This must be done before registering core services
            RegisterUseComponents(services);

            _chromelyApp?.ConfigureCoreServices(services);
            _chromelyApp?.ConfigureServiceResolvers(services);
            _chromelyApp?.ConfigureDefaultHandlers(services);
        })
        .Configure(app =>
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            // Configure App, Env, LoggerFactory
            _owinAppStartup.Configure(app, env, loggerFactory);

        });

        // Configure builder
        builder.UseStartup<OwinStartup>()
               // https://github.com/aspnet/Hosting/issues/903
               // Ignore the startup class assembly as the "entry point" and instead point it to this app
               .UseSetting(WebHostDefaults.ApplicationKey, Assembly.GetEntryAssembly()?.FullName);

        _owinAppStartup.Configure(builder);

        _owinHost = builder.Build();

        // Configure host
        _owinAppStartup.Configure(_owinHost);

        _serviceProvider = _owinHost.Services;
    }

    public void BuildInternal()
    {
        _tcsAppFunc = null;

        if (_serviceCollection == null)
        {
            _serviceCollection = new ServiceCollection();
        }

        _chromelyApp?.ConfigureServices(_serviceCollection);

        // This must be done before registering core services
        RegisterUseComponents(_serviceCollection);

        _chromelyApp?.ConfigureCoreServices(_serviceCollection);
        _chromelyApp?.ConfigureServiceResolvers(_serviceCollection);
        _chromelyApp?.ConfigureDefaultHandlers(_serviceCollection);

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }

    #endregion
}