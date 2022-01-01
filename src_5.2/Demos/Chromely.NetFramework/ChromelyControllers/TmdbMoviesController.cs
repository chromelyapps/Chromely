// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Chromely;
using Chromely.Core.Configuration;
using Chromely.Core.Network;

namespace Chromely.NetFramework.Controllers
{
    [ChromelyController(Name = "TmdbMoviesController")]
    public class TmdbMoviesController : ChromelyController
    {
        private const string TmdbBaseUrl = "https://api.themoviedb.org/3/";

        private const string ChromelyTmdbApiKey = "4f457e870e91b76e02292d52a46fc445";

        private static string TmdbLatestUrl(string apiKey = ChromelyTmdbApiKey) => $"movie/latest?api_key={apiKey}&language=en-US";
        private static string TmdbPopularUrl(string apiKey = ChromelyTmdbApiKey) => $"movie/popular?api_key={apiKey}&language=en-US&page=1";
        private static string TmdbTopRatedUrl(string apiKey = ChromelyTmdbApiKey) => $"movie/top_rated?api_key={apiKey}&language=en-US&page=1";
        private static string TmdbNowPlayingUrl(string apiKey = ChromelyTmdbApiKey) => $"movie/now_playing?api_key={apiKey}&language=en-US&page=1";
        private static string TmdbUpcomingUrl(string apiKey = ChromelyTmdbApiKey) => $"movie/upcoming?api_key={apiKey}&language=en-US&page=1";
        private static string TmdbSearchUrl(string queryValue, string apiKey = ChromelyTmdbApiKey) => $"search/movie?api_key={apiKey}&query={queryValue}&language=en-US&page=1&include_adult=false";
        private static string TmdbGetMovieUrl(string movieId, string apiKey = ChromelyTmdbApiKey) => $"movie/{movieId}?api_key={apiKey}";

        private readonly IChromelyConfiguration _config;

        public TmdbMoviesController(IChromelyConfiguration config)
        {
            _config = config;
        }

        [ChromelyRoute(Path = "/tmdbmoviescontroller/movies")]
        public List<Result> GetMovies(string name, string query)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Result>();
            }

            if (name.Equals("search") && string.IsNullOrWhiteSpace(query))
            {
                return new List<Result>();
            }

            var paramUrl = string.Empty;
            switch (name.ToLower())
            {
                case "latest":
                    paramUrl = TmdbLatestUrl();
                    break;
                case "popular":
                    paramUrl = TmdbPopularUrl();
                    break;
                case "toprated":
                    paramUrl = TmdbTopRatedUrl();
                    break;
                case "nowplaying":
                    paramUrl = TmdbNowPlayingUrl();
                    break;
                case "upcoming":
                    paramUrl = TmdbUpcomingUrl();
                    break;
                case "search":
                    paramUrl = TmdbSearchUrl(query);
                    break;
            }

            var tmdbMoviesTask = Task.Run(() =>
            {
                return GetTmdbMovieListAsync(paramUrl);
            });

            tmdbMoviesTask.Wait();

            List<Result> movies = new List<Result>();
            var tmdMovieInfo = tmdbMoviesTask.Result;

            if (tmdbMoviesTask.Result != null)
            {
                movies = tmdbMoviesTask.Result.results;
            }

            return movies;
        }

        [ChromelyRoute(Path = "/tmdbmoviescontroller/homepage")]
        public void HomePage(string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId))
            {
                return;
            }

            var tmdbMovieTask = Task.Run(() =>
            {
                return GetTmdbMovieAsync(movieId);
            });

            tmdbMovieTask.Wait();

            var movie = tmdbMovieTask.Result;
            if (movie != null && !string.IsNullOrWhiteSpace(movie.homepage))
            {
                BrowserLauncher.Open(_config.Platform, movie.homepage);
            }
        }

        private async Task<TmdMoviesInfo> GetTmdbMovieListAsync(string paramUrl)
        {
            var baseAddress = new Uri(TmdbBaseUrl);
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var response = await httpClient.GetAsync(paramUrl))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;

                    return JsonSerializer.Deserialize<TmdMoviesInfo>(responseData, options);
                }
            }
        }

        private async Task<TmdMovie> GetTmdbMovieAsync(string movieId)
        {
            var baseAddress = new Uri(TmdbBaseUrl);
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var response = await httpClient.GetAsync(TmdbGetMovieUrl(movieId)))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;

                    return JsonSerializer.Deserialize<TmdMovie>(responseData, options);
                }
            }
        }
    }

    public class Result
    {
        public double popularity { get; set; }
        public int vote_count { get; set; }
        public bool video { get; set; }
        public string poster_path { get; set; }
        public int id { get; set; }
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public List<int> genre_ids { get; set; }
        public string title { get; set; }
        public double vote_average { get; set; }
        public string overview { get; set; }
        private string _releaseDate;

        public string release_date
        {
            get
            {
                DateTime dateTime;
                if (DateTime.TryParse(_releaseDate, out dateTime))
                {
                    return string.Format("{0:dddd, MMMM d, yyyy}", dateTime);
                }

                return _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }
        }
    }

    public class TmdMoviesInfo
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<Result> results { get; set; }
    }

    public class TmdMovie
    {
        public int id { get; set; }
        public string homepage { get; set; }
    }
}