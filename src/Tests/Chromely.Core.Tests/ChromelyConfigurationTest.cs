// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationTest.cs" company="Chromely">
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

namespace Chromely.Core.Tests
{
    using Chromely.Core.Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The chromely configuration test.
    /// </summary>
    public class ChromelyConfigurationTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyConfigurationTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ChromelyConfigurationTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
        }

        /// <summary>
        /// The basic config test.
        /// </summary>
        [Fact]
        public void BasicConfigTest()
        {
            string title = "chromely";
            string iconFile = "chromely.ico";
            string[] args = null;

            int hostWidth = 1200;
            int hostHeight = 900;

            string cefLogFile = "logs\\chromely.cef_new.log";
            string defaultLogFile = "logs\\chromely_new.log";
            string startUrl = "www.google.com";

            var logSeverity = LogSeverity.Error;  
            var config = ChromelyConfiguration
                                          .Create()
                                          .WithHostTitle(title)
                                          .WithHostIconFile(iconFile)
                                          .WithAppArgs(null)
                                          .WithHostSize(hostWidth, hostHeight)
                                          .WithLogFile(cefLogFile)
                                          .WithStartUrl(startUrl)
                                          .WithLogSeverity(logSeverity)
                                          .UseDefaultLogger(defaultLogFile);

            Assert.NotNull(config);
            Assert.Equal(title, config.HostTitle);
            Assert.Equal(iconFile, config.HostIconFile);
            Assert.Equal(args, config.AppArgs);
            Assert.Equal(hostWidth, config.HostWidth);
            Assert.Equal(hostHeight, config.HostHeight);
            Assert.Equal(cefLogFile, config.LogFile);
            Assert.Equal(startUrl, config.StartUrl);
        }
    }
}
