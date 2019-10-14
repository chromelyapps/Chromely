// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.CefGlue.BrowserWindow;

namespace Chromely.CefGlue
{
    /// <summary>
    /// Factory class to create the application window
    /// in a platform independent way.
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
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dllName = Path.Combine(path, $"Chromely.dll");
            var assembly = Assembly.LoadFile(dllName);

            var type = assembly.GetTypes().First(t => t.Name == "CefGlueWindow");
            return Activator.CreateInstance(type, config) as HostBase;
        }
    }

}