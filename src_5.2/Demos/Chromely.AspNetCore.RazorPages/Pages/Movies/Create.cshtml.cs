namespace Chromely.AspNetCore.RazorPages.Pages.Movies;

public class CreateModel : PageModel
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public CreateModel(IDbContextFactory<MovieContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Movie? Movie { get; set; }

    #region snippet
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using var context = _contextFactory.CreateDbContext();
        context.Movie.Add(Movie);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
    #endregion
}