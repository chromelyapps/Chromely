namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using System;
    using Xilium.CefGlue.Wrapper;
    using Xilium.CefGlue;
    using Chromely.CefGlue.Gtk.Browser;

    public abstract class HostBase : IDisposable
    {
        public static CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

        private Window _mainView;

        protected HostBase()
        {
        }

        #region IDisposable

        ~HostBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion

        public string Name { get { return "Xilium CefGlue Demo"; } }
        public int DefaultWidth { get { return 800; } }
        public int DefaultHeight { get { return 600; } }
        public string HomeUrl { get { return "http://google.com"; } }

        protected Window MainView { get { return _mainView; } }

        public int Run(string[] args)
        {
            try
            {
                return RunInternal(args);
            }
            catch (Exception ex)
            {
                // Log(ex.ToString());
                return 1;
            }
        }

        protected bool MultiThreadedMessageLoop { get; private set; }

        private int RunInternal(string[] args)
        {
            CefRuntime.Load();

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.SingleProcess = false;
            settings.LogSeverity = CefLogSeverity.Verbose;
            settings.LogFile = "cef.log";
            settings.ResourcesDirPath = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;

            var argv = args;
            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
            {
                argv = new string[args.Length + 1];
                Array.Copy(args, 0, argv, 1, args.Length);
                argv[0] = "-";
            }

            var mainArgs = new CefMainArgs(argv);
            var app = new CefWebApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
            if (exitCode != -1)
                return exitCode;

            // guard if something wrong
            foreach (var arg in args) { if (arg.StartsWith("--type=")) { return -2; } }

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            //RegisterSchemes();
            //RegisterMessageRouter();

            PlatformInitialize();

            _mainView = CreateMainView();

            PlatformRunMessageLoop();

            _mainView.Dispose();
            _mainView = null;

            CefRuntime.Shutdown();

            PlatformShutdown();
            return 0;
        }

        public void Quit()
        {
            PlatformQuitMessageLoop();
        }

        protected abstract void PlatformInitialize();

        protected abstract void PlatformShutdown();

        protected abstract void PlatformRunMessageLoop();

        protected abstract void PlatformQuitMessageLoop();

        protected abstract Window CreateMainView();

        private void RegisterSchemes()
        {
            // register custom scheme handler
          //  CefRuntime.RegisterSchemeHandlerFactory("http", DumpRequestDomain, new DemoAppSchemeHandlerFactory());
            // CefRuntime.AddCrossOriginWhitelistEntry("http://localhost", "http", "", true);
        }

        private void RegisterMessageRouter()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, this.RegisterMessageRouter);
                return;
            }

            // window.cefQuery({ request: 'my_request', onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } });
            //  DemoApp.BrowserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());
            //   DemoApp.BrowserMessageRouter.AddHandler(new DemoMessageRouterHandler());
        }

        public static void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

        internal sealed class ActionTask : CefTask
        {
            public Action _action;

            public ActionTask(Action action)
            {
                _action = action;
            }

            protected override void Execute()
            {
                _action();
                _action = null;
            }
        }

        public delegate void Action();
    }
}
