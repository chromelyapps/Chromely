
namespace Chromely.AspNetCore.Mvc.Models;

public class MovieGenreViewModel
{
    public List<Movie>? Movies { get; set; }
    public SelectList? Genres { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? MovieGenre { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }
}