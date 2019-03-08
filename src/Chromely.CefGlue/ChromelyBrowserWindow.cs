using System;
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
                    return new WinApiCefGlueBrowserWindow(config);
                case CefRuntimePlatform.Linux:
                    return new GtkCefGlueBrowserWindow(config);
            }
            
            throw new PlatformNotSupportedException($"Chromely.CefGlue does not support {CefRuntime.Platform}");
        }
        
    }
    
}