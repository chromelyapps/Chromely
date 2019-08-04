using Chromely.Core;

namespace Chromely.CefGlue.Winapi
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ChromelyConfigurationExtension
    {
        public static ChromelyConfiguration WithHostCustomStyle(this ChromelyConfiguration configuration, WindowCreationStyle customStyle)
        {
            if (configuration != null)
            {
                configuration.HostCustomCreationStyle = customStyle;
            }

            return configuration;
        }
    }
}
