/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.CefGlueApp.Demo.Controllers
{
    using System;
    using System.Collections.Generic;
    using Chromely.Core.RestfulService;

    [ControllerProperty(Name = "DemoController", Route = "democontroller")]
    public class DemoController : ChromelyController
    {
        public DemoController()
        {
            RegisterGetRequest("/democontroller/movies", GetMovies);
            RegisterPostRequest("/democontroller/savemovies", SaveMovies);
        }

        private ChromelyResponse GetMovies(ChromelyRequest request)
        {

            List<MovieInfo> movieInfos = new List<MovieInfo>();
            string assemblyName = typeof(MovieInfo).Assembly.GetName().Name;

            movieInfos.Add(new MovieInfo(id: 1, title: "The Shawshank Redemption", year: 1994, votes: 678790, rating: 9.2, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 2, title: "The Godfather", year: 1972, votes: 511495, rating: 9.2, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 3, title: "The Godfather: Part II", year: 1974, votes: 319352, rating: 9.0, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 4, title: "The Good, the Bad and the Ugly", year: 1966, votes: 213030, rating: 8.9, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 5, title: "My Fair Lady", year: 1964, votes: 533848, rating: 8.9, assembly: assemblyName));
            movieInfos.Add(new MovieInfo(id: 6, title: "12 Angry Men", year: 1957, votes: 164558, rating: 8.9, assembly: assemblyName));

            ChromelyResponse response = new ChromelyResponse();
            response.Data = movieInfos;
            return response;
        }

        private ChromelyResponse SaveMovies(ChromelyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request null argument!");
            }

            if (request.PostData == null)
            {
                throw new Exception("Post data is null or invalid.");
            }

            ChromelyResponse response = new ChromelyResponse();
            var postDataJson = request.PostData.EnsureJson();
            int rowsReceived = postDataJson.ArrayCount();

            response.Data = string.Format("{0} rows of data successfully saved.", rowsReceived);

            return response;
        }
    }

    public class MovieInfo
    {
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
