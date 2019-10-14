using Chromely.Core;

namespace Chromely.Native
{
    public static class NativeGuiFactory
    {
        public static INativeGui GetNativeGui(ChromelyPlatform chromelyPlatform)
        {
            switch (chromelyPlatform)
            {
                case ChromelyPlatform.MacOSX:
                    return new MacNativeGui();

                case ChromelyPlatform.Linux:
                    return new LinuxNativeGui(); 

                case ChromelyPlatform.Windows:
                    return new WinNativeGui();

                default:
                    return new WinNativeGui();
            }
        }
    }
}
