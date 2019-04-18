// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostBase.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    using CefSharpGlobal = global::CefSharp;

    /// <summary>
    /// The host base.
    /// </summary>
    internal abstract class HostBase : IChromelyWindow
    {
        /// <summary>
        /// The m main view.
        /// </summary>
        private Window mMainView;

        /// <summary>
        /// The CefSettings object.
        /// </summary>
        private CefSettings mSettings;

        /// <summary>
        /// The Wwindow created.
        /// </summary>
        private bool mWindowCreated;

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
        /// Gets the browser.
        /// </summary>
        public object Browser => mMainView?.Browser;

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
            if (mWindowCreated)
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
            if (mWindowCreated)
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
            if (mWindowCreated)
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
            mMainView?.CloseWindowExternally();
        }

        /// <summary>
        /// The Exit.
        /// </summary>
        public void Exit()
        {
            mMainView?.CloseWindowExternally();
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
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        public void RegisterServiceAssemblies(List<string> filenames)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssemblies(filenames);
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
                    var currentRouteDictionary = scanner.Scan();
                    ServiceRouteProvider.MergeRoutes(currentRouteDictionary);

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
            mMainView?.Dispose();
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
            CefSharpGlobal.Cef.EnableHighDPISupport();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            var localesDirPath = Path.Combine(localFolder ?? throw new InvalidOperationException(), "locales");

            mSettings = new CefSettings
                            {
                                LocalesDirPath = localesDirPath,
                                Locale = HostConfig.Locale,
                                MultiThreadedMessageLoop = true,
                                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                                LogSeverity = (CefSharpGlobal.LogSeverity)HostConfig.LogSeverity,
                                LogFile = HostConfig.LogFile
                            };

            // Update configuration settings
            mSettings.Update(HostConfig.CustomSettings);
            mSettings.UpdateCommandLineArgs(HostConfig.CommandLineArgs);

            RegisterSchemeHandlers();

            // Perform dependency check to make sure all relevant resources are in our output directory.
            CefSharpGlobal.Cef.Initialize(mSettings, performDependencyCheck: true, browserProcessHandler: null);
       
            Initialize();

            mMainView = CreateMainView(mSettings);

            if (HostConfig.HostCenterScreen)
            {
                mMainView.CenterToScreen();
            }

            mWindowCreated = true;

            RunMessageLoop();

            mMainView.Dispose();
            mMainView = null;

            CefSharpGlobal.Cef.Shutdown();

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
                                mSettings.RegisterScheme(new CefSharpGlobal.CefCustomScheme
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
                                mSettings.RegisterScheme(new CefSharpGlobal.CefCustomScheme
                                {
                                    SchemeName = handler.SchemeName,
                                    DomainName = handler.DomainName,
                                    IsSecure = handler.IsSecure,
                                    IsCorsEnabled = handler.IsCorsEnabled,
                                    SchemeHandlerFactory = new CefSharpHttpSchemeHandlerFactory()
                                });
                            }
                        }
                        else if (handler.HandlerFactory is CefSharpGlobal.ISchemeHandlerFactory)
                        {
                            mSettings.RegisterScheme(new CefSharpGlobal.CefCustomScheme
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
