using System;
using System.IO;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.BrowserWindow
{
    public abstract partial class HostBase : IChromelyWindow
    {
        protected readonly IChromelyContainer _container;
        protected readonly IChromelyConfiguration _config;
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;
        protected readonly IChromelyCommandTaskRunner _commandTaskRunner;

        protected IWindow _mainWindow;

        protected HostBase(IChromelyContainer container, IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner, IChromelyCommandTaskRunner commandTaskRunner)
        {
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
            _mainWindow?.Dispose();
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

            var mainArgs = new CefMainArgs(argv);
            CefApp app = new CefGlueApp(_config);

            // CEF applications have multiple sub-processes (render, plugin, GPU, etc)
            // that share the same executable. This function checks the command-line and,
            // if this is a sub-process, executes the appropriate logic.
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode >= 0)
            {
                // The sub-process has completed so return here.
                CefBinariesLoader.DeleteTempFiles(tempFiles);
                Logger.Instance.Log.Info($"Sub process executes successfully with code: {exitCode}");
                return exitCode;
            }

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            ScanAssemblies();
            RegisterRoutes();
            RegisterResourceHandlers();
            RegisterSchemeHandlers();
            RegisterMessageRouters();

            CreateMainWindow();
   
            bool centerScreen = _config.WindowCenterScreen;
            if (centerScreen)
            {
                _mainWindow.CenterToScreen();
            }

            CefBinariesLoader.DeleteTempFiles(tempFiles);

            RunMessageLoop();

            _mainWindow.Dispose();
            _mainWindow = null;

            CefRuntime.Shutdown();

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
