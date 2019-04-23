#pragma warning disable 1591
namespace Chromely.Core
{
    /// <summary>
    /// List of supported host interface apis.
    /// Attention! Not every api is supported on all platforms.
    /// </summary>
    public enum ChromelyHostApi
    {
        None,

        Winapi,
        Gtk,
        Libui
    }
}
