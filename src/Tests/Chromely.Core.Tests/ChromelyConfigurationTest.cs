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
                                          .WithHostTitle(title)
                                          .WithHostIconFile(iconFile)
                                          .WithAppArgs(args)
                                          .WithHostSize(hostWidth, hostHeight)
                                          .WithLogFile(cefLogFile)
                                          .WithStartUrl(startUrl)
                                          .WithLogSeverity(logSeverity)
                                          .UseDefaultLogger(defaultLogFile, true);


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
