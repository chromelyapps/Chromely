// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Host;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    /// <summary>
    /// Factory class to create the application window.
    /// This version is limited to windows and CefSharp.
    /// </summary>
    public static class ChromelyWindow
    {
        /// <summary>
        /// Factory method to create main window.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Interface to the main window</returns>
        public static IChromelyWindow Create(ChromelyConfiguration config)
        {
            return new CefSharpWindow(config);
        }
    }
}