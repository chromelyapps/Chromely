using System;
using System.IO;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.BrowserWindow
{
    public abstract partial class HostBase : IChromelyWindow
    {
        protected readonly IChromelyNativeHost _nativeHost;
        protected readonly IChromelyContainer _container;
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;
        protected readonly IChromelyCommandTaskRunner _commandTaskRunner;

        protected IWindow _mainWindow;

        protected HostBase(IChromelyNativeHost nativeHost, IChromelyContainer container, IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner, IChromelyCommandTaskRunner commandTaskRunner)
        {
            _nativeHost = nativeHost;
            _container = container;
            _config = config;
            _requestTaskRunner = requestTaskRunner;
            _commandTaskRunner = commandTaskRunner;
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

        public CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

        #region IChromelyWindow implementations

        public IChromelyConfiguration Config => _config;

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (_mainWindow != null)
                {
                    return _mainWindow.HostHandle;
                }

                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public object Browser => _mainWindow?.Browser;

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
                Logger.Instance.Log.Error(exception);
                return 1;
            }
        }

        /// <summary>
        /// The close - closing window externally/programatically.
        /// </summary>
        public void Close()
        {
            _mainWindow?.Exit();
        }

        /// <summary>
        /// The Exit - closing window externally/programatically.
        /// </summary>
        public void Exit()
        {
            _mainWindow?.Exit();
        }

        #endregion

        #region Quit/IDisposable

        /// <summary>
        /// The quit.
        /// </summary>
        public virtual void Quit()
        {
            _mainWindow?.Browser?.OnBeforeClose(new BeforeCloseEventArgs());
            ExitWindow();
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
            _mainWindow?.Dispose();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected abstract void ExitWindow();

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="IWindow"/>.
        /// </returns>
        protected abstract IWindow CreateMainView();

        #endregion Abstract Methods

        private static void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

        private int RunInternal(string[] args)
        {
            Initialize();

            _config.ChromelyVersion = CefRuntime.ChromeVersion;

            var tempFiles = CefBinariesLoader.Load(_config);

            CefRuntime.EnableHighDpiSupport();

            var settings = new CefSettings
            {
                MultiThreadedMessageLoop = _config.Platform == ChromelyPlatform.Windows,
                LogSeverity = CefLogSeverity.Info,
                LogFile = "logs\\chromely.cef_" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                ResourcesDirPath = _config.AppExeLocation
            };

            if (_config.WindowFrameless || _config.WindowKioskMode)
            {
                // MultiThreadedMessageLoop is not allowed to be used as it will break frameless mode
                settings.MultiThreadedMessageLoop = false;
            }

            settings.LocalesDirPath = Path.Combine(settings.ResourcesDirPath, "locales");
            settings.RemoteDebuggingPort = 20480;
            settings.Locale = "en-US";
            settings.NoSandbox = true;

            var argv = args;
            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
            {
                argv = new string[args.Length + 1];
                Array.Copy(args, 0, argv, 1, args.Length);
                argv[0] = "-";
            }

            // Update configuration settings
            settings.Update(_config.CustomSettings);

            // Set DevTools url
            _config.DevToolsUrl = $"http://127.0.0.1:{settings.RemoteDebuggingPort}";

            var mainArgs = new CefMainArgs(argv);
            CefApp app = new CefGlueApp(_config);

            if (ClientAppUtils.ExecuteProcess(_config.Platform, argv))
            {
                // CEF applications have multiple sub-processes (render, plugin, GPU, etc)
                // that share the same executable. This function checks the command-line and,
                // if this is a sub-process, executes the appropriate logic.
                var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
                if (exitCode >= 0)
                {
                    // The sub-process has completed so return here.
                    Logger.Instance.Log.Info($"Sub process executes successfully with code: {exitCode}");
                    return exitCode;
                }
            }

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            ScanAssemblies();
            RegisterRoutes();
            RegisterMessageRouters();
            RegisterResourceHandlers();
            RegisterSchemeHandlers();

            CefBinariesLoader.DeleteTempFiles(tempFiles);

            CreateMainWindow();

            Run();

            _mainWindow.Dispose();
            _mainWindow = null;

            Shutdown();

            return 0;
        }

        private void CreateMainWindow()
        {
            if (_config.Platform != ChromelyPlatform.Windows)
            {
                if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
                {
                    PostTask(CefThreadId.UI, CreateMainWindow);
                    return;
                }
            }

            _mainWindow = CreateMainView();
        }

        /// <summary>
        /// The shutdown.
        /// </summary>
        private void Shutdown()
        {
            ExitWindow();
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
