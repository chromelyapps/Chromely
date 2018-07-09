// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBrowserHost.cs" company="Chromely">
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

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.ChromeHost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.CefSharp.Winapi.Browser.Internals;
    using Chromely.Core;
    using Chromely.Core.Host;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using WinApi.Windows;
    using global::CefSharp;

    using CefSharpGlobal = global::CefSharp;

    /// <summary>
    /// The CefSharp browser host/window/app.
    /// </summary>
    public class CefSharpBrowserHost : EventedWindowCore, IChromelyHost, IChromelyServiceProvider
    {
        /// <summary>
        /// The ChromiumWebBrowser object.
        /// </summary>
        private ChromiumWebBrowser mBrowser;

        /// <summary>
        /// The CefSettings object.
        /// </summary>
        private CefSettings mSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpBrowserHost"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefSharpBrowserHost(ChromelyConfiguration hostConfig)
        {
            this.mBrowser = null;
            this.mSettings = new CefSettings();
            this.HostConfig = hostConfig;
            IoC.RegisterInstance(typeof(ChromelyConfiguration), typeof(ChromelyConfiguration).FullName, hostConfig);
            this.ServiceAssemblies = new List<Assembly>();
        }

        /// <summary>
        /// Gets the service assemblies.
        /// </summary>
        public List<Assembly> ServiceAssemblies { get; }

        /// <summary>
        /// Gets the host config.
        /// </summary>
        public ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// Registers url scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        public void RegisterUrlScheme(UrlScheme scheme)
        {
            UrlSchemeProvider.RegisterScheme(scheme);
        }

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void RegisterServiceAssembly(string filename)
        {
            this.ServiceAssemblies?.RegisterServiceAssembly(Assembly.LoadFile(filename));
        }

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public void RegisterServiceAssembly(Assembly assembly)
        {
            this.ServiceAssemblies?.RegisterServiceAssembly(assembly);
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        public void RegisterServiceAssemblies(string folder)
        {
            this.ServiceAssemblies?.RegisterServiceAssemblies(folder);
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        public void RegisterServiceAssemblies(List<string> filenames)
        {
            this.ServiceAssemblies?.RegisterServiceAssemblies(filenames);
        }

        /// <summary>
        /// Scan registered assemblies.
        /// </summary>
        public void ScanAssemblies()
        {
            if ((this.ServiceAssemblies == null) || 
                this.ServiceAssemblies.Count == 0)
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
        /// Registers custom scheme handlers.
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
                                this.mSettings.RegisterScheme(new CefCustomScheme
                                {
                                    SchemeName = handler.SchemeName,
                                    DomainName = handler.DomainName,
                                    IsSecure = handler.IsSecure,
                                    IsCorsEnabled = handler.IsCorsEnabled,
                                    SchemeHandlerFactory = new CefSharpResourceSchemeHandlerFactory()
                                });
                            }

                            if (handler.UseDefaultHttp)
                            {
                                this.mSettings.RegisterScheme(new CefCustomScheme
                                {
                                    SchemeName = handler.SchemeName,
                                    DomainName = handler.DomainName,
                                    IsSecure = handler.IsSecure,
                                    IsCorsEnabled = handler.IsCorsEnabled,
                                    SchemeHandlerFactory = new CefSharpHttpSchemeHandlerFactory()
                                });
                            }
                        }
                        else if (handler.HandlerFactory is ISchemeHandlerFactory)
                        {
                            this.mSettings.RegisterScheme(new CefCustomScheme
                            {
                                SchemeName = handler.SchemeName,
                                DomainName = handler.DomainName,
                                IsSecure = handler.IsSecure,
                                IsCorsEnabled = handler.IsCorsEnabled,
                                SchemeHandlerFactory = (ISchemeHandlerFactory)handler.HandlerFactory
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers custom Javascript Bound (JSB) handlers.
        /// </summary>
        public void RegisterJsHandlers()
        {
            // Register javascript handlers
            var jsHandlerObjs = IoC.GetAllInstances(typeof(ChromelyJsHandler));
            if (jsHandlerObjs != null)
            {
                var jsHandlers = jsHandlerObjs.ToList();

                foreach (var item in jsHandlers)
                {
                    if (item is ChromelyJsHandler handler)
                    {
                        BindingOptions options = null;

                        if (handler.BindingOptions is BindingOptions)
                        {
                            options = (BindingOptions)handler.BindingOptions;
                        }

                        var boundObject = handler.UseDefault ? new CefSharpBoundObject() : handler.BoundObject;

                        if (handler.RegisterAsAsync)
                        {
                            this.mBrowser.RegisterAsyncJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                        else
                        {
                            this.mBrowser.RegisterJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers custom MessageRouter handlers.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// Exception - NotImplementedException.
        /// </exception>
        public void RegisterMessageRouters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// OnCreate method.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnCreate(ref CreateWindowPacket packet)
        {
            // For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            var localesDirPath = Path.Combine(localFolder ?? throw new InvalidOperationException(), "locales");

            this.mSettings = new CefSettings
            {
                LocalesDirPath = localesDirPath,
                Locale = this.HostConfig.Locale,
                MultiThreadedMessageLoop = true,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = (CefSharpGlobal.LogSeverity)this.HostConfig.LogSeverity,
                LogFile = this.HostConfig.LogFile
            };

            // Update configuration settings
            this.mSettings.Update(this.HostConfig.CustomSettings);
            this.mSettings.UpdateCommandLineArgs(this.HostConfig.CommandLineArgs);

            this.RegisterSchemeHandlers();

            // Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(this.mSettings, this.HostConfig.PerformDependencyCheck, null);

            this.mBrowser = new ChromiumWebBrowser(this.Handle, this.HostConfig.StartUrl);
            this.mBrowser.IsBrowserInitializedChanged += this.IsBrowserInitializedChanged;

            // Set handlers
            this.mBrowser.SetHandlers();

            this.RegisterJsHandlers();

            base.OnCreate(ref packet);

            Log.Info("Cef browser successfully created.");
        }

        /// <summary>
        /// OnSize method.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnSize(ref SizePacket packet)
        {
            base.OnSize(ref packet);

            var size = packet.Size;
            this.mBrowser.SetSize(size.Width, size.Height);
        }

        /// <summary>
        /// OnDestroy method.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnDestroy(ref Packet packet)
        {
            this.mBrowser.Dispose();
            Cef.Shutdown();

            base.OnDestroy(ref packet);
        }

        /// <summary>
        /// Browser initialized changed event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs eventArgs)
        {
            if (eventArgs.IsBrowserInitialized)
            {
                var size = this.GetClientSize();
                this.mBrowser.SetSize(size.Width, size.Height);
                this.mBrowser.IsBrowserInitializedChanged -= this.IsBrowserInitializedChanged;
            }
        }
    }
}
