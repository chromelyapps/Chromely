
namespace Chromely.Shared.ChromelyControllers;

[ChromelyController(Name = "WindowController")]
public class CommonController : ChromelyController
{
    private readonly IChromelyConfiguration _config;
    private readonly IChromelyWindow _window;

    public CommonController(IChromelyConfiguration config, IChromelyWindow window)
    {
        _config = config;
        _window = window;
    }

    [ChromelyRoute(Path = "/browser/reload")]
    public void Reload()
    {
        var scriptExecutor = _config?.JavaScriptExecutor;
        if (scriptExecutor != null)
        {
            Xilium.CefGlue.CefBrowser? browser = scriptExecutor?.GetBrowser() as Xilium.CefGlue.CefBrowser;
            if (browser != null)
            {
                browser.Reload();
            }
        }
    }

    [ChromelyRoute(Path = "/window/close")]
    public void Close()
    {
        _window?.Close();
    }
}