#define  SearchGenreAndString   //SearchString
#region snippet_newProps

namespace Chromely.AspNetCore.RazorPages.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly IDbContextFactory<MovieContext> _contextFactory;

    public IndexModel(IDbContextFactory<MovieContext> contextFactory)
    {
        _contextFactory = contextFactory;
        Movie = new List<Movie>();
    }

    public IList<Movie> Movie { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }
    // Requires using Microsoft.AspNetCore.Mvc.Rendering;
    public SelectList? Genres { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? MovieGenre { get; set; }
    #endregion

#if SearchString
    #region snippet_1stSearch
        public async Task OnGetAsync()
        {
            var movies = _context.Movie;
    #region snippet_SearchNull
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }
    #endregion

            Movie = await movies.ToListAsync();
        }
    #endregion
#endif

#if Original
        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
#endif
#if SearchGenreAndString
    #region snippet_SearchGenre
    public async Task OnGetAsync()
    {
        using var context = _contextFactory.CreateDbContext();

        #region snippet_LINQ
        // Use LINQ to get list of genres.
        IQueryable<string> genreQuery = from m in context.Movie
                                        orderby m.Genre
                                        select m.Genre;
        #endregion

        var movies = from m in context.Movie
                     select m;

        if (!string.IsNullOrEmpty(SearchString))
        {
            movies = movies.Where(s => !string.IsNullOrEmpty(s.Title) && s.Title.Contains(SearchString));
        }

        if (!string.IsNullOrEmpty(MovieGenre))
        {
            movies = movies.Where(x => x.Genre == MovieGenre);
        }
        #region snippet_SelectList
        Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
        #endregion
        Movie = await movies.ToListAsync();
    }
    #endregion
#endif
}