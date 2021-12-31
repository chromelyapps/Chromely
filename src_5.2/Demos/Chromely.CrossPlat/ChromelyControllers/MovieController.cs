// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Chromely.Core.Network;

namespace Chromely.CrossPlat.ChromelyControllers
{
    /// <summary>
    /// The movie controller.
    /// </summary>
    [ChromelyController(Name = "MovieController")]
    public class MovieController : ChromelyController
    {
        private readonly IChromelyConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        public MovieController(IChromelyConfiguration config)
        {
            _config = config;
        }


        [ChromelyRoute(Path = "/democontroller/showdevtools")]
        public void ShowDevTools()
        {
            if (_config != null && !string.IsNullOrWhiteSpace(_config.DevToolsUrl))
            {
                BrowserLauncher.Open(_config.Platform, _config.DevToolsUrl);
            }
        }

        [ChromelyRoute(Path = "/democontroller/movies/get")]
        public List<MovieInfo> GetMovies()
        {
            var movieInfos = new List<MovieInfo>();
            var assemblyName = typeof(MovieInfo).Assembly.GetName().Name;

            movieInfos.Add(new MovieInfo(id: 1, title: "The Shawshank Redemption", year: 1994, votes: 678790, rating: 9.2, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 2, title: "The Godfather", year: 1972, votes: 511495, rating: 9.2, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 3, title: "The Godfather: Part II", year: 1974, votes: 319352, rating: 9.0, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 4, title: "The Good, the Bad and the Ugly", year: 1966, votes: 213030, rating: 8.9, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 5, title: "My Fair Lady", year: 1964, votes: 533848, rating: 8.9, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 6, title: "12 Angry Men", year: 1957, votes: 164558, rating: 8.9, assembly: assemblyName));

            return movieInfos;
        }

        [ChromelyRoute(Path = "/democontroller/movies/post")]
        public string SaveMovies(List<MovieInfo> movies)
        {
            var rowsReceived = movies != null ? movies.Count : 0;
            return $"{DateTime.Now}: {rowsReceived} rows of data successfully saved.";
        }
    }

    /// <summary>
    /// The movie info.
    /// </summary>
    // ReSharper disable once StyleCop.SA1402
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("ReSharper", "StyleCop.SA1600")]
    public class MovieInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieInfo"/> class.
        /// </summary>
        public MovieInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieInfo"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <param name="votes">
        /// The votes.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public MovieInfo(int id, string title, int year, int votes, double rating, string assembly)
        {
            Id = id;
            Title = title;
            Year = year;
            Votes = votes;
            Rating = rating;
            Date = DateTime.Now;
            RestfulAssembly = assembly;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public int Votes { get; set; }

        public double Rating { get; set; }

        public DateTime Date { get; set; }

        public string RestfulAssembly { get; set; }
    }
}
