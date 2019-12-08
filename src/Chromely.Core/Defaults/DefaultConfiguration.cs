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
            WindowOptions = new WindowOptions()
            {
                Title = AppName
            };

            Platform = ChromelyRuntime.Platform;
            AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
        }

        public static IChromelyConfiguration CreateOSDefault(ChromelyPlatform platform)
        {
            IChromelyConfiguration config;

            try
            {
                config = new DefaultConfiguration
                {
                    AppName = "chromely_app",
                    StartUrl = "local://app/chromely.html",
                    DebuggingMode = true,
                    UrlSchemes = new List<UrlScheme>(),
                    CefDownloadOptions = new CefDownloadOptions()
                    {
                        AutoDownloadWhenMissing = true,
                        DownloadSilently = false
                    }
                };

                var schemeDefaultResource = new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false);
                var schemeCustomHttp = new UrlScheme("default-custom-http", "http", "chromely.com", string.Empty, UrlSchemeType.Custom, false);
                var schemeCommandHttp = new UrlScheme("default-command-http", "http", "command.com", string.Empty, UrlSchemeType.Command, false);
                var schemeExternal1 = new UrlScheme("chromely-site", string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.External, true);

                config.UrlSchemes.Add(schemeDefaultResource);
                config.UrlSchemes.Add(schemeCustomHttp);
                config.UrlSchemes.Add(schemeCommandHttp);
                config.UrlSchemes.Add(schemeExternal1);

                config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
                config.ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");

                config.CustomSettings = new Dictionary<string, string>();
                config.CustomSettings["cefLogFile"] = "logs\\chromely.cef.log";
                config.CustomSettings["logSeverity"] = "info";
                config.CustomSettings["locale"] = "en-US";

                switch (platform)
                {
                    case ChromelyPlatform.Windows:
                        config.WindowOptions.CustomStyle = new WindowCustomStyle(0, 0);
                        config.WindowOptions.UseCustomStyle = false;
                        break;

                    case ChromelyPlatform.Linux:
                        config.CommandLineArgs = new Dictionary<string, string>();
                        config.CommandLineArgs["disable-gpu"] = "1";

                        config.CommandLineOptions = new List<string>();
                        config.CommandLineOptions.Add("no-zygote");
                        config.CommandLineOptions.Add("disable-gpu");
                        config.CommandLineOptions.Add("disable-software-rasterizer");
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