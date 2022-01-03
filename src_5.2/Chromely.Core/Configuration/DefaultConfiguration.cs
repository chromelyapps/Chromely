// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chromely.Core.Configuration
{
    public class DefaultConfiguration : IChromelyConfiguration
    {
        /// <summary>Gets or sets the name of the application.</summary>
        public string? AppName { get; set; }

        /// <summary>Gets or sets the start URL.</summary>
        public string StartUrl { get; set; }

        /// <summary>Gets or sets the application executable location.</summary>
        public string AppExeLocation { get; set; }

        /// <summary>Gets or sets the chromely version.</summary>
        public string? ChromelyVersion { get; set; }

        /// <summary>Gets or sets the platform.</summary>
        public ChromelyPlatform Platform { get; set; }

        /// <summary>Gets or sets a value indicating whether debugging is enabled or not.</summary>
        public bool DebuggingMode { get; set; }

        /// <summary>Gets or sets the dev tools URL.</summary>
        public string? DevToolsUrl { get; set; }

        /// <summary>Gets or sets the command line arguments.</summary>
        public Dictionary<string, string>? CommandLineArgs { get; set; }

        /// <summary>Gets or sets the command line options.</summary>
        public List<string>? CommandLineOptions { get; set; }

        /// <summary>Gets or sets the custom settings.</summary>
        public Dictionary<string, string>? CustomSettings { get; set; }

        /// <summary>Gets or sets the extension data.</summary>
        public Dictionary<string, object>? ExtensionData { get; set; }

        /// <summary>Gets or sets the java script executor.</summary>
        public IChromelyJavaScriptExecutor? JavaScriptExecutor { get; set; }

        /// <summary>Gets or sets the URL schemes.</summary>
        public List<UrlScheme> UrlSchemes { get; set; }

        /// <summary>Gets or sets the cef download options.</summary>
        public CefDownloadOptions CefDownloadOptions { get; set; }

        /// <summary>Gets or sets the window options.</summary>
        public IWindowOptions WindowOptions { get; set; }

        public DefaultConfiguration()
        {
            AppName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Chromely App";
            Platform = ChromelyRuntime.Platform;
            AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
            StartUrl = "local://app/index.html";
            DebuggingMode = true;
            UrlSchemes = new List<UrlScheme>();
            CefDownloadOptions = new CefDownloadOptions();
            WindowOptions = new WindowOptions();
            if (string.IsNullOrWhiteSpace(WindowOptions.Title))
            {
                WindowOptions.Title = AppName;
            }

            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme(DefaultSchemeName.LOCALRESOURCE, "local", string.Empty, string.Empty, UrlSchemeType.LocalResource),
                new UrlScheme(DefaultSchemeName.LOCALREQUEST, "http", "chromely.com", string.Empty, UrlSchemeType.LocalRequest),
                new UrlScheme(DefaultSchemeName.OWIN, "http", "chromely.owin.com", string.Empty, UrlSchemeType.Owin),
                new UrlScheme(DefaultSchemeName.GITHUBSITE, string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.ExternalBrowser, true)
            });

            CustomSettings = new Dictionary<string, string>()
            {
                ["cefLogFile"] = "logs\\chromely.cef.log",
                ["logSeverity"] = "info",
                ["locale"] = "en-US"
            };
        }

        public virtual void Update(ConfiguratorSection configSection)
        {
            if (configSection is null) return;

            if (!StringUtil.IsNullOrWhiteSpace(configSection.AppName)) AppName = configSection.AppName;
#pragma warning disable CS8601 // Possible null reference assignment.
            if (!StringUtil.IsNullOrWhiteSpace(configSection.StartUrl)) StartUrl = configSection.StartUrl;
#pragma warning restore CS8601 // Possible null reference assignment.
            if (!string.IsNullOrWhiteSpace(configSection.ChromelyVersion)) ChromelyVersion = configSection.ChromelyVersion;
            if (!string.IsNullOrWhiteSpace(configSection.DevToolsUrl)) DevToolsUrl = configSection.DevToolsUrl;
            if (!string.IsNullOrWhiteSpace(configSection.AppName)) AppName = configSection.AppName;

            DebuggingMode = configSection.DebuggingMode;

            if (CommandLineArgs is not null) CommandLineArgs = configSection.CommandLineArgs;
            if (CommandLineOptions is not null) CommandLineOptions = configSection.CommandLineOptions;
            if (CustomSettings is not null) CustomSettings = configSection.CustomSettings;
            if (ExtensionData is not null) ExtensionData = configSection.ExtensionData;
        }

        public static IChromelyConfiguration CreateForRuntimePlatform()
        {
            return CreateForPlatform(ChromelyRuntime.Platform);
        }

        public static IChromelyConfiguration CreateForPlatform(ChromelyPlatform platform)
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
                            "disable-gpu"
                        };
                        break;

                    case ChromelyPlatform.MacOSX:
                        break;
                }

                return config;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception);
            }

            return config;
        }

        public static IChromelyConfiguration? CreateFromConfigSection(IConfigurationRoot configuration, string sectionName = "ChromelyConfig")
        {
            if (configuration is null)
                return null;

            var configSection = configuration.GetSection(sectionName).Get(typeof(ConfiguratorSection)) as ConfiguratorSection;
            if (configSection is not null)
            {
                var config = new DefaultConfiguration();
                config.Update(configSection);
                return config;
            }

            return null;
        }
    }
}