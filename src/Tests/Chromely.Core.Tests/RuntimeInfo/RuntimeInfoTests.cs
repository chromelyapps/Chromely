// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuntimeInfoTests.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

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
        

        // No idea to run this test without referencing Chromely.CefSharp in project
        // but this will blow up linux compilation due CefSharp is NOT cross plattform.

        //[Fact]
        //public void ExpectedCefSharpBuildNumberShouldNotBeZero()
        //{
        //    if (ChromelyRuntime.Platform != ChromelyPlatform.Windows) return;
            
        //    var buildNumber = ChromelyRuntime.GetExpectedChromiumBuildNumber(ChromelyCefWrapper.CefSharp);
        //    Assert.True(buildNumber > 3300);
        //}
        
    }
}
