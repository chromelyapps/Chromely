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

namespace Chromely.CefSharp.Winapi.ChromeHost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using WinApi.Windows;

    using CefSharpGlobal = global::CefSharp;

    public class CefSharpBrowserHost : EventedWindowCore, IChromelyServiceProvider
    {
        private ChromiumWebBrowser m_browser;

        public CefSharpBrowserHost(ChromelyConfiguration hostConfig)
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
            CefSharpGlobal.Cef.EnableHighDPISupport();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            var localesDirPath = Path.Combine(localFolder, "locales");
            var locale = "en-US";

            var settings = new CefSharpGlobal.CefSettings
            {
                LocalesDirPath = localesDirPath,
                Locale = locale,
                MultiThreadedMessageLoop = true,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = (CefSharpGlobal.LogSeverity)HostConfig.CefLogSeverity,
                LogFile = HostConfig.CefLogFile
            };

            RegisterSchemeHandlers(settings);

            //Perform dependency check to make sure all relevant resources are in our output directory.
            CefSharpGlobal.Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            m_browser = new ChromiumWebBrowser(Handle, HostConfig.CefStartUrl);
            m_browser.IsBrowserInitializedChanged += IsBrowserInitializedChanged;
            m_browser.MenuHandler = new CefSharpContextMenuHandler();

            m_browser.RequestHandler = new CefSharpRequestHandler();
            RegisterJsHandlers();

            base.OnCreate(ref packet);

            Log.Info("Cef browser successfully created.");
        }

        private void IsBrowserInitializedChanged(object sender, CefSharpGlobal.IsBrowserInitializedChangedEventArgs eventArgs)
        {
            if (eventArgs.IsBrowserInitialized)
            {
                var size = GetClientSize();
                m_browser.SetSize(size.Width, size.Height);
                m_browser.IsBrowserInitializedChanged -= IsBrowserInitializedChanged;
            }
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
            CefSharpGlobal.Cef.Shutdown();

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

        private void RegisterSchemeHandlers(CefSharpGlobal.CefSettings settings)
        {
            // Register scheme handlers
            IEnumerable<object> schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();

                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler)
                    {
                        ChromelySchemeHandler handler = (ChromelySchemeHandler)item;
                        settings.RegisterScheme(new CefSharpGlobal.CefCustomScheme
                        {
                            SchemeName = handler.SchemeName,
                            DomainName = handler.DomainName,
                            SchemeHandlerFactory = (CefSharpGlobal.ISchemeHandlerFactory)handler.HandlerFactory
                        });
                    }
                }
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
                        CefSharpGlobal.BindingOptions options = null;
                        if ((handler.BindingOptions != null) && (handler.BindingOptions is CefSharpGlobal.BindingOptions))
                        {
                            options = (CefSharpGlobal.BindingOptions)handler.BindingOptions;
                        }

                        if (handler.RegisterAsAsync)
                        {
                            m_browser.RegisterAsyncJsObject(handler.JsMethod, handler.BoundObject, options);
                        }
                        else
                        {
                            m_browser.RegisterJsObject(handler.JsMethod, handler.BoundObject, options);
                        }
                    }
                }
            }
        }
    }
}
