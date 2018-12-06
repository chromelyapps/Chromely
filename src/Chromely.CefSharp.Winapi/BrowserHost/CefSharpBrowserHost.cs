// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBrowserHost.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.BrowserHost
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
            mBrowser = null;
            mSettings = new CefSettings();
            HostConfig = hostConfig;
        }

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
            HostConfig?.ServiceAssemblies?.RegisterServiceAssembly(filename);
        }

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public void RegisterServiceAssembly(Assembly assembly)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssembly(assembly);
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        public void RegisterServiceAssemblies(string folder)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssemblies(folder);
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        public void RegisterServiceAssemblies(List<string> filenames)
        {
            HostConfig?.ServiceAssemblies?.RegisterServiceAssemblies(filenames);
        }

        /// <summary>
        /// Scan registered assemblies.
        /// </summary>
        public void ScanAssemblies()
        {
            if ((HostConfig?.ServiceAssemblies == null) ||
                HostConfig?.ServiceAssemblies.Count == 0)
            {
                return;
            }

            foreach (var assembly in HostConfig?.ServiceAssemblies)
            {
                if (!assembly.IsScanned)
                {
                    var scanner = new RouteScanner(assembly.Assembly);
                    var currentRouteDictionary = scanner.Scan();
                    ServiceRouteProvider.MergeRoutes(currentRouteDictionary);

                    assembly.IsScanned = true;
                }
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
                                mSettings.RegisterScheme(new CefCustomScheme
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
                                mSettings.RegisterScheme(new CefCustomScheme
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
                            mSettings.RegisterScheme(new CefCustomScheme
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
                            mBrowser.RegisterAsyncJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                        else
                        {
                            mBrowser.RegisterJsObject(handler.ObjectNameToBind, boundObject, options);
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

            mSettings = new CefSettings
            {
                LocalesDirPath = localesDirPath,
                Locale = HostConfig.Locale,
                MultiThreadedMessageLoop = true,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = (CefSharpGlobal.LogSeverity)HostConfig.LogSeverity,
                LogFile = HostConfig.LogFile
            };

            // Update configuration settings
            mSettings.Update(HostConfig.CustomSettings);
            mSettings.UpdateCommandLineArgs(HostConfig.CommandLineArgs);

            RegisterSchemeHandlers();

            // Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(mSettings, HostConfig.PerformDependencyCheck, null);

            mBrowser = new ChromiumWebBrowser(Handle, mSettings, HostConfig.StartUrl);
            mBrowser.IsBrowserInitializedChanged += IsBrowserInitializedChanged;

            // Set handlers
            mBrowser.SetHandlers();

            RegisterJsHandlers();

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

            if (mBrowser != null)
            {
                var size = packet.Size;
                mBrowser.SetSize(size.Width, size.Height);
            }
        }

        /// <summary>
        /// OnDestroy method.
        /// </summary>
        /// <param name="packet">
        /// The packet.
        /// </param>
        protected override void OnDestroy(ref Packet packet)
        {
            mBrowser?.Dispose();
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
                var size = GetClientSize();
                mBrowser.SetSize(size.Width, size.Height);
                mBrowser.IsBrowserInitializedChanged -= IsBrowserInitializedChanged;
            }
        }
    }
}
