namespace Chromely.AspNetCore.RazorPages.Pages.Movies;

public class DetailsModel : PageModel
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public DetailsModel(IDbContextFactory<MovieContext> contextFactory)
    {
        _contextFactory = contextFactory;
        Movie = new();
    }

    public Movie Movie { get; set; }

    #region snippet1
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        using var context = _contextFactory.CreateDbContext();
        Movie = await context.Movie.FirstOrDefaultAsync(m => m.Id == id);

        if (Movie == null)
        {
            return NotFound();
        }
        return Page();
    }
    #endregion
}