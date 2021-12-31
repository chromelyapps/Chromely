// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chromely.Core
{
    public abstract class AppBuilderBase
    {
        protected string[] _args;
        protected IServiceCollection _serviceCollection;
        protected IServiceProvider _serviceProvider;
        protected ChromelyApp _chromelyApp;
        protected IChromelyConfiguration _config;
        protected IChromelyWindow _chromelyWindow;
        protected IChromelyErrorHandler _chromelyErrorHandler;
        protected Type _chromelyUseConfigType;
        protected Type _chromelyUseWindowType;
        protected Type _chromelyUseErrorHandlerType;
        protected int _stepCompleted;

        protected AppBuilderBase(string[] args)
        {
            _args = args;
            _config = null;
            _chromelyUseConfigType = null;
            _chromelyUseWindowType = null;
            _chromelyUseErrorHandlerType = null;
            _stepCompleted = -1;
        }

        public virtual AppBuilderBase UseConfig<TService>(IChromelyConfiguration config = null) where TService : IChromelyConfiguration
        {
            if (config != null)
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

        public virtual AppBuilderBase UseWindow<TService>(IChromelyWindow chromelyWindow = null) where TService : IChromelyWindow
        {
            if (chromelyWindow != null)
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

        public virtual AppBuilderBase UseErrorHandler<TService>(IChromelyErrorHandler chromelyErrorHandler = null) where TService : IChromelyErrorHandler
        {
            if (chromelyErrorHandler != null)
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

        public virtual AppBuilderBase UseApp<TApp>(ChromelyApp chromelyApp = null) where TApp : ChromelyApp
        {
            _chromelyApp = chromelyApp;
            if (_chromelyApp == null)
            {
                typeof(TApp).EnsureIsDerivedFromType<ChromelyApp>();
                _chromelyApp = (TApp)Activator.CreateInstance(typeof(TApp));
            }

            _stepCompleted = 1;
            return this;
        }

        public abstract AppBuilderBase Build();

        public abstract void Run();

        protected void RegisterUseComponents(IServiceCollection services)
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