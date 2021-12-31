
namespace Chromely.AspNetCore.RazorPages.Pages;

public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Objective { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Platform { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Version { get; set; }


    private readonly IChromelyInfo _chromelyInfo;

    public IndexModel(IChromelyInfo chromelyInfo)
    {
        _chromelyInfo = chromelyInfo;
    }

    public void OnGet()
    {
        var infoDc = _chromelyInfo.GetInfo() as IDictionary<string, string>;
        if (infoDc != null)
        {
            Objective = infoDc.ContainsKey("divObjective") ? infoDc["divObjective"] : Objective;
            Platform = infoDc.ContainsKey("divPlatform") ? infoDc["divPlatform"] : Platform;
            Version = infoDc.ContainsKey("divVersion") ? infoDc["divVersion"] : Version;
        }
    }
}