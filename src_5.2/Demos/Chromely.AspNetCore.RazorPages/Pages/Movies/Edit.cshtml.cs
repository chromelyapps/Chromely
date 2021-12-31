namespace Chromely.AspNetCore.RazorPages.Pages.Movies;

public class EditModel : PageModel
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public EditModel(IDbContextFactory<MovieContext> contextFactory)
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

    #region snippet
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using var context = _contextFactory.CreateDbContext();
        context.Attach(Movie).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(context, Movie.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool MovieExists(MovieContext context, int id)
    {
        return context.Movie.Any(e => e.Id == id);
    }
    #endregion
}