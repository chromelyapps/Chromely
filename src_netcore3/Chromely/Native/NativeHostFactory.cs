using Chromely.Core;

namespace Chromely.Native
{
    public static class NativeHostFactory
    {
        public static INativeHost GetNativeHost(ChromelyConfiguration config)
        {
            switch (config.Platform)
            {
                case ChromelyPlatform.MacOSX:
                    if (config.HostType == ChromelyHostType.Gtk3)
                    {
                        return new MacGtk3Host();
                    }
                    return new MacCocoaHost();

                case ChromelyPlatform.Linux:
                    return new LinuxGtk3Host(); 

                case ChromelyPlatform.Windows:
                    if (config.HostType == ChromelyHostType.Gtk3)
                    {
                        return new WinGtk3Host();
                    }
                    return new WinAPIHost();

                default:
                    return new WinAPIHost();
            }
        }
    }
}
