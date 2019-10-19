using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Native;
using Xilium.CefGlue;

namespace Chromely.BrowserWindow
{
    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class CefGlueWindow : HostBase
    {
        private IWindow _mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefGlueWindow(ChromelyConfiguration hostConfig)
            : base(hostConfig)
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
            HostRuntime.LoadNativeHostFile(HostConfig);
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected override void RunMessageLoop()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                /* Run the GTK3 LINUX OR COCOA MACOS main loop */
                _mainWindow?.Run();
            }
            else
            {
                CefRuntime.RunMessageLoop();
            }
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void QuitMessageLoop()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                _mainWindow?.Exit();
            }
            else
            {
                CefRuntime.QuitMessageLoop();
            }
        }

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected override IWindow CreateMainView()
        {
            HostRuntime.EnsureNativeHostFileExists(HostConfig);

            _mainWindow = new Window(this, HostConfig);
            return _mainWindow;
        }
    }
}
