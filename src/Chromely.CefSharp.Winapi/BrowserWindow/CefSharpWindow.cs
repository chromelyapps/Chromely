// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    /// <summary>
    /// The cefsharp chromium window.
    /// </summary>
    internal class CefSharpWindow : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefSharpWindow(ChromelyConfiguration hostConfig) 
            : base(hostConfig)
        {
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public new void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// The close.
        /// </summary>
        public new void Close()
        {
            base.Close();
        }

        /// <summary>
        /// The exit.
        /// </summary>
        public new void Exit()
        {
            base.Exit();
        }
    }
}
