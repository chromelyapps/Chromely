// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.Core.Tests
{
    /// <summary>
    /// The chromely configuration test.
    /// </summary>
    public class ChromelyConfigurationTest
    {
        /// <summary>
        /// The title.
        /// </summary>
        private const string Title = "chromely";

        /// <summary>
        /// The icon file.
        /// </summary>
        private const string IconFile = "chromely.ico";

        /// <summary>
        /// The args.
        /// </summary>
        private const string[] Args = null;

        /// <summary>
        /// The host width.
        /// </summary>
        private const int HostWidth = 1200;

        /// <summary>
        /// The host height.
        /// </summary>
        private const int HostHeight = 900;

        /// <summary>
        /// The cef log file.
        /// </summary>
        private readonly string CefLogFile = Path.Combine("logs", "chromely.cef_new.log");

        /// <summary>
        /// The default log file.
        /// </summary>
        private readonly string DefaultLogFile = Path.Combine("logs", "chromely_new.log");

        /// <summary>
        /// The start url.
        /// </summary>
        private const string StartUrl = "www.google.com";

        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyConfigurationTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ChromelyConfigurationTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        /// <summary>
        /// The basic config test.
        /// TODO: Rename test to express what is the intention instead of what it is doing
        /// </summary>
        [Fact]
        public void BasicConfigTest()
        {
            var config = GetBaseChromelyConfiguration();

            Assert.NotNull(config);
            Assert.Equal(Title, config.HostTitle);
            Assert.Equal(IconFile, config.HostIconFile);
            Assert.Equal(Args, config.AppArgs);
            Assert.Equal(HostWidth, config.HostPlacement.Width);
            Assert.Equal(HostHeight, config.HostPlacement.Height);
            Assert.Equal(CefLogFile, config.LogFile);
            Assert.Equal(StartUrl, config.StartUrl);
        }

        /// <summary>
        /// Ensure platform specific default settings.
        /// </summary>
        [Fact]
        public void ConfigurationShouldSetPlatformSpecificDefaults()
        {
            var config = ChromelyConfiguration.Create();

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Assert.True(config.CustomSettings.ContainsKey(CefSettingKeys.MultiThreadedMessageLoop));
                Assert.True(config.CustomSettings.ContainsKey(CefSettingKeys.SingleProcess));
                Assert.True(config.CustomSettings.ContainsKey(CefSettingKeys.NoSandbox));

                Assert.False((bool) config.CustomSettings[CefSettingKeys.MultiThreadedMessageLoop]);
                Assert.True((bool) config.CustomSettings[CefSettingKeys.SingleProcess]);
                Assert.True((bool) config.CustomSettings[CefSettingKeys.NoSandbox]);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Assert.False(config.CustomSettings.Any());
            }
        }

        /// <summary>
        /// The get base chromely configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        private ChromelyConfiguration GetBaseChromelyConfiguration()
        {
            var logSeverity = LogSeverity.Error;
            var config = ChromelyConfiguration.Create()
                .WithHostTitle(Title)
                .WithHostIconFile(IconFile)
                .WithAppArgs(null)
                .WithHostBounds(HostWidth, HostHeight)
                .WithLogFile(CefLogFile)
                .WithStartUrl(StartUrl)
                .WithLogSeverity(logSeverity)
                .UseDefaultLogger(DefaultLogFile);

            return config;
        }
    }
}
