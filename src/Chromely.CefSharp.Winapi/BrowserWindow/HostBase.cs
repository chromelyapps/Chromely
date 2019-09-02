// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostBase.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CefSharp;
using Chromely.CefSharp.Winapi.Browser;
using Chromely.CefSharp.Winapi.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

using CefSharpGlobal = global::CefSharp;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    /// <summary>
    /// The host base.
    /// </summary>
    internal abstract class HostBase : IChromelyWindow
    {
        /// <summary>
        /// The main view.
        /// </summary>
        private Window _mainView;

        /// <summary>
        /// The CefSettings object.
        /// </summary>
        private CefSettings _settings;

        /// <summary>
        /// The Wwindow created.
        /// </summary>
        private bool _windowCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBase"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        protected HostBase(ChromelyConfiguration hostConfig)
        {
            HostConfig = hostConfig;
        }

        #region Destructor

        /// <summary>
        /// Finalizes an instance of the <see cref="HostBase"/> class. 
        /// </summary>
        ~HostBase()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region IChromelyWindow implementations

        /// <summary>
        /// Gets the host config.
        /// </summary>
        public ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (_mainView != null)
                {
                    return _mainView.Handle;
                }

                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public object Browser => _mainView?.Browser;

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int Run(string[] args)
        {
            try
            {
                return RunInternal(args);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return 1;
            }
        }

        /// <summary>
        /// The register event handler.
        /// The event handler must be registered before calling "Run".
        /// Alternatively this can be done before window is created during ChromelyConfiguration instantiation.
        /// Only one type of event handler can be registered. The first one is valid, consequent registrations will be ignored.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// This is the event argument classe - e,g - LoadErrorEventArgs, FrameLoadStartEventArgs. 
        /// </typeparam>
        public void RegisterEventHandler<T>(CefEventKey key, EventHandler<T> handler)
        {
            if (_windowCreated)
            {
                throw new Exception("\"RegisterEventHandler\" method must be called before \"Run\" method.");
            }

            HostConfig?.RegisterEventHandler(key, handler);
        }

        /// <summary>
        /// The register event handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// This is the event argument classe - e,g - LoadErrorEventArgs, FrameLoadStartEventArgs. 
        /// </typeparam>
        public void RegisterEventHandler<T>(CefEventKey key, ChromelyEventHandler<T> handler)
        {
            if (_windowCreated)
            {
                throw new Exception("\"RegisterEventHandler\" method must be called before \"Run\" method.");
            }

            HostConfig?.RegisterEventHandler(key, handler);
        }

        /// <summary>
        /// The register custom handler. 
        /// The custom handler must be registered before calling "Run".
        /// Alternatively this can be done before window is created during ChromelyConfiguration instantiation.
        /// Only one type of custom handler can be registered. The first one is valid, consequent registrations will be ignored.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="implementation">
        /// The implementation.
        /// </param>
        public void RegisterCustomHandler(CefHandlerKey key, Type implementation)
        {
            if (_windowCreated)
            {
                throw new Exception("\"RegisterCustomHandler\" method must be called before \"Run\" method.");
            }

            HostConfig?.RegisterCustomHandler(key, implementation);
        }

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            _mainView?.CloseWindowExternally();
        }

        /// <summary>
        /// The Exit.
        /// </summary>
        public void Exit()
        {
            _mainView?.CloseWindowExternally();
        }

        #endregion

        #region IChromelyServiceProvider implementations

        /// <summary>
        /// The register url scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        public void RegisterUrlScheme(UrlScheme scheme)
        {
            UrlSchemeProvider.RegisterScheme(scheme);
        }

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void RegisterServiceAssembly(string filename)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssembly(filename);
        }

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public void RegisterServiceAssembly(Assembly assembly)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssembly(assembly);
        }

        /// <summary>
        /// The register service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        public void RegisterServiceAssemblies(string folder)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssemblies(folder);
        }

        /// <summary>
        /// The register service assemblies.
        /// </summary>
        /// <param name="fileNames">
        /// The filenames.
        /// </param>
        public void RegisterServiceAssemblies(List<string> fileNames)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssemblies(fileNames);
        }

        /// <summary>
        /// The scan assemblies.
        /// </summary>
        public void ScanAssemblies()
        {
            if ((HostConfig?.ServiceAssemblies == null) || (HostConfig?.ServiceAssemblies.Count == 0))
            {
                return;
            }

            foreach (var assembly in HostConfig?.ServiceAssemblies)
            {
                if (!assembly.IsScanned)
                {
                    var scanner = new RouteScanner(assembly.Assembly);
                    var currentRouteDictionary = scanner.Scan().Item1;
                    var currentCommandDictionary = scanner.Scan().Item2;
                    ServiceRouteProvider.MergeRoutes(currentRouteDictionary);
                    ServiceRouteProvider.MergeCommands(currentCommandDictionary);

                    assembly.IsScanned = true;
                }
            }
        }

        #endregion

        #region Quit/IDisposable

        /// <summary>
        /// The quit.
        /// </summary>
        public virtual void Quit()
        {
            QuitMessageLoop();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        public virtual void Dispose(bool disposing)
        {
            _mainView?.Dispose();
        }

        #endregion

        /// <summary>
        /// The run internal.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int RunInternal(string[] args)
        {
            // For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            var localesDirPath = Path.Combine(localFolder ?? throw new InvalidOperationException(), "locales");

            _settings = new CefSettings
            {
                LocalesDirPath = localesDirPath,
                Locale = HostConfig.Locale,

               // MultiThreadedMessageLoop = true,
                // MultiThreadedMessageLoop is not allowed to be used as it will break frameless mode
                MultiThreadedMessageLoop = !(HostConfig.HostPlacement.Frameless || HostConfig.HostPlacement.KioskMode),

                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = (CefSharpGlobal.LogSeverity)HostConfig.LogSeverity,
                LogFile = HostConfig.LogFile
            };

            // Update configuration settings
            _settings.Update(HostConfig.CustomSettings);
            _settings.UpdateCommandLineArgs(HostConfig.CommandLineArgs);

            RegisterSchemeHandlers();

            // Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(_settings, true, browserProcessHandler: null);
       
            Initialize();

            _mainView = CreateMainView(_settings);

            bool centerScreen = HostConfig.HostPlacement.CenterScreen;
            if (centerScreen)
            {
                _mainView.CenterToScreen();
            }

            _windowCreated = true;

            IoC.RegisterInstance(typeof(IChromelyWindow), typeof(IChromelyWindow).FullName, this);

            RunMessageLoop();

            _mainView.Dispose();
            _mainView = null;

            Cef.Shutdown();

            Shutdown();

            return 0;
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        private void RunMessageLoop()
        {
            NativeWindow.RunMessageLoop();
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        private void QuitMessageLoop()
        {
            NativeWindow.Exit();
        }

        /// <summary>
        /// The shutdown.
        /// </summary>
        private void Shutdown()
        {
            QuitMessageLoop();
        }

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        private Window CreateMainView(CefSettings settings)
        {
            return new Window(this, HostConfig, settings);
        }

        /// <summary>
        /// Registers custom scheme handlers.
        /// </summary>
        private void RegisterSchemeHandlers()
        {
            // Register scheme handlers
            var schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();

                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler handler)
                    {
                        if (handler.HandlerFactory == null)
                        {
                            if (handler.UseDefaultResource)
                            {
                                _settings.RegisterScheme(new CefSharpGlobal.CefCustomScheme
                                {
                                    SchemeName = handler.SchemeName,
                                    DomainName = handler.DomainName,
                                    IsSecure = handler.IsSecure,
                                    IsCorsEnabled = handler.IsCorsEnabled,
                                    SchemeHandlerFactory = new CefSharpResourceSchemeHandlerFactory()
                                });
                            }

                            if (handler.UseDefaultHttp)
                            {
                                _settings.RegisterScheme(new CefCustomScheme
                                {
                                    SchemeName = handler.SchemeName,
                                    DomainName = handler.DomainName,
                                    IsSecure = handler.IsSecure,
                                    IsCorsEnabled = handler.IsCorsEnabled,
                                    SchemeHandlerFactory = new CefSharpHttpSchemeHandlerFactory()
                                });
                            }
                        }
                        else if (handler.HandlerFactory is ISchemeHandlerFactory)
                        {
                            _settings.RegisterScheme(new CefCustomScheme
                            {
                                SchemeName = handler.SchemeName,
                                DomainName = handler.DomainName,
                                IsSecure = handler.IsSecure,
                                IsCorsEnabled = handler.IsCorsEnabled,
                                SchemeHandlerFactory = (CefSharpGlobal.ISchemeHandlerFactory)handler.HandlerFactory
                            });
                        }
                    }
                }
            }
        }
    }
}
