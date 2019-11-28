namespace Chromely.Core.Helpers
{
    public enum CefEventKey
    {
        None,
        FrameLoadStart,
        AddressChanged,
        TitleChanged,
        FrameLoadEnd,
        LoadingStateChanged,
        ConsoleMessage,
        StatusMessage,
        LoadError,
        TooltipChanged,
        BeforeClose,
        BeforePopup,
        PluginCrashed,
        RenderProcessTerminated
    }
}
