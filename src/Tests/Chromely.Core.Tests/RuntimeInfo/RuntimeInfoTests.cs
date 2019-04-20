using Xunit;

namespace Chromely.Core.Tests.RuntimeInfo
{
    public class RuntimeInfoTests
    {
        
        [Fact]
        public void ExpectedCefGlueBuildNumberShouldNotBeZero()
        {
            var buildNumber = ChromelyRuntime.GetExpectedChromiumBuildNumber(ChromelyCefWrapper.CefGlue);
            Assert.True(buildNumber > 3300);
        }
        
        [Fact]
        public void ExpectedCefSharpBuildNumberShouldNotBeZero()
        {
            if (ChromelyRuntime.Platform != ChromelyPlatform.Windows) return;
            
            var buildNumber = ChromelyRuntime.GetExpectedChromiumBuildNumber(ChromelyCefWrapper.CefSharp);
            Assert.True(buildNumber > 3300);
        }
        
    }
}