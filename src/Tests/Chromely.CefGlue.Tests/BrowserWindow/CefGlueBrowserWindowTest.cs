// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserWindowTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.CefGlue.Tests.BrowserWindow
{
    /// <summary>
    /// The chromely configuration test.
    /// </summary>
    public class CefGlueBrowserWindowTest
    {
        /// <summary>
        /// The output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowserWindowTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public CefGlueBrowserWindowTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        /// <summary>
        /// The settings update test.
        /// </summary>
        [Fact]
        public void SettingsUpdateTest()
        {
            var hostConfig = GetConfigWithDefaultValues();
            var settings = new CefSettings
            {
                Locale = hostConfig.Locale,
                MultiThreadedMessageLoop = false,
                LogSeverity = (CefLogSeverity)hostConfig.LogSeverity,
                LogFile = hostConfig.LogFile,
                NoSandbox = false
            };

            // Update configuration settings
            settings.Update(hostConfig.CustomSettings);

            Assert.True(settings.NoSandbox);
            Assert.True(settings.MultiThreadedMessageLoop);
            Assert.True(settings.ExternalMessagePump);
            Assert.True(settings.WindowlessRenderingEnabled);
            Assert.True(settings.CommandLineArgsDisabled);
            Assert.True(settings.PackLoadingDisabled);
            Assert.True(settings.IgnoreCertificateErrors);
            Assert.True(settings.EnableNetSecurityExpiration);

            Assert.Equal(nameof(CefSettingKeys.BrowserSubprocessPath), settings.BrowserSubprocessPath);
            Assert.Equal(nameof(CefSettingKeys.CachePath), settings.CachePath);
            Assert.Equal(nameof(CefSettingKeys.UserDataPath), settings.UserDataPath);
            Assert.Equal(nameof(CefSettingKeys.UserAgent), settings.UserAgent);
            Assert.Equal(nameof(CefSettingKeys.ProductVersion), settings.ProductVersion);
            Assert.Equal(nameof(CefSettingKeys.Locale), settings.Locale);
            Assert.Equal(nameof(CefSettingKeys.LogFile), settings.LogFile);
            Assert.Equal(CefLogSeverity.Error, settings.LogSeverity);
            Assert.Equal(nameof(CefSettingKeys.JavaScriptFlags), settings.JavaScriptFlags);
            Assert.Equal(nameof(CefSettingKeys.ResourcesDirPath), settings.ResourcesDirPath);
            Assert.Equal(nameof(CefSettingKeys.LocalesDirPath), settings.LocalesDirPath);
            Assert.Equal(nameof(CefSettingKeys.AcceptLanguageList), settings.AcceptLanguageList);

            Assert.Equal(1025, settings.RemoteDebuggingPort);
            Assert.Equal(1000, settings.UncaughtExceptionStackSize);
        }

        /// <summary>
        /// The get config with default values.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        private ChromelyConfiguration GetConfigWithDefaultValues()
        {
            var defaultLogFile = Path.Combine("logs", "chromely_new.log");

            var config = ChromelyConfiguration.Create()
                .UseDefaultLogger(defaultLogFile)
                .UseDefaultResourceSchemeHandler("local", string.Empty)
                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                //.UseDefaultJsHandler("boundedObject", true)       only available with Chromely.CefSharp.Winapi.ChromelyConfigurationExtension
                .WithCustomSetting(CefSettingKeys.NoSandbox, true)
                .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, nameof(CefSettingKeys.BrowserSubprocessPath))
                .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, true)
                .WithCustomSetting(CefSettingKeys.ExternalMessagePump, true)
                .WithCustomSetting(CefSettingKeys.WindowlessRenderingEnabled, true)
                .WithCustomSetting(CefSettingKeys.CommandLineArgsDisabled, true)
                .WithCustomSetting(CefSettingKeys.CachePath, nameof(CefSettingKeys.CachePath))
                .WithCustomSetting(CefSettingKeys.UserDataPath, nameof(CefSettingKeys.UserDataPath))
                .WithCustomSetting(CefSettingKeys.UserAgent, nameof(CefSettingKeys.UserAgent))
                .WithCustomSetting(CefSettingKeys.ProductVersion, nameof(CefSettingKeys.ProductVersion))
                .WithCustomSetting(CefSettingKeys.Locale, nameof(CefSettingKeys.Locale))
                .WithCustomSetting(CefSettingKeys.LogFile, nameof(CefSettingKeys.LogFile))
                .WithCustomSetting(CefSettingKeys.LogSeverity, (int)CefLogSeverity.Error)
                .WithCustomSetting(CefSettingKeys.JavaScriptFlags, nameof(CefSettingKeys.JavaScriptFlags))
                .WithCustomSetting(CefSettingKeys.ResourcesDirPath, nameof(CefSettingKeys.ResourcesDirPath))
                .WithCustomSetting(CefSettingKeys.LocalesDirPath, nameof(CefSettingKeys.LocalesDirPath))
                .WithCustomSetting(CefSettingKeys.PackLoadingDisabled, true)
                .WithCustomSetting(CefSettingKeys.RemoteDebuggingPort, 1025)
                .WithCustomSetting(CefSettingKeys.UncaughtExceptionStackSize, 1000)
                .WithCustomSetting(CefSettingKeys.IgnoreCertificateErrors, true)
                .WithCustomSetting(CefSettingKeys.EnableNetSecurityExpiration, true)
                .WithCustomSetting(CefSettingKeys.AcceptLanguageList, nameof(CefSettingKeys.AcceptLanguageList));

            return config;
        }

        /// <summary>
        /// The get config.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        private ChromelyConfiguration GetConfig()
        {
            var title = "chromely";
            var iconFile = "chromely.ico";

            var hostWidth = 1200;
            var hostHeight = 900;

            var cefLogFile = Path.Combine("logs", "chromely.cef_new.log");
            var defaultLogFile = Path.Combine("logs", "chromely_new.log");
            var startUrl = "www.google.com";

            var logSeverity = LogSeverity.Error;
            var config = ChromelyConfiguration
                .Create()
                .WithHostTitle(title)
                .WithHostIconFile(iconFile)
                .WithAppArgs(null)
                .WithHostBounds(hostWidth, hostHeight)
                .WithLogFile(cefLogFile)
                .WithStartUrl(startUrl)
                .WithLogSeverity(logSeverity)
                .UseDefaultLogger(defaultLogFile);

            return config;
        }
    }
}
