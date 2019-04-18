using System;
using Chromely.Core.Host;
using Chromely.Core;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    public static class ChromelyWindow
    {
        public static IChromelyWindow Create(ChromelyConfiguration config)
        {
            return new CefSharpWindow(config);
        }
    }
}