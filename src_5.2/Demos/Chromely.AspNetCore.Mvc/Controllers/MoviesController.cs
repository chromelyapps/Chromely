
namespace Chromely.AspNetCore.Mvc.Controllers;

public class MoviesController : Controller
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public MoviesController(IDbContextFactory<MovieContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // GET: Movies
    public async Task<IActionResult> Index(string movieGenre, string searchString)
    {
        using var context = _contextFactory.CreateDbContext();

        // Use LINQ to get list of genres.
        IQueryable<string> genreQuery = from m in context.Movie
                                    orderby m.Genre
                                    select m.Genre;

        var movies = from m in context.Movie
                     select m;

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => !string.IsNullOrEmpty(s.Title) && s.Title.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        var genres = new SelectList(await genreQuery.Distinct().ToListAsync());
        var moviesList = await movies.ToListAsync();

        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = genres,
            Movies = moviesList
        };

        return View(movieGenreVM);
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        using var context = _contextFactory.CreateDbContext();

        var movie = await context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            using var context = _contextFactory.CreateDbContext();

            context.Add(movie);
            await context.SaveChangesAsync();
            var redirectResult = RedirectToAction(nameof(Index));
            return redirectResult;
        }

        return View(movie);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        using var context = _contextFactory.CreateDbContext();

        var movie = await context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            using var context = _contextFactory.CreateDbContext();

            try
            {
                context.Update(movie);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(context, movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        using var context = _contextFactory.CreateDbContext();

        var movie = await context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        using var context = _contextFactory.CreateDbContext();

        var movie = await context.Movie.FindAsync(id);
        if (movie != null)
        {
            context.Movie.Remove(movie);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(MovieContext context, int id)
    {
        return context.Movie.Any(e => e.Id == id);
    }
}