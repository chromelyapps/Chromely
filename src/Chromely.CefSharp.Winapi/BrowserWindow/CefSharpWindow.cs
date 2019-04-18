// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBrowserWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.Core;

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

        public void Initialize()
        {
            base.Initialize();
        }

        public void Close()
        {
            base.Close();
        }

        public void Exit()
        {
            base.Exit();
        }
    }
}
