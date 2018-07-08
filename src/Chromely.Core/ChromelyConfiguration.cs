// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfiguration.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;
    using System.Collections.Generic;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The Chromely configuration.
    /// </summary>
    public class ChromelyConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyConfiguration"/> class.
        /// </summary>
        public ChromelyConfiguration()
        {
            this.PerformDependencyCheck = false;
            this.LogSeverity = LogSeverity.Warning;
            this.LogFile = "logs\\chromely.cef.log";
            this.HostWidth = 1200;
            this.HostHeight = 900;
            this.Locale = "en-US";
            this.StartWebSocket = false;
            this.CommandLineArgs = new Dictionary<string, string>();
            this.CustomSettings = new Dictionary<string, object>();

#if DEBUG
            this.DebuggingMode = true;
#endif
        }

        /// <summary>
        /// Gets or sets the host/window/app title.
        /// </summary>
        public string HostTitle { get; set; }

        /// <summary>
        /// Gets or sets the host/window/app width.
        /// </summary>
        public int HostWidth { get; set; }

        /// <summary>
        /// Gets or sets the host/window/app height.
        /// </summary>
        public int HostHeight { get; set; }

        /// <summary>
        /// Gets or sets the host/window/app icon file.
        /// </summary>
        public string HostIconFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CEF browser creation should perform dependency check.
        /// </summary>
        public bool PerformDependencyCheck { get; set; }

        /// <summary>
        /// Gets or sets the app args.
        /// </summary>
        public string[] AppArgs { get; set; }

        /// <summary>
        /// Gets or sets the start url/file.
        /// </summary>
        public string StartUrl { get; set; }

        /// <summary>
        /// Gets or sets the log severity.
        /// </summary>
        public LogSeverity LogSeverity { get; set; }

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether start web socket.
        /// </summary>
        public bool StartWebSocket { get; set; }

        /// <summary>
        /// Gets or sets the websocket address.
        /// </summary>
        public string WebsocketAddress { get; set; }

        /// <summary>
        /// Gets or sets the websocket port.
        /// </summary>
        public int WebsocketPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is debugging.
        /// </summary>
        public bool DebuggingMode { get; set; }

        /// <summary>
        /// Gets or sets the command line args.
        /// </summary>
        public Dictionary<string, string> CommandLineArgs { get; set; }

        /// <summary>
        /// Gets or sets the custom settings.
        /// </summary>
        public Dictionary<string, object> CustomSettings { get; set; }

        /// <summary>
        /// The create methods.
        /// </summary>
        /// <param name="container">
        /// The container of <see cref="IChromelyContainer"/> object.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration Create(IChromelyContainer container = null)
        {
            if (container != null)
            {
                IoC.Container = container;
            }

            return new ChromelyConfiguration();
        }

        /// <summary>
        /// Set app args.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithAppArgs(string[] args)
        {
            this.AppArgs = args;
            return this;
        }

        /// <summary>
        /// Sets Cef dependency check.
        /// </summary>
        /// <param name="checkDependencies">
        /// The check dependencies.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithDependencyCheck(bool checkDependencies)
        {
            this.PerformDependencyCheck = checkDependencies;
            return this;
        }

        /// <summary>
        /// The with is debugging.
        /// </summary>
        /// <param name="debugging">
        /// The isdebugging.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithDebuggingMode(bool debugging)
        {
            this.DebuggingMode = debugging;
            return this;
        }

        /// <summary>
        /// Sets start url.
        /// </summary>
        /// <param name="startUrl">
        /// The start url.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithStartUrl(string startUrl)
        {
            this.StartUrl = startUrl;
            return this;
        }

        /// <summary>
        /// Sets host/window/app title.
        /// </summary>
        /// <param name="hostTitle">
        /// The host title.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithHostTitle(string hostTitle)
        {
            this.HostTitle = hostTitle;
            return this;
        }

        /// <summary>
        /// Sets host/window/app icon file.
        /// </summary>
        /// <param name="hostIconFile">
        /// The host icon file.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithHostIconFile(string hostIconFile)
        {
            this.HostIconFile = hostIconFile;
            return this;
        }

        /// <summary>
        /// Sets host/window/app size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithHostSize(int width, int height)
        {
            this.HostWidth = width;
            this.HostHeight = height;
            return this;
        }

        /// <summary>
        /// Sets log severity.
        /// </summary>
        /// <param name="logSeverity">
        /// The log severity.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithLogSeverity(LogSeverity logSeverity)
        {
            this.LogSeverity = logSeverity;
            return this;
        }

        /// <summary>
        /// Sets log file.
        /// </summary>
        /// <param name="logFile">
        /// The log file.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithLogFile(string logFile)
        {
            this.LogFile = logFile;
            return this;
        }

        /// <summary>
        /// Sets locale.
        /// </summary>
        /// <param name="locale">
        /// The locale.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithLocale(string locale)
        {
            this.Locale = locale;
            return this;
        }

        /// <summary>
        /// Sets use default logger flag.
        /// </summary>
        /// <param name="logFile">
        /// The log file.
        /// </param>
        /// <param name="logToConsole">
        /// The log to console.
        /// </param>
        /// <param name="rollingMaxMbFileSize">
        /// The rolling max mb file size.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration UseDefaultLogger(string logFile = null, bool logToConsole = true, int rollingMaxMbFileSize = 10)
        {
            var simpleLogger = new SimpleLogger(logFile, logToConsole, rollingMaxMbFileSize);
            IoC.RegisterInstance(typeof(IChromelyLogger), typeof(Log).FullName, simpleLogger);
            return this;
        }

        /// <summary>
        /// Sets use default resource scheme handler flag.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration UseDefaultResourceSchemeHandler(string schemeName, string domainName)
        {
            var handler = new ChromelySchemeHandler(schemeName, domainName, true, false);
            this.RegisterSchemeHandler(handler);

            return this;
        }

        /// <summary>
        /// Sets use default http scheme handler flag.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration UseDefaultHttpSchemeHandler(string schemeName, string domainName)
        {
            var handler = new ChromelySchemeHandler(schemeName, domainName, false, true);
            this.RegisterSchemeHandler(handler);

            return this;
        }

        /// <summary>
        /// Sets use defaut Javascript object handler flag.
        /// </summary>
        /// <param name="objectNameToBind">
        /// The object name to bind.
        /// </param>
        /// <param name="registerAsync">
        /// The register async.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration UseDefautJsHandler(string objectNameToBind, bool registerAsync)
        {
            return this.RegisterJsHandler(new ChromelyJsHandler(objectNameToBind, registerAsync));
        }

        /// <summary>
        /// The use default websocket handler.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="onloadstartserver">
        /// The onloadstartserver.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration UseDefaultWebsocketHandler(string address, int port, bool onloadstartserver)
        {
            this.WebsocketAddress = address;
            this.WebsocketPort = port;
            this.StartWebSocket = onloadstartserver;
            return this;
        }

        /// <summary>
        /// Sets with logger object to use. Should be of type <see cref="IChromelyLogger"/>
        /// </summary>
        /// <param name="logger">
        /// The logger - type <see cref="IChromelyLogger"/> object.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithLogger(IChromelyLogger logger)
        {
            Log.Logger = logger;
            return this;
        }

        /// <summary>
        /// Registers a new command line argument.
        /// </summary>
        /// <param name="nameKey">
        /// The key/name of argument.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithCommandLineArg(string nameKey, string value)
        {
            if (this.CommandLineArgs == null)
            {
                this.CommandLineArgs = new Dictionary<string, string>();
            }

            this.CommandLineArgs[nameKey] = value;
            return this;
        }

        /// <summary>
        /// Registers a new custom setting.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithCustomSetting(string propertyName, object value)
        {
            if (this.CustomSettings == null)
            {
                this.CustomSettings = new Dictionary<string, object>();
            }

            this.CustomSettings[propertyName] = value;
            return this;
        }

        /// <summary>
        /// Registers customr url scheme.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterCustomrUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, false);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        /// <summary>
        /// Registers external url scheme.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterExternalUrlScheme(string schemeName, string domainName)
        {
            var scheme = new UrlScheme(schemeName, domainName, true);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        /// <summary>
        /// Registers custom handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="implementation">
        /// The implementation.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterCustomHandler(CefHandlerKey key, Type implementation)
        {
            var service = CefHandlerFakeTypes.GetHandlerType(key);
            var keyStr = key.EnumToString();
            IoC.RegisterPerRequest(service, keyStr, implementation);

            return this;
        }

        /// <summary>
        /// Registers scheme handler.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterSchemeHandler(string schemeName, string domainName, object handler)
        {
            return this.RegisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handler));
        }

        /// <summary>
        /// Registers scheme handler.
        /// </summary>
        /// <param name="schemeHandler">
        /// The chromely scheme handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterSchemeHandler(ChromelySchemeHandler schemeHandler)
        {
            if (schemeHandler != null)
            {
                var scheme = new UrlScheme(schemeHandler.SchemeName, schemeHandler.DomainName, false);
                UrlSchemeProvider.RegisterScheme(scheme);
                IoC.RegisterInstance(typeof(ChromelySchemeHandler), schemeHandler.Key, schemeHandler);
            }

            return this;
        }

        /// <summary>
        /// Registers Javascript object handler.
        /// </summary>
        /// <param name="jsMethod">
        /// The js method.
        /// </param>
        /// <param name="boundObject">
        /// The bound object.
        /// </param>
        /// <param name="boundingOptions">
        /// The bounding options.
        /// </param>
        /// <param name="registerAsync">
        /// The register async.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterJsHandler(string jsMethod, object boundObject, object boundingOptions, bool registerAsync)
        {
            return this.RegisterJsHandler(new ChromelyJsHandler(jsMethod, boundObject, boundingOptions, registerAsync));
        }

        /// <summary>
        /// Registers Javascript object handler.
        /// </summary>
        /// <param name="chromelyJsHandler">
        /// The chromely js handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterJsHandler(ChromelyJsHandler chromelyJsHandler)
        {
            if (chromelyJsHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyJsHandler), chromelyJsHandler.Key, chromelyJsHandler);
            }

            return this;
        }

        /// <summary>
        /// Registers message router handler.
        /// </summary>
        /// <param name="messageRouterHandler">
        /// The chromely messsage router handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterMessageRouterHandler(object messageRouterHandler)
        {
            return this.RegisterMessageRouterHandler(new ChromelyMessageRouter(messageRouterHandler));
        }

        /// <summary>
        /// Registers message router handler.
        /// </summary>
        /// <param name="messageRouterHandler">
        /// The chromely messsage router.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterMessageRouterHandler(ChromelyMessageRouter messageRouterHandler)
        {
            if (messageRouterHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyMessageRouter), messageRouterHandler.Key, messageRouterHandler);
            }

            return this;
        }


        /// <summary>
        /// The register websocket handler.
        /// </summary>
        /// <param name="sockeHandler">
        /// The socke handler.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="onloadstartserver">
        /// The onloadstartserver.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration RegisterWebsocketHandler(IChromelyWebsocketHandler sockeHandler, string address, int port, bool onloadstartserver)
        {
            if (sockeHandler == null)
            {
                return this;
            }

            // Remove handler if exists - only one handler is allowed.
            bool isHandlerRegistered = IoC.IsRegistered<IChromelyWebsocketHandler>(typeof(IChromelyWebsocketHandler).FullName);
            if (isHandlerRegistered)
            {
                IoC.UnregisterHandler<IChromelyWebsocketHandler>(typeof(IChromelyWebsocketHandler).FullName);
            }

            IoC.RegisterInstance(typeof(IChromelyWebsocketHandler), typeof(IChromelyWebsocketHandler).FullName, sockeHandler);

            this.WebsocketAddress = address;
            this.WebsocketPort = port;
            this.StartWebSocket = onloadstartserver;
            return this;
        }

    }
}
