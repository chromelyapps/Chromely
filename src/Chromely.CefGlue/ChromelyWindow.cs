using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    using Chromely.Core.Host;

    public class ChromelyWindow
    {
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
            var assembly = System.Reflection.Assembly.LoadFile(dllName);
    
            var type = assembly.GetTypes().First(t => t.Name == "CefGlueWindow");
            return Activator.CreateInstance(type, new object[] { config }) as HostBase;
        }

    }

}