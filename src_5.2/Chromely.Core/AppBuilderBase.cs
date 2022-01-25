// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Base application builder class.
/// </summary>
public abstract class AppBuilderBase
{
    protected string[] _args;
    protected IServiceCollection? _serviceCollection;
    protected IServiceProvider? _serviceProvider;
    protected ChromelyApp? _chromelyApp;
    protected IChromelyConfiguration? _config;
    protected IChromelyWindow? _chromelyWindow;
    protected IChromelyErrorHandler? _chromelyErrorHandler;
    protected Type? _chromelyUseConfigType;
    protected Type? _chromelyUseWindowType;
    protected Type? _chromelyUseErrorHandlerType;
    protected int _stepCompleted;

    /// <summary>
    /// Initializes a new instance of <see cref="AppBuilderBase"/>.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    protected AppBuilderBase(string[] args)
    {
        _args = args;
        _config = null;
        _chromelyUseConfigType = null;
        _chromelyUseWindowType = null;
        _chromelyUseErrorHandlerType = null;
        _stepCompleted = -1;
    }

    /// <summary>
    /// Allows the developer to use custom configuration of derived type of <see cref="IChromelyConfiguration"/> 
    /// or an instance of derived type of <see cref="IChromelyConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromelyConfiguration" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromelyConfiguration" /> is not provided as a parameter, the of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">A derived type of <see cref="IChromelyConfiguration" /> definition.</typeparam>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseConfig<TService>(IChromelyConfiguration? config = null) where TService : IChromelyConfiguration
    {
        if (config is not null)
        {
            _config = config;
        }
        else
        {
            _chromelyUseConfigType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromelyConfiguration>();
            _chromelyUseConfigType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom window of derived type of <see cref="IChromelyWindow"/> 
    /// or an instance of derived type of <see cref="IChromelyWindow"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromelyWindow" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromelyWindow" /> is not provided as a parameter, the of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">Type of <see cref="IChromelyWindow"/>.</typeparam>
    /// <param name="chromelyWindow">The <see cref="IChromelyWindow" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseWindow<TService>(IChromelyWindow? chromelyWindow = null) where TService : IChromelyWindow
    {
        if (chromelyWindow is not null)
        {
            _chromelyWindow = chromelyWindow;
        }
        else
        {
            _chromelyUseWindowType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromelyWindow>();
            _chromelyUseWindowType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom error handler of derived type of <see cref="IChromelyErrorHandler"/> 
    /// or an instance of derived type of <see cref="IChromelyErrorHandler"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromelyErrorHandler" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromelyErrorHandler" /> is not provided as a parameter, the of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">Type of <see cref="IChromelyErrorHandler"/>.</typeparam>
    /// <param name="chromelyErrorHandler">The <see cref="IChromelyErrorHandler" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseErrorHandler<TService>(IChromelyErrorHandler? chromelyErrorHandler = null) where TService : IChromelyErrorHandler
    {
        if (chromelyErrorHandler is not null)
        {
            _chromelyErrorHandler = chromelyErrorHandler;
        }
        else
        {
            _chromelyUseErrorHandlerType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromelyErrorHandler>();
            _chromelyUseErrorHandlerType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom application class of derived type of <see cref="ChromelyApp"/> 
    /// or an instance of derived type of <see cref="ChromelyApp"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="ChromelyApp" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="ChromelyApp" /> is not provided as a parameter, the of TApp is used to create one.
    /// </remarks>
    /// <typeparam name="TApp">Type of <see cref="IChromelyErrorHandler"/>.</typeparam>
    /// <param name="chromelyApp">The <see cref="ChromelyApp" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseApp<TApp>(ChromelyApp? chromelyApp = null) where TApp : ChromelyApp
    {
        _chromelyApp = chromelyApp;
        if (_chromelyApp is null)
        {
            typeof(TApp).EnsureIsDerivedFromType<ChromelyApp>();
            _chromelyApp = Activator.CreateInstance(typeof(TApp)) as TApp;
        }

        _stepCompleted = 1;
        return this;
    }

    /// <summary>
    /// Builds the application based on configured default and custom conifigured services.
    /// </summary>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public abstract AppBuilderBase Build();

    /// <summary>
    /// Runs the application.
    /// </summary>
    public abstract void Run();

    /// <summary>
    /// Register all the custom configured services.
    /// </summary>
    /// <param name="services"></param>
    protected void RegisterUseComponents(IServiceCollection services)
    {
        #region IChromelyConfiguration

        if (_config is not null)
        {
            services.TryAddSingleton<IChromelyConfiguration>(_config);
        }
        else if (_chromelyUseConfigType is not null)
        {
            services.TryAddSingleton(typeof(IChromelyConfiguration), _chromelyUseConfigType);
        }

        #endregion IChromelyConfiguration

        #region IChromelyWindow

        if (_chromelyWindow is not null)
        {
            services.TryAddSingleton<IChromelyWindow>(_chromelyWindow);
        }
        else if (_chromelyUseWindowType is not null)
        {
            services.TryAddSingleton(typeof(IChromelyWindow), _chromelyUseWindowType);
        }

        #endregion IChromelyWindow

        #region IChromelyErrorHandler

        if (_chromelyErrorHandler is not null)
        {
            services.TryAddSingleton<IChromelyErrorHandler>(_chromelyErrorHandler);
        }
        else if (_chromelyUseErrorHandlerType is not null)
        {
            services.TryAddSingleton(typeof(IChromelyErrorHandler), _chromelyUseErrorHandlerType);
        }

        #endregion IChromelyErrorHandler
    }
}