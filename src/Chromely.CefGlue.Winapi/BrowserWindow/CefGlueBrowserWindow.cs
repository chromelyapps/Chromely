// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    using Chromely.Core;

    /// <summary>
    /// The cef glue chromium window.
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
        protected override void Initialize()
        {
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
            NativeWindow.RunMessageLoop();
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void QuitMessageLoop()
        {
            NativeWindow.Exit();
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
