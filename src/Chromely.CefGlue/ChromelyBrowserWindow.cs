using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    public class ChromelyBrowserWindow
    {
        public static HostBase Create(ChromelyConfiguration config)
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
        
        private static HostBase CreateFromAssembly(string platform, ChromelyConfiguration config)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? ".";
            var dllName = Path.Combine(path, $"Chromely.CefGlue.{platform}.dll");
            var assembly = System.Reflection.Assembly.LoadFile(dllName);
    
            var type = assembly.GetTypes().First(t => t.Name == $"{platform}CefGlueBrowserWindow");
            return Activator.CreateInstance(type, new object[] { config }) as HostBase;
        }

    }

}