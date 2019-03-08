// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GtkCefGlueBrowserWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.BrowserWindow
{
    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class GtkCefGlueBrowserWindow : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GtkCefGlueBrowserWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public GtkCefGlueBrowserWindow(ChromelyConfiguration hostConfig) 
            : base(hostConfig)
        {
        }

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected override void Initialize()
        {
            GtkNativeMethods.InitWindow(0, null);
        }

        /// <summary>
        /// The platform shutdown.
        /// </summary>
        protected override void Shutdown()
        {
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected override void RunMessageLoop()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                /* run the GTK+ main loop */
                GtkNativeMethods.Run();
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
                GtkNativeMethods.Quit();
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
        /// The <see cref="GtkWindow"/>.
        /// </returns>
        protected override IWindow CreateMainView()
        {
            return new GtkWindow(this, HostConfig);
        }
    }
}
