// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using Xunit;

namespace Chromely.Tests.ChromelyCore
{
    public class ChromelyConfigTests
    {
        [Fact]
        public void ConfigTest()
        {
            // Arrange
            var appName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
            var windowTitle = appName;
            var platform = ChromelyRuntime.Platform;
            var appExeLocation = AppDomain.CurrentDomain.BaseDirectory;

            // Act
            var config = DefaultConfig;

            // Assert
            Assert.NotNull(config);
            Assert.Equal(appName, config.AppName);
            Assert.Equal(windowTitle, config.WindowOptions.Title);
            Assert.Equal(platform, config.Platform);
            Assert.Equal(appExeLocation, config.AppExeLocation);
        }

        private IChromelyConfiguration DefaultConfig
        {
            get
            {
                var config = new DefaultConfiguration();
                return config;
            }
        }

        private IChromelyConfiguration DefaultConfigFromFileExpectedValues
        {
            get
            {
                var config = new DefaultConfiguration
                {
                    AppName = "chromely_test",
                    StartUrl = "local://app/chromely.html",
                    DebuggingMode = true,
                    CefDownloadOptions = new CefDownloadOptions()
                    {
                        AutoDownloadWhenMissing = true,
                        DownloadSilently = false
                    },

                    WindowOptions = new WindowOptions
                    {
                        Size = new WindowSize(1200, 900),
                        Position = new WindowPosition(1, 2),
                        DisableResizing = false,
                        DisableMinMaximizeControls = false,
                        WindowFrameless = false,
                        StartCentered = true,
                        KioskMode = false,
                        WindowState = WindowState.Normal,
                        Title = "chromely",
                        RelativePathToIconFile = "chromely.ico",
                        CustomStyle = new WindowCustomStyle(0, 0),
                        UseCustomStyle = false
                    },

                    UrlSchemes = new List<UrlScheme>()
                };

                var schemeDefaultResource = new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false);
                var schemeCustomHttp = new UrlScheme("default-request-http", "http", "chromely.com", string.Empty, UrlSchemeType.LocalRequest, false);
                var schemeCommandHttp = new UrlScheme("default-command-http", "http", "command.com", string.Empty, UrlSchemeType.Command, false);
                var schemeExternal1 = new UrlScheme("chromely-site", string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.ExternalBrowser, true);

                config.UrlSchemes.Add(schemeDefaultResource);
                config.UrlSchemes.Add(schemeCustomHttp);
                config.UrlSchemes.Add(schemeCommandHttp);
                config.UrlSchemes.Add(schemeExternal1);

                config.CustomSettings = new Dictionary<string, string>();
                config.CustomSettings["cefLogFile"] = "logs\\chromely.cef.log";
                config.CustomSettings["logSeverity"] = "info";
                config.CustomSettings["locale"] = "en-US";

                config.CommandLineArgs = new Dictionary<string, string>();
                config.CommandLineArgs["disable-gpu"] = "1";

                config.CommandLineOptions = new List<string>();
                config.CommandLineOptions.Add("no-zygote");
                config.CommandLineOptions.Add("disable-gpu");
                config.CommandLineOptions.Add("disable-software-rasterizer");

                return config;
            }
        }
    }
}
