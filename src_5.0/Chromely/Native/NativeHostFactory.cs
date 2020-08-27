using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;

namespace Chromely.Native
{
    public static class NativeHostFactory
    {
        public static IChromelyNativeHost GetNativeHost(IChromelyConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            switch (config.Platform)
            {
                case ChromelyPlatform.MacOSX:
                    return new MacCocoaHost();

                case ChromelyPlatform.Linux:
                    return new LinuxGtk3Host(); 

                case ChromelyPlatform.Windows:
                    if (config.WindowOptions != null && config.WindowOptions.WindowFrameless)
                    {
                        return new FramelessWinAPIHost();
                    }

                    return new WinAPIHost();

                default:
                    return new WinAPIHost();
            }
        }
    }
}
