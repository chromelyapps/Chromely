namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using Chromely.CefGlue.Gtk.Browser;
    using System;
    using Xilium.CefGlue;

    public sealed class Window : NativeWindow
    {
        private CefWebBrowser _core;
        private IntPtr _browserWindowHandle;

        private readonly HostBase _application;
        private readonly string _applicationTitle;

        private int _x;
        private int _y;
        private int _width;
        private int _height;

        private bool _created;

        public Window(HostBase application, string title, int width, int height, string iconFile = null)
            : base(title, width, height, iconFile)
        {
            _core = new CefWebBrowser(this, new CefBrowserSettings(), "www.google.com");
            _core.Created += new EventHandler(BrowserCreated);

            _application = application;
            _applicationTitle = title;

            ShowWindow();
        }

        public void Close()
        {
            Console.WriteLine("Close()");
            Dispose();
        }

        public string StartUrl
        {
            get { return _core.StartUrl; }
            set { _core.StartUrl = value; }
        }

        public CefWebBrowser WebBrowser { get { return _core; } }

        public CefBrowser CurrentBrowser => throw new NotImplementedException();

        protected override void OnRealized(object sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    var parentHandle = HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, 1200, 800)); // TODO: set correct  x, y, width, height  to do not waiting OnSizeAllocated event
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(HostXid, new CefRectangle(0, 0, 0, 0));
                    break;

                case CefRuntimePlatform.MacOSX:
                default:
                    throw new NotSupportedException();
            }

            _core.Create(windowInfo);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _application.Quit();
        }

        private void BrowserCreated(object sender, EventArgs e)
        {
            _browserWindowHandle = _core.CefBrowser.GetHost().GetWindowHandle();
            _created = true;
        }

        public void Dispose()
        {
            if (_core != null)
            {
                var browser = _core.CefBrowser;
                var host = browser.GetHost();
                host.CloseBrowser();
                host.Dispose();
                browser.Dispose();
                browser = null;
                _browserWindowHandle = IntPtr.Zero;
            }
        }
    }
}
