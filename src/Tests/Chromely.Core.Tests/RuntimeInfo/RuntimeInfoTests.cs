using Xunit;

namespace Chromely.Core.Tests.RuntimeInfo
{
    public class RuntimeInfoTests
    {
        
        [Fact]
        public void ExpectedCefGlueBuildNumberShouldNotBeZero()
        {
            var buildNumber = ChromelyRuntime.GetExpectedChromiumBuildNumber(ChromelyCefWrapper.CefGlue);
            Assert.NotEqual(0, buildNumber);
        }
        
        [Fact]
        public void ExpectedCefSharpBuildNumberShouldNotBeZero()
        {
            if (ChromelyRuntime.Platform != ChromelyPlatform.Windows) return;
            
            var buildNumber = ChromelyRuntime.GetExpectedChromiumBuildNumber(ChromelyCefWrapper.CefSharp);
            Assert.NotEqual(0, buildNumber);
        }
        
    }
}