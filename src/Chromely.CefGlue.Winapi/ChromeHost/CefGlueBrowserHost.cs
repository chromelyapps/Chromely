// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserHost.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.ChromeHost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefGlue.Winapi.Browser;
    using Chromely.CefGlue.Winapi.Browser.Handlers;
    using Chromely.Core;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using WinApi.Windows;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class CefGlueBrowserHost : EventedWindowCore, IChromelyHost, IChromelyServiceProvider
    {
        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private CefGlueBrowser mBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowserHost"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefGlueBrowserHost(ChromelyConfiguration hostConfig)
        {
            this.mBrowser = null;
            this.HostConfig = hostConfig;
            IoC.RegisterInstance(typeof(ChromelyConfiguration), typeof(ChromelyConfiguration).FullName, hostConfig);
            this.ServiceAssemblies = new List<Assembly>();
        }

        /// <summary>
        /// Gets the browser message router.
        /// </summary>
        public static CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

        /// <summary>
        /// Gets the service assemblies.
        /// </summary>
        public List<Assembly> ServiceAssemblies { get; }

        /// <summary>
        /// Gets the host config.
        /// </summary>
        public ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// The register url scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        public void RegisterUrlScheme(UrlScheme scheme)
        {
            UrlSchemeProvider.RegisterScheme(scheme);
        }

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void RegisterServiceAssembly(string filename)
        {
            this.ServiceAssemblies?.RegisterServiceAssembly(Assembly.LoadFile(filename));
        }

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public void RegisterServiceAssembly(Assembly assembly)
        {
            this.ServiceAssemblies?.RegisterServiceAssembly(assembly);
        }

        /// <summary>
        /// The register service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        public void RegisterServiceAssemblies(string folder)
        {
            this.ServiceAssemblies?.RegisterServiceAssemblies(folder);
        }

        /// <summary>
        /// The register service assemblies.
        /// </summary>
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        public void RegisterServiceAssemblies(List<string> filenames)
        {
            this.ServiceAssemblies?.RegisterServiceAssemblies(filenames);
        }

        /// <summary>
        /// The scan assemblies.
        /// </summary>
        public void ScanAssemblies()
        {
            if ((this.ServiceAssemblies == null) || 
                (this.ServiceAssemblies.Count == 0))
            {
                return;
            }

            foreach (var assembly in this.ServiceAssemblies)
            {
                var scanner = new RouteScanner(assembly);
                var currentRouteDictionary = scanner.Scan();
                ServiceRouteProvider.MergeRoutes(currentRouteDictionary);
            }
        }

        /// <summary>
        /// The register scheme handlers.
        /// </summary>
        public void RegisterSchemeHandlers()
        {
            // Register scheme handlers
            var schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();

                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler handler)
                    {
                        if (handler.HandlerFactory == null)
                        {
                            if (handler.UseDefaultResource)
                            {
                                CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, new CefGlueResourceSchemeHandlerFactory());
                            }

                            if (handler.UseDefaultHttp)
                            {
                                CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, new CefGlueHttpSchemeHandlerFactory());
                            }
                        }
                        else if (handler.HandlerFactory is CefSchemeHandlerFactory)
                        {
                            CefRuntime.RegisterSchemeHandlerFactory(handler.SchemeName, handler.DomainName, (CefSchemeHandlerFactory)handler.HandlerFactory);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The register message routers.
        /// </summary>
        public void RegisterMessageRouters()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                this.PostTask(CefThreadId.UI, this.RegisterMessageRouters);
                return;
            }

            BrowserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());

            // Register message router handlers
            var messageRouterHandlers = IoC.GetAllInstances(typeof(ChromelyMessageRouter)).ToList();
            if ((messageRouterHandlers != null) && (messageRouterHandlers.Count > 0))
            {
                var routerHandlers = messageRouterHandlers.ToList();

                foreach (var item in routerHandlers)
                {
                    var routerHandler = (ChromelyMessageRouter)item;
                    if (routerHandler.Handler is CefMessageRouterBrowserSide.Handler)
                    {
                        BrowserMessageRouter.AddHandler((CefMessageRouterBrowserSide.Handler)routerHandler.Handler);
                    }
                }
            }
            else
            {
                BrowserMessageRouter.AddHandler(new CefGlueMessageRouterHandler());
            }
        }

        /// <summary>
        /// The on create.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        /// <exception cref="Exception">
        /// Rethrown exception on Cef load failure.
        /// </exception>
        protected override void OnCreate(ref CreateWindowPacket packet)
        {
            // Will throw exception if Cef load fails. 
            CefRuntime.Load();

            var mainArgs = new CefMainArgs(this.HostConfig.AppArgs);
            var app = new CefGlueApp(this.HostConfig);

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
            {
                return;
            }

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            var localesDirPath = Path.Combine(localFolder, "locales");

            var settings = new CefSettings
            {
                LocalesDirPath = localesDirPath,
                Locale = this.HostConfig.Locale,
                SingleProcess = false,
                MultiThreadedMessageLoop = true,
                LogSeverity = (CefLogSeverity)this.HostConfig.LogSeverity,
                LogFile = this.HostConfig.LogFile,
                ResourcesDirPath = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath),
                NoSandbox = true
            };

            // Update configuration settings
            settings.Update(this.HostConfig.CustomSettings);

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            this.RegisterSchemeHandlers();
            this.RegisterMessageRouters();

            var browserConfig = new CefBrowserConfig
            {
                StartUrl = this.HostConfig.StartUrl,
                ParentHandle = this.Handle,
                AppArgs = this.HostConfig.AppArgs,
                StartWebSocket = this.HostConfig.StartWebSocket,
                WebsocketAddress = this.HostConfig.WebsocketAddress,
                WebsocketPort = this.HostConfig.WebsocketPort,
                CefRectangle =
                    new CefRectangle
                        {
                            X = 0,
                            Y = 0,
                            Width = this.HostConfig.HostWidth,
                            Height = this.HostConfig.HostHeight
                        }
            };

            this.mBrowser = new CefGlueBrowser(browserConfig);

            base.OnCreate(ref packet);

            Log.Info("Cef browser successfully created.");
        }

        /// <summary>
        /// The on size.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnSize(ref SizePacket packet)
        {
            base.OnSize(ref packet);

            var size = packet.Size;
            this.mBrowser.ResizeWindow(size.Width, size.Height);
        }

        /// <summary>
        /// The on destroy.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnDestroy(ref Packet packet)
        {
            this.mBrowser.Dispose();
            CefRuntime.Shutdown();

            base.OnDestroy(ref packet);
        }

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        private void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask : CefTask
        {
            /// <summary>
            /// The action.
            /// </summary>
            private Action mAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            public ActionTask(Action action)
            {
                this.mAction = action;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                this.mAction();
                this.mAction = null;
            }
        }
    }
}
