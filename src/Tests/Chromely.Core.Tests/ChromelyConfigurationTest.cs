namespace Chromely.Core.Tests
{
    using Chromely.Core.Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class ChromelyConfigurationTest
    {
        private readonly ITestOutputHelper m_testOutput;

        public ChromelyConfigurationTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
        }

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

            LogSeverity logSeverity = LogSeverity.Error;  

            ChromelyConfiguration config = ChromelyConfiguration
                                          .Create()
                                          .WithCefTitle(title)
                                          .WithCefIconFile(iconFile)
                                          .WithCefAppArgs(args)
                                          .WithCefHostSize(hostWidth, hostHeight)
                                          .WithCefLogFile(cefLogFile)
                                          .WithCefStartUrl(startUrl)
                                          .WithCefLogSeverity(logSeverity)
                                          .UseDefaultLogger(defaultLogFile, true);


            Assert.NotNull(config);
            Assert.Equal(title, config.CefTitle);
            Assert.Equal(iconFile, config.CefIconFile);
            Assert.Equal(args, config.CefAppArgs);
            Assert.Equal(hostWidth, config.CefHostWidth);
            Assert.Equal(hostHeight, config.CefHostHeight);
            Assert.Equal(cefLogFile, config.CefLogFile);
            Assert.Equal(startUrl, config.CefStartUrl);
        }
    }
}
