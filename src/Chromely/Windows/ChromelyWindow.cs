using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Network;
using Chromely.Native;
using Xilium.CefGlue;

namespace Chromely.Windows
{
    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class ChromelyWindow : HostBase
    {
        public ChromelyWindow(IChromelyNativeHost nativeHost, IChromelyContainer container, IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner, IChromelyCommandTaskRunner commandTaskRunner)
            : base(nativeHost, container, config, requestTaskRunner, commandTaskRunner)
        {
        }

        /// <summary>
        /// The close.
        /// </summary>
        public new void Close()
        {
            _mainWindow?.Exit();
        }

        /// <summary>
        /// The exit.
        /// </summary>
        public new void Exit()
        {
            _mainWindow?.Exit();
        }

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected override void Initialize()
        {
            HostRuntime.LoadNativeHostFile(_config);
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void Run()
        {
            _mainWindow?.Run();
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void ExitWindow()
        {
            _mainWindow?.Exit();
        }

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected override IWindow CreateMainView()
        {
            HostRuntime.EnsureNativeHostFileExists(_config);

            if (_mainWindow == null)
            {
                _mainWindow = new Window(_nativeHost, _container, _config, _commandTaskRunner, BrowserMessageRouter);
            }

            return _mainWindow;
        }
    }
}
