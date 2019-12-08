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
        public string AppName { get; set; }
        public string StartUrl { get; set; }
        public string AppExeLocation { get; set; }
        public string ChromelyVersion { get; set; }
        public ChromelyPlatform Platform { get; set; }
        public bool DebuggingMode { get; set; }
        public string DevToolsUrl { get; set; }
        public IDictionary<string, string> CommandLineArgs { get; set; }
        public List<string> CommandLineOptions { get; set; }
        public List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }
        public IDictionary<string, string> CustomSettings { get; set; }
        public List<ChromelyEventHandler<object>> EventHandlers { get; set; }
        public IDictionary<string, object> ExtensionData { get; set; }
        public IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        public List<UrlScheme> UrlSchemes { get; set; }
        public CefDownloadOptions CefDownloadOptions { get; set; }
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
            WindowOptions = new WindowOptions();

            // set default value
            WindowOptions.Title = AppName;

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