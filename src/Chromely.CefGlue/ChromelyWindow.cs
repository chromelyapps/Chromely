using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// Factory class to create the application window
    /// in a platform independent way.
    /// </summary>
    public class ChromelyWindow
    {
        /// <summary>
        /// Factory method to create main window.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Interface to the main window</returns>
        public static IChromelyWindow Create(ChromelyConfiguration config)
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    return CreateFromAssembly("Winapi", config);
                case CefRuntimePlatform.Linux:
                    return CreateFromAssembly("Gtk", config);
            }

            throw new PlatformNotSupportedException($"Chromely.CefGlue does not support {CefRuntime.Platform}");
        }
        
        private static IChromelyWindow CreateFromAssembly(string platform, ChromelyConfiguration config)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? ".";
            var dllName = Path.Combine(path, $"Chromely.CefGlue.{platform}.dll");
            var assembly = Assembly.LoadFile(dllName);
    
            var type = assembly.GetTypes().First(t => t.Name == "CefGlueWindow");
            return Activator.CreateInstance(type, config) as HostBase;
        }

    }

}