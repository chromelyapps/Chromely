/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.ChromeHosts.Winapi
{
    using System;
    using System.IO;
    using Xilium.CefGlue;
    using System.Reflection;
    using Chromely.Browser;
    using Chromely.Browser.ResourceHandlers;
    using System.Collections.Generic;
    using WinApi.Windows;
    using Chromely.RestfulService;
    using Chromely.Infrastructure;

    public sealed class ChromeBrowserWinapi : EventedWindowCore, IChromelyServiceProvider
    {
        private static ILogger Logger = LoggerFactory.GetLogger();

        public ChromeBrowserWinapi(HostConfig hostConfig)
        {
            HostConfig = hostConfig;
            Browser = null;
            ServiceAssemblies = new List<Assembly>();
        }

        public HostConfig HostConfig { get; private set; }

        public CefWebBrowser Browser { get; private set; }

        public List<Assembly> ServiceAssemblies { get; private set; }

        protected override void OnCreate(ref CreateWindowPacket packet)
        {
            try
            {
                CefRuntime.Load();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var mainArgs = new CefMainArgs(HostConfig.AppArgs);
            var app = new CefWebApp(HostConfig.Scheme);

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
                return;

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);

            var settings = new CefSettings
            {
                // BrowserSubprocessPath = browserProcessPath,
                SingleProcess = false,
                MultiThreadedMessageLoop = true,
                LogSeverity = HostConfig.CefLogSeverity,
                LogFile = HostConfig.LogFile
            };

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory(HostConfig.Scheme, HostConfig.Domain, new ChromelySchemeHandlerFactory());

            CefBrowserConfig browserConfig = new CefBrowserConfig();
            browserConfig.StartUrl = HostConfig.StartUrl;
            browserConfig.ParentHandle = Handle;
            browserConfig.AppArgs = HostConfig.AppArgs;
            browserConfig.CefRectangle = new CefRectangle { X = 0, Y = 0, Width = HostConfig.Width, Height = HostConfig.Height };

            Browser = new CefWebBrowser(browserConfig);

            base.OnCreate(ref packet);

            Logger.LogInfo("Cef browser successfully created.");
        }

        protected override void OnSize(ref SizePacket packet)
        {
            base.OnSize(ref packet);

            var size = packet.Size;
            Browser.ResizeWindow(size.Width, size.Height);
        }

        protected override void OnDestroy(ref Packet packet)
        {
            Browser.Dispose();
            CefRuntime.Shutdown();

            base.OnDestroy(ref packet);
        }

        public void RegisterExternalUrlScheme(UrlScheme scheme)
        {
            ExternalUrlSchemeFactory.RegisterScheme(scheme);
        }

        public void RegisterServiceAssembly(string filename)
        {
            if (File.Exists(filename))
            {
                RegisterServiceAssembly(Assembly.LoadFile(filename));
            }
        }

        public void RegisterServiceAssembly(Assembly assembly)
        {
            if (ServiceAssemblies == null)
            {
                ServiceAssemblies = new List<Assembly>();
            }

            if (assembly != null)
            {
                ServiceAssemblies.Add(assembly);
            }
        }

        public void RegisterLogger(string loggerName, ILogger logger)
        {
            LoggerFactory.RegisterLogger(loggerName, logger);
        }

        public void ScanAssemblies()
        {
            if ((ServiceAssemblies == null) || (ServiceAssemblies.Count == 0))
            {
                return;
            }

            foreach (var assembly in ServiceAssemblies)
            {
                RouteScanner scanner = new RouteScanner(assembly);
                Dictionary<string, Route> currentRouteDictionary = scanner.Scan();
                ServiceRouteFactory.MergeRoutes(currentRouteDictionary);
            }
        }
    }
}
