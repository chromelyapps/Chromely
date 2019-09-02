// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfiguration.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable MemberCanBeProtected.Global

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.Core
{
    /// <summary>
    /// The Chromely configuration.
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ChromelyConfiguration
    {
        /// <summary>
        /// The instance.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        private static readonly ChromelyConfiguration instance = new ChromelyConfiguration();

        /// <summary>
        /// Prevents a default instance of the <see cref="ChromelyConfiguration"/> class from being created.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed. Suppression is OK here.")]
        private ChromelyConfiguration()
        {
            HostApi = ChromelyRuntime.DefaultHostApi;
            LoadCefBinariesIfNotFound = true;
            SilentCefBinariesLoading = false;
            PerformDependencyCheck = false;
            ShutdownCefOnExit = true;
            LogSeverity = LogSeverity.Warning;
            LogFile = Path.Combine("logs", "chromely.cef.log");
            HostPlacement = new WindowPlacement();
            HostCustomCreationStyle = null;
            UseDefaultSubprocess = ChromelyRuntime.Platform == ChromelyPlatform.Windows;
            Locale = "en-US";
            StartWebSocket = false;
            ServiceAssemblies = new List<ControllerAssemblyInfo>();
            CommandLineArgs = new List<Tuple<string, string, bool>>();
            CustomSettings = new Dictionary<string, object>();

#if DEBUG
            DebuggingMode = true;
#endif
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ChromelyConfiguration Instance
        {
            // ReSharper disable once ArrangeAccessorOwnerBody
            get
            {
                // ReSharper disable once ArrangeAccessorOwnerBody
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the desired host interface technology.
        /// </summary>
        public ChromelyHostApi HostApi { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether load cef binaries if not found.
        /// </summary>
        public bool LoadCefBinariesIfNotFound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether silent cef binaries loading.
        /// </summary>
        public bool SilentCefBinariesLoading { get; set; }

        /// <summary>
        /// Gets or sets the window placement - X, Y, Width, Height, Center (or not)
        /// </summary>
        public WindowPlacement HostPlacement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use default subprocess.
        /// </summary>
        public bool UseDefaultSubprocess { get; set; }

        /// <summary>
        /// Gets or sets the host/window/app title.
        /// </summary>
        public string HostTitle { get; set; }

        /// <summary>
        /// Gets or sets the host/window/app icon file.
        /// </summary>
        public string HostIconFile { get; set; }

        /// <summary>
        /// Gets or sets the host custom creation style.
        /// </summary>
        public object HostCustomCreationStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use host custom creation style.
        /// </summary>
        public bool UseHostCustomCreationStyle => HostCustomCreationStyle != null;

        /// <summary>
        /// Gets or sets a value indicating whether CEF browser creation should perform dependency check.
        /// </summary>
        public bool PerformDependencyCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CEF should be shutdown on application exit.
        /// Default is true.
        /// </summary>
        public bool ShutdownCefOnExit { get; set; }

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
        /// Gets the service assemblies.
        /// </summary>
        public List<ControllerAssemblyInfo> ServiceAssemblies { get; }

        /// <summary>
        /// Gets or sets the command line args.
        /// Tuple data:
        /// Key = Item1; Value/Option=Item2; IsKeyValuePair = Item3.
        /// </summary>
        public List<Tuple<string, string, bool>> CommandLineArgs { get; set; }

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

            EnsureExpectedWorkingDirectory();
            switch (ChromelyRuntime.Platform)
            {
                case ChromelyPlatform.MacOSX:
                    //TODO: check and define requirements for Mac OS - for now use linux settings
                
                case ChromelyPlatform.Linux:
                    return Instance
                        .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, false)
                        .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                        .WithCustomSetting(CefSettingKeys.NoSandbox, true)

                        .WithCommandLineArg("disable-extensions", "1")
                        .WithCommandLineArg("disable-gpu", "1")
                        .WithCommandLineArg("disable-gpu-compositing", "1")
                        .WithCommandLineArg("disable-smooth-scrolling", "1")
                        .WithCommandLineArg("no-sandbox", "1")
                        .WithCommandLineArg("no-zygote", "1");

                case ChromelyPlatform.Windows:
                    return Instance;

                default:
                    throw new PlatformNotSupportedException();
            }
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
            AppArgs = args;
            return this;
        }

        /// <summary>
        /// The with loading cef binaries if not found.
        /// </summary>
        /// <param name="loadIfNotFound">
        /// The load if not found.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithLoadingCefBinariesIfNotFound(bool loadIfNotFound)
        {
            LoadCefBinariesIfNotFound = loadIfNotFound;
            return this;
        }

        /// <summary>
        /// The with silent cef binaries loading.
        /// </summary>
        /// <param name="silentLoading">
        /// The silent loading.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithSilentCefBinariesLoading(bool silentLoading)
        {
            SilentCefBinariesLoading = silentLoading;
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
            PerformDependencyCheck = checkDependencies;
            return this;
        }

        /// <summary>
        /// The with shutdown cef on exit.
        /// </summary>
        /// <param name="shutdownCefOnExitFlag">
        /// The shutdown cef on exit flag.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithShutdownCefOnExit(bool shutdownCefOnExitFlag)
        {
            ShutdownCefOnExit = shutdownCefOnExitFlag;
            return this;
        }

        /// <summary>
        /// The with is debugging.
        /// </summary>
        /// <param name="debugging">
        /// The is debugging.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithDebuggingMode(bool debugging)
        {
            DebuggingMode = debugging;
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
            StartUrl = startUrl;
            return this;
        }

        /// <summary>
        /// The with host mode.
        /// </summary>
        /// <param name="windowState">
        /// The window state.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithHostMode(WindowState windowState)
        {
            if (HostPlacement == null)
            {
                throw new Exception("HostPlacement cannot be nul");
            }

            HostPlacement.State = windowState;
            if (HostPlacement.KioskMode)
            {
                if (HostPlacement.State == WindowState.Normal)
                {
                    HostPlacement.Frameless = false;
                }
            }

            return this;
        }

        /// <summary>
        /// Sets the hosting api to use to GTK
        /// </summary>
        /// <returns></returns>
        public ChromelyConfiguration WithGtkHostApi()
        {
            HostApi = ChromelyHostApi.Gtk;
            return this;
        }

        /// <summary>
        /// Sets the hosting api to use to Libui
        /// </summary>
        /// <returns></returns>
        public ChromelyConfiguration WithLibuiHostApi()
        {
            HostApi = ChromelyHostApi.Libui;
            return this;
        }

        /// <summary>
        /// The with default subprocess.
        /// </summary>
        /// <param name="useDefault">
        /// The use default.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration WithDefaultSubprocess(bool useDefault = true)
        {
            UseDefaultSubprocess = useDefault;

            if (useDefault)
            {
                // Disable security features
                WithCommandLineArg("default-encoding", "utf-8");
                WithCommandLineArg("allow-file-access-from-files");
                WithCommandLineArg("allow-universal-access-from-files");
                WithCommandLineArg("disable-web-security");
                WithCommandLineArg("ignore-certificate-errors");
            }

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
            HostTitle = hostTitle;
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
            HostIconFile = hostIconFile;
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
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithHostBounds(int width, int height, int left = 0, int top = 0)
        {
            if (HostPlacement == null) HostPlacement = new WindowPlacement();
            HostPlacement.Left = left;
            HostPlacement.Top = top;
            HostPlacement.Width = width;
            HostPlacement.Height = height;
            return this;
        }

        public ChromelyConfiguration WithHostFlag(HostFlagKey key, bool flag)
        {
            if (HostPlacement == null)
            {
                throw new Exception("HostPlacement cannot be null.");
            }

            switch (key)
            {
                case HostFlagKey.None:
                    break;

                case HostFlagKey.CenterScreen:
                    HostPlacement.CenterScreen = flag;
                    break;

                case HostFlagKey.Frameless:
                    HostPlacement.Frameless = flag;
                    break;

                case HostFlagKey.NoResize:
                    HostPlacement.NoResize = flag;
                    break;

                case HostFlagKey.NoMinMaxBoxes:
                    HostPlacement.NoMinMaxBoxes = flag;
                    break;

                case HostFlagKey.KioskMode:
                    HostPlacement.KioskMode = flag;
                    if (HostPlacement.KioskMode)
                    {
                        if (HostPlacement.State == WindowState.Normal)
                        {
                            HostPlacement.Frameless = false;
                        }
                    }
                    break;
            }

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
            LogSeverity = logSeverity;
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
            LogFile = logFile;
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
            Locale = locale;
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
            RegisterSchemeHandler(handler);

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
            RegisterSchemeHandler(handler);

            return this;
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
        /// <param name="onLoadStartServer">
        /// The onLoadStartServer.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public ChromelyConfiguration UseDefaultWebsocketHandler(string address, int port, bool onLoadStartServer)
        {
            WebsocketAddress = address;
            WebsocketPort = port;
            StartWebSocket = onLoadStartServer;
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
        /// <param name="option">
        /// The command line option. Examples:
        ///     .WithCommandLineArg("no-sandbox")
        ///     .WithCommandLineArg("no-zygote")
        /// Note that "--" is not required. 
        /// This option to add command line switch is not implemented for CefSharp and will be ignored.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithCommandLineArg(string option)
        {
            if (CommandLineArgs == null)
            {
                CommandLineArgs = new List<Tuple<string, string, bool>>();
            }

            CommandLineArgs.Add(new Tuple<string, string, bool>(null, option, false));
            return this;
        }

        /// <summary>
        /// Registers a new command line argument.
        /// </summary>
        /// <param name="nameKey">
        /// The key/name of argument.
        /// </param>
        /// <param name="value">
        /// The command line option value. Examples:
        ///     .WithCommandLineArg("no-sandbox", 1)
        ///     .WithCommandLineArg("no-zygote", 1)
        /// Note that "--" is not required.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration WithCommandLineArg(string nameKey, string value)
        {
            if (CommandLineArgs == null)
            {
                CommandLineArgs = new List<Tuple<string, string, bool>>();
            }

            CommandLineArgs.Add(new Tuple<string, string, bool>(nameKey, value, true));
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
            if (CustomSettings == null)
            {
                CustomSettings = new Dictionary<string, object>();
            }

            CustomSettings[propertyName] = value;
            return this;
        }


        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration RegisterServiceAssembly(string filename)
        {
            ServiceAssemblies?.RegisterServiceAssembly(Assembly.LoadFile(filename));
            return this;
        }

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration RegisterServiceAssembly(Assembly assembly)
        {
            ServiceAssemblies?.RegisterServiceAssembly(assembly);
            return this;
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration RegisterServiceAssemblies(string folder)
        {
            ServiceAssemblies?.RegisterServiceAssemblies(folder);
            return this;
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="fileNames">
        /// The file names.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public ChromelyConfiguration RegisterServiceAssemblies(List<string> fileNames)
        {
            ServiceAssemblies?.RegisterServiceAssemblies(fileNames);
            return this;
        }

        /// <summary>
        /// Registers customer url scheme.
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
        public virtual ChromelyConfiguration RegisterCustomerUrlScheme(string schemeName, string domainName)
        {
            var scheme = new UrlScheme(schemeName, domainName, UrlSchemeType.Custom);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        /// <summary>
        /// Registers customer url scheme.
        /// </summary>
        /// <param name="urlScheme">
        /// The url scheme object.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public virtual ChromelyConfiguration RegisterCustomerUrlScheme(UrlScheme urlScheme)
        {
            UrlSchemeProvider.RegisterScheme(urlScheme);
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
            var scheme = new UrlScheme(schemeName, domainName, UrlSchemeType.External);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        /// <summary>
        /// The register event handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public virtual ChromelyConfiguration RegisterEventHandler<T>(CefEventKey key, EventHandler<T> handler)
        {
            return RegisterEventHandler(key, new ChromelyEventHandler<T>(key, handler));
        }

        /// <summary>
        /// The register event handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public virtual ChromelyConfiguration RegisterEventHandler<T>(CefEventKey key, ChromelyEventHandler<T> handler)
        {
            var service = CefEventHandlerFakeTypes.GetHandlerType(key);
            IoC.RegisterInstance(service, handler.Key, handler);
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
            var service = CefCustomHandlerFakeTypes.GetHandlerType(key);
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
            return RegisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handler));
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
                var scheme = new UrlScheme(schemeHandler.SchemeName, schemeHandler.DomainName, UrlSchemeType.Custom);
                UrlSchemeProvider.RegisterScheme(scheme);
                IoC.RegisterInstance(typeof(ChromelySchemeHandler), schemeHandler.Key, schemeHandler);
            }

            return this;
        }

        /// <summary>
        /// Using local resource handling requires files to be relative to the 
        /// Expected working directory
        /// For example, if the app is launched via the taskbar the working directory gets changed to
        /// C:\Windows\system32
        /// This needs to be changed to the right one.
        /// </summary>
        private static void EnsureExpectedWorkingDirectory()
        {
           try
            {
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                 Directory.SetCurrentDirectory(appDirectory);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }
    }
}
