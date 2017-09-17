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
    using System.Reflection;
    using System.Collections.Generic;
    using WinApi.Windows;
    using Chromely.Core.RestfulService;
    using Chromely.Core.Infrastructure;
    using System.Linq;
    using CefSharp;
    using Chromely.CefSharpBrowser;
    using Chromely.Core;
    using Chromely.CefSharpBrowser.RequestHandlers;

    public sealed class CefSharpBrowserWinapiHost : EventedWindowCore, IChromelyServiceProvider
    {
        private ChromiumWebBrowser m_browser;

        public CefSharpBrowserWinapiHost(ChromelyConfiguration hostConfig)
        {
            HostConfig = hostConfig;
            m_browser = null;
            ServiceAssemblies = new List<Assembly>();
        }

        public ChromelyConfiguration HostConfig { get; private set; }

        public List<Assembly> ServiceAssemblies { get; private set; }

        protected override void OnCreate(ref CreateWindowPacket packet)
        {
            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);

            var settings = new CefSettings
            {
                LocalesDirPath = localFolder,
                MultiThreadedMessageLoop = true,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = (CefSharp.LogSeverity)HostConfig.LogSeverity,
                LogFile = HostConfig.CefLogFile
            };

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            m_browser = new ChromiumWebBrowser(this.Handle, HostConfig.StartUrl);

            m_browser.RequestHandler = new CefSharpDefaultRequestHandler();
            RegisterJsHandlers();

            base.OnCreate(ref packet);

            Log.Info("Cef browser successfully created.");
        }

        protected override void OnSize(ref SizePacket packet)
        {
            base.OnSize(ref packet);

            var size = packet.Size;
            m_browser.SetSize(size.Width, size.Height);
        }

        protected override void OnDestroy(ref Packet packet)
        {
            m_browser.Dispose();
            Cef.Shutdown();

            base.OnDestroy(ref packet);
        }

        public void RegisterExternalUrlScheme(UrlScheme scheme)
        {
            UrlSchemeProvider.RegisterScheme(scheme);
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
                ServiceRouteProvider.MergeRoutes(currentRouteDictionary);
            }
        }

        private void RegisterJsHandlers()
        {
            // Register javascript handlers
            IEnumerable<object> jsHandlerObjs = IoC.GetAllInstances(typeof(ChromelyJsHandler));
            if (jsHandlerObjs != null)
            {
                var jsHandlers = jsHandlerObjs.ToList();

                foreach (var item in jsHandlers)
                {
                    if (item is ChromelyJsHandler)
                    {
                        ChromelyJsHandler handler = (ChromelyJsHandler)item;
                        if (handler.RegisterAsAsync)
                        {
                            m_browser.RegisterAsyncJsObject(handler.JsMethod, handler.BoundObject);
                        }
                        else
                        {
                            m_browser.RegisterJsObject(handler.JsMethod, handler.BoundObject);
                        }
                    }
                }
            }
        }
    }
}
