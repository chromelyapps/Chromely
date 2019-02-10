// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostBase.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.BrowserWindow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefGlue.Gtk.Browser;
    using Chromely.CefGlue.Gtk.Browser.Handlers;
    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    /// <summary>
    /// The host base.
    /// </summary>
    public abstract class HostBase : IChromelyWindow, IChromelyServiceProvider, IDisposable
    {
        /// <summary>
        /// The m main view.
        /// </summary>
        private Window mMainView;

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

        /// <summary>
        /// Gets the browser message router.
        /// </summary>
        public static CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

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
        /// Runs the application.
        /// This call does not return until the application terminates
        /// or an error is occured.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        ///  0 successfully run application - now terminated
        ///  1 on internal exception (see log for more information).
        /// -2 if something wrong !? TODO: @mattkol clarify this result code 
        ///  n CefRuntime.ExecuteProcess result 
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

        #endregion

        #region Quit/IDisposable

        /// <summary>
        /// The quit.
        /// </summary>
        public virtual void Quit()
        {
            PlatformQuitMessageLoop();
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
                    RouteScanner scanner = new RouteScanner(assembly.Assembly);
                    Dictionary<string, Route> currentRouteDictionary = scanner.Scan();
                    ServiceRouteProvider.MergeRoutes(currentRouteDictionary);

                    assembly.IsScanned = true;
                }
            }
        }

        #region Abstract Methods

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected abstract void PlatformInitialize();

        /// <summary>
        /// The platform shutdown.
        /// </summary>
        protected abstract void PlatformShutdown();

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected abstract void PlatformRunMessageLoop();

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected abstract void PlatformQuitMessageLoop();

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected abstract Window CreateMainView();

        #endregion Abstract Methods

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        private static void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

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
            CefRuntime.Load();

            var settings = new CefSettings
            {
                MultiThreadedMessageLoop = true,
                LogSeverity = (CefLogSeverity)HostConfig.LogSeverity,
                LogFile = HostConfig.LogFile,
                ResourcesDirPath = Path.GetDirectoryName(
                    new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
            };

            settings.LocalesDirPath = Path.Combine(settings.ResourcesDirPath, "locales");
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;
            settings.Locale = HostConfig.Locale;

            var argv = args;
            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
            {
                argv = new string[args.Length + 1];
                Array.Copy(args, 0, argv, 1, args.Length);
                argv[0] = "-";
            }

            // Update configuration settings
            settings.Update(HostConfig.CustomSettings);

            var mainArgs = new CefMainArgs(argv);
            var app = new CefGlueApp(HostConfig);

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            Log.Info($"CefRuntime.ExecuteProcess() returns {exitCode}");

            if (exitCode != -1)
            {
                // An error has occured.
                return exitCode;
            }

            // guard if something wrong
            foreach (var arg in args)
            {
                if (arg.StartsWith("--type="))
                {
                    return -2;
                }
            }

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            RegisterSchemeHandlers();
            RegisterMessageRouters();

            PlatformInitialize();

            mMainView = CreateMainView();

            mWindowCreated = true;

            PlatformRunMessageLoop();

            mMainView.Dispose();
            mMainView = null;

            CefRuntime.Shutdown();

            PlatformShutdown();

            return 0;
        }

        /// <summary>
        /// The register scheme handlers.
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
                                CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, new CefGlueResourceSchemeHandlerFactory());
                            }

                            if (handler.UseDefaultHttp)
                            {
                                CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, new CefGlueHttpSchemeHandlerFactory());
                            }
                        }
                        else if (handler.HandlerFactory is CefSchemeHandlerFactory)
                        {
                            CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, (CefSchemeHandlerFactory)handler.HandlerFactory);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The register message routers.
        /// </summary>
        private void RegisterMessageRouters()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, RegisterMessageRouters);
                return;
            }

            BrowserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());

            // Register message router handlers
            List<object> messageRouterHandlers = IoC.GetAllInstances(typeof(ChromelyMessageRouter)).ToList();
            if ((messageRouterHandlers != null) && (messageRouterHandlers.Count > 0))
            {
                var routerHandlers = messageRouterHandlers.ToList();

                foreach (var item in routerHandlers)
                {
                    ChromelyMessageRouter routerHandler = (ChromelyMessageRouter)item;
                    if (routerHandler.Handler is CefMessageRouterBrowserSide.Handler)
                    {
                        BrowserMessageRouter.AddHandler((CefMessageRouterBrowserSide.Handler)routerHandler.Handler);
                    }
                }
            }
            else
            {
                BrowserMessageRouter.AddHandler(new CefGlueMessageRouterHandler());
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private sealed class ActionTask : CefTask
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            public ActionTask(Action action)
            {
                Action = action;
            }

            /// <summary>
            /// Gets or sets the action.
            /// </summary>
            public Action Action { get; set; }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                Action();
                Action = null;
            }
        }
    }
}
