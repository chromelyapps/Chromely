using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    public class ChromelyBrowserWindow
    {
        public static IDisposable Create(ChromelyConfiguration config)
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    return CreateFromAssembly("WinApi", config);
                case CefRuntimePlatform.Linux:
                    return CreateFromAssembly("Gtk", config);
            }
            
            throw new PlatformNotSupportedException($"Chromely.CefGlue does not support {CefRuntime.Platform}");
        }

        private static IDisposable CreateFromAssembly(string platform, ChromelyConfiguration config)
        {
            var type = typeof(ChromelyBrowserWindow).Assembly.GetTypes().First(t => t.Name == $"{platform}CefGlueBrowserWindow");
            return Activator.CreateInstance(type, new object[] { config }) as IDisposable;
        }
        
    }
    
}