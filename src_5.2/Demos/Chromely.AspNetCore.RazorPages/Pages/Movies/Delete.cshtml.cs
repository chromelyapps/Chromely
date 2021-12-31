namespace Chromely.AspNetCore.RazorPages.Pages.Movies;

public class DeleteModel : PageModel
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public DeleteModel(IDbContextFactory<MovieContext> contextFactory)
    {
        _contextFactory = contextFactory;
        Movie = new();
    }

    [BindProperty]
    public Movie Movie { get; set; }

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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        using var context = _contextFactory.CreateDbContext();
        Movie = await context.Movie.FindAsync(id);

        if (Movie != null)
        {
            context.Movie.Remove(Movie);
            await context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}