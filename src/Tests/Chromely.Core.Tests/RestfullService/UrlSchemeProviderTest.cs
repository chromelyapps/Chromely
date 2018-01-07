namespace Chromely.Core.Tests.RestfullService
{
    using Chromely.Core.Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class UrlSchemeProviderTest
    {
        private readonly ITestOutputHelper m_testOutput;

        public UrlSchemeProviderTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
            Init();
        }

        [Fact]
        public void IsCustomUrlTest()
        {
            Assert.True(UrlSchemeProvider.IsUrlOfRegisteredCustomScheme("http://chromely.com/getvaluefromme"));
            Assert.True(UrlSchemeProvider.IsUrlOfRegisteredCustomScheme("https://test.com/savevaluetome"));
        }

        [Fact]
        public void IsExternalUrlTest()
        {
            Assert.True(UrlSchemeProvider.IsUrlRegisteredExternal("https://www.google.com/getvaluefromme"));
            Assert.True(UrlSchemeProvider.IsUrlRegisteredExternal("http://www.test.com/savevaluetome"));
        }

        private void Init()
        {
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http", "chromely.com", false));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https", "test.com", false));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https://www.google.com", true));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http://www.test.com", true));
        }
    }
}
