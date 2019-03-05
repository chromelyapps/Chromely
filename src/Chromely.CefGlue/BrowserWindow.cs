using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    public class BrowserWindow
    {
        public static IDisposable Create(ChromelyConfiguration config)
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

        private static IDisposable CreateFromAssembly(string platform, ChromelyConfiguration config)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? ".";
            var dllName = Path.Combine(path, $"Chromely.CefGlue.{platform}.dll");
            var assembly = System.Reflection.Assembly.LoadFile(dllName);
            var type = assembly.GetTypes().First(t => t.Name == "CefGlueBrowserWindow");
            return Activator.CreateInstance(type, new object[] { config }) as IDisposable;
        }
        
    }
    
}