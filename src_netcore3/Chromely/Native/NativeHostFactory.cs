using Chromely.Core;

namespace Chromely.Native
{
    public static class NativeHostFactory
    {
        public static INativeHost GetNativeHost(IChromelyConfiguration config)
        {
            switch (config.Platform)
            {
                case ChromelyPlatform.MacOSX:
                    return new MacCocoaHost();

                case ChromelyPlatform.Linux:
                    return new LinuxGtk3Host(); 

                case ChromelyPlatform.Windows:
                    throw new System.Exception("Windows host not migrated yet!");
                    //return new WinAPIHost();

                default:
                    return new WinAPIHost();
            }
        }
    }
}
