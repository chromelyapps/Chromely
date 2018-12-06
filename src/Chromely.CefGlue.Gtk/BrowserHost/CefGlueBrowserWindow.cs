// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.BrowserHost
{
    using Chromely.Core;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class CefGlueBrowserWindow : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowserWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefGlueBrowserWindow(ChromelyConfiguration hostConfig) 
            : base(hostConfig)
        {
        }

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected override void PlatformInitialize()
        {
            Application.Init();
        }

        /// <summary>
        /// The platform shutdown.
        /// </summary>
        protected override void PlatformShutdown()
        {
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected override void PlatformRunMessageLoop()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Application.Run();
            }
            else
            {
                CefRuntime.RunMessageLoop();
            }
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void PlatformQuitMessageLoop()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Application.Quit();
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
        protected override Window CreateMainView()
        {
            return new Window(this, HostConfig);
        }
    }
}
