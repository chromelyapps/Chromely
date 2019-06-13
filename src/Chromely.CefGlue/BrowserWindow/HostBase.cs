// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostBase.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.Browser.Handlers;
using Chromely.CefGlue.Subprocess;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.BrowserWindow
{
    /// <summary>
    /// The host base.
    /// </summary>
    public abstract class HostBase : IChromelyWindow
    {
        /// <summary>
        /// The main view.
        /// </summary>
        private IWindow _mainView;

        /// <summary>
        /// The window created.
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

        /// <summary>
        /// Gets the browser message router.
        /// </summary>
        public CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

        #region IChromelyWindow implementations

        /// <summary>
        /// Gets the host config.
        /// </summary>
        public ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public object Browser => _mainView?.Browser;

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
        /// The close - closing window externally/programatically.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// The Exit - closing window externally/programatically.
        /// </summary>
        public void Exit()
        {
            _mainView?.Exit();
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
            _mainView?.Dispose();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected abstract void RunMessageLoop();

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected abstract void QuitMessageLoop();

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="IWindow"/>.
        /// </returns>
        protected abstract IWindow CreateMainView();

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
            var tempFiles = CefBinariesLoader.Load(HostConfig);

            CefRuntime.EnableHighDpiSupport();

            var assembly = Assembly.GetEntryAssembly() ?? typeof(ChromelyConfiguration).Assembly;
            var settings = new CefSettings
            {
                MultiThreadedMessageLoop = true,
                LogSeverity = (CefLogSeverity)HostConfig.LogSeverity,
                LogFile = HostConfig.LogFile,
                ResourcesDirPath = Path.GetDirectoryName(new Uri(assembly.CodeBase).LocalPath)
            };

            if (HostConfig.HostFrameless)
            {
                if (HostConfig.HostApi == ChromelyHostApi.Gtk)
                {
                    throw new NotSupportedException("Chromely currently does not support frameless windows using GTK.");
                }

                // MultiThreadedMessageLoop is not allowed to be used as it will break frameless mode
                settings.MultiThreadedMessageLoop = false;
            }

            settings.LocalesDirPath = Path.Combine(settings.ResourcesDirPath, "locales");
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;
            settings.Locale = HostConfig.Locale;

            if (HostConfig.UseDefaultSubprocess)
            {
                var subprocessExeFullpath = DefaultSubprocessExe.FulPath;
                settings.BrowserSubprocessPath = subprocessExeFullpath ?? settings.BrowserSubprocessPath;
            }

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

            // CEF applications have multiple sub-processes (render, plugin, GPU, etc)
            // that share the same executable. This function checks the command-line and,
            // if this is a sub-process, executes the appropriate logic.
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode >= 0)
            {
                // The sub-process has completed so return here.
                CefBinariesLoader.DeleteTempFiles(tempFiles);
                Log.Info($"Sub process executes successfully with code: {exitCode}");
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

            Initialize();

            _mainView = CreateMainView();

            if (HostConfig.HostCenterScreen)
            {
                _mainView.CenterToScreen();
            }

            _windowCreated = true;

            CefBinariesLoader.DeleteTempFiles(tempFiles);

            RunMessageLoop();

            _mainView.Dispose();
            _mainView = null;

            CefRuntime.Shutdown();

            Shutdown();

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
            IoC.RegisterInstance(typeof(CefMessageRouterBrowserSide).FullName, BrowserMessageRouter);

            // Register message router handlers
            var messageRouterHandlers = IoC.GetAllInstances(typeof(ChromelyMessageRouter)).ToList();
            if (messageRouterHandlers.Any())
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
        /// The shutdown.
        /// </summary>
        private void Shutdown()
        {
            QuitMessageLoop();
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
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                Action();
                Action = null;
            }

            /// <summary>
            /// Gets or sets the action.
            /// </summary>
            private Action Action { get; set; }
        }
    }
}
