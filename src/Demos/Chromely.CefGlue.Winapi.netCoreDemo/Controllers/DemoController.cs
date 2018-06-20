// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoController.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once StyleCop.SA1300
namespace Chromely.CefGlue.Winapi.netCoreDemo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Chromely.Core.RestfulService;

    /// <summary>
    /// The demo controller.
    /// </summary>
    [ControllerProperty(Name = "DemoController", Route = "democontroller")]
    public class DemoController : ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoController"/> class.
        /// </summary>
        public DemoController()
        {
            this.RegisterGetRequest("/democontroller/movies", this.GetMovies);
            this.RegisterPostRequest("/democontroller/movies", this.SaveMovies);
        }

        /// <summary>
        /// The get movies.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
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

            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.Data = movieInfos;
            return response;
        }

        /// <summary>
        /// The save movies.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// ArgumentNullException - request is null exception.
        /// </exception>
        /// <exception cref="Exception">
        /// Exception - post data is null exception.
        /// </exception>
        private ChromelyResponse SaveMovies(ChromelyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.PostData == null)
            {
                throw new Exception("Post data is null or invalid.");
            }

            ChromelyResponse response = new ChromelyResponse(request.Id);
            var postDataJson = request.PostData.EnsureJson();
            int rowsReceived = postDataJson.ArrayCount();

            response.Data = $"{DateTime.Now}: {rowsReceived} rows of data successfully saved.";

            return response;
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
            this.Id = id;
            this.Title = title;
            this.Year = year;
            this.Votes = votes;
            this.Rating = rating;
            this.Date = DateTime.Now;
            this.RestfulAssembly = assembly;
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
