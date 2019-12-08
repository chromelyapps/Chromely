using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chromely.Core
{
    public class DefaultConfiguration : IChromelyConfiguration
    {
        /// <summary>Gets or sets the name of the application.</summary>
        public string AppName { get; set; }

        /// <summary>Gets or sets the start URL.</summary>
        public string StartUrl { get; set; }

        /// <summary>Gets or sets the application executable location.</summary>
        public string AppExeLocation { get; set; }

        /// <summary>Gets or sets the chromely version.</summary>
        public string ChromelyVersion { get; set; }

        /// <summary>Gets or sets the platform.</summary>
        public ChromelyPlatform Platform { get; set; }

        /// <summary>Gets or sets a value indicating whether debugging is enabled or not.</summary>
        public bool DebuggingMode { get; set; }

        /// <summary>Gets or sets the dev tools URL.</summary>
        public string DevToolsUrl { get; set; }

        /// <summary>Gets or sets the command line arguments.</summary>
        public IDictionary<string, string> CommandLineArgs { get; set; }

        /// <summary>Gets or sets the command line options.</summary>
        public List<string> CommandLineOptions { get; set; }

        /// <summary>Gets or sets the controller assemblies.</summary>
        public List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }

        /// <summary>Gets or sets the custom settings.</summary>
        public IDictionary<string, string> CustomSettings { get; set; }

        /// <summary>Gets or sets the event handlers.</summary>
        public List<ChromelyEventHandler<object>> EventHandlers { get; set; }

        /// <summary>Gets or sets the extension data.</summary>
        public IDictionary<string, object> ExtensionData { get; set; }

        /// <summary>Gets or sets the java script executor.</summary>
        public IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }

        /// <summary>Gets or sets the URL schemes.</summary>
        public List<UrlScheme> UrlSchemes { get; set; }

        /// <summary>Gets or sets the cef download options.</summary>
        public CefDownloadOptions CefDownloadOptions { get; set; }

        /// <summary>Gets or sets the window options.</summary>
        public IWindowOptions WindowOptions { get; set; }

        public DefaultConfiguration()
        {
            AppName = Assembly.GetEntryAssembly()?.GetName().Name;
            Platform = ChromelyRuntime.Platform;
            AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
            StartUrl = "local://app/chromely.html";
            DebuggingMode = true;
            UrlSchemes = new List<UrlScheme>();
            CefDownloadOptions = new CefDownloadOptions();
            WindowOptions = new WindowOptions
            {
                Title = AppName
            };

            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false),
                new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false),
                new UrlScheme("default-custom-http", "http", "chromely.com", string.Empty, UrlSchemeType.Custom, false),
                new UrlScheme("default-command-http", "http", "command.com", string.Empty, UrlSchemeType.Command, false),
                new UrlScheme("chromely-site", string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.External, true)
            });

            ControllerAssemblies = new List<ControllerAssemblyInfo>();
            ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");

            CustomSettings = new Dictionary<string, string>()
            {
                ["cefLogFile"] = "logs\\chromely.cef.log",
                ["logSeverity"] = "info",
                ["locale"] = "en-US"
            };
        }

        public static IChromelyConfiguration CreateConfigurationForPlatform(ChromelyPlatform platform)
        {
            IChromelyConfiguration config = new DefaultConfiguration();

            try
            {
                switch (platform)
                {
                    case ChromelyPlatform.Windows:
                        config.WindowOptions.CustomStyle = new WindowCustomStyle(0, 0);
                        config.WindowOptions.UseCustomStyle = false;
                        break;

                    case ChromelyPlatform.Linux:
                        config.CommandLineArgs = new Dictionary<string, string>
                        {
                            ["disable-gpu"] = "1"
                        };

                        config.CommandLineOptions = new List<string>()
                        {
                            "no-zygote",
                            "disable-gpu",
                            "disable-software-rasterizer"
                        };
                        break;

                    case ChromelyPlatform.MacOSX:
                        break;
                }

                return config;
            }
            catch (Exception exception)
            {
                config = null;
                Logger.Instance.Log.Error(exception);
            }

            return config;
        }
    }
}