namespace Chromely.Shared.Models;

public class MoviesViewModel
{
    public List<Movie>? Movies { get; set; }
    public List<string>? Genres { get; set; }
    public string? MovieGenre { get; set; }
    public string? SearchString { get; set; }
}