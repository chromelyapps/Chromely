using System.Runtime.InteropServices;
using Chromely.CefGlue.Loader;
using Chromely.Core;
using Xunit;

namespace Chromely.CefGlue.Tests.Loader
{
    public class CefLoaderTests
    {
        private static void CheckArchiveName(int build)
        {
            var name = CefLoader.FindCefArchiveName(ChromelyPlatform.Windows, Architecture.X86, build);
            Assert.True(!string.IsNullOrEmpty(name), $"Not found {build} for Windows x86");

            name = CefLoader.FindCefArchiveName(ChromelyPlatform.Windows, Architecture.X64, build);
            Assert.True(!string.IsNullOrEmpty(name), $"Not found {build} for Windows x64");

            name = CefLoader.FindCefArchiveName(ChromelyPlatform.Linux, Architecture.X86, build);
            Assert.True(!string.IsNullOrEmpty(name), $"Not found {build} for Linux x86");

            name = CefLoader.FindCefArchiveName(ChromelyPlatform.Linux, Architecture.X64, build);
            Assert.True(!string.IsNullOrEmpty(name), $"Not found {build} for Linux x86");
            
            name = CefLoader.FindCefArchiveName(ChromelyPlatform.MacOSX, Architecture.X64, build);
            Assert.True(!string.IsNullOrEmpty(name), $"Not found {build} for MaxOSX x64");
        }
        
        
        [Fact]
        public void FindCefArchiveNameShouldFindBuild3538AsChrome70()
        {
            CheckArchiveName(3538); 
        }

        [Fact]
        public void FindCefArchiveNameShouldFindBuild3578AsChrome71()
        {
            CheckArchiveName(3578); 
        }

        [Fact]
        public void FindCefArchiveNameShouldFindBuild3683AsChrome73()
        {
            CheckArchiveName(3683); 
        }

        
    }
}