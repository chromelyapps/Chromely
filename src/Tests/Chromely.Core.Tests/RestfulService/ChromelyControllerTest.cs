// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyControllerTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chromely.Core.RestfulService;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.Core.Tests.RestfulService
{
    /// <summary>
    /// The chromely controller test.
    /// </summary>
    public class ChromelyControllerTest
    {
        /// <summary>
        /// The test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// The test controller.
        /// </summary>
        private readonly TestControllerMovies _testControllerMovies;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyControllerTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ChromelyControllerTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            _testControllerMovies = new TestControllerMovies();
        }

        /// <summary>
        /// The is controller test.
        /// </summary>
        [Fact]
        public void IsControllerTest()
        {
            Assert.NotNull(_testControllerMovies);
            Assert.True(_testControllerMovies != null);
            Assert.True(Attribute.IsDefined(_testControllerMovies.GetType(), typeof(ControllerPropertyAttribute)));
        }

        /// <summary>
        /// The route count test.
        /// </summary>
        [Fact]
        public void RouteCountTest()
        {
            var routeDict = BaseTest();

            Assert.Equal(3, routeDict.Count);
        }

        /// <summary>
        /// The get route count test.
        /// </summary>
        [Fact]
        public void GetRouteCountTest()
        {
            var routeDict = BaseTest();

            var values = routeDict.Values;
            Assert.Equal(2, values.Count(x => x.Method == Method.GET));
        }

        /// <summary>
        /// The save route count test.
        /// </summary>
        [Fact]
        public void SaveRouteCountTest()
        {
            var routeDict = BaseTest();
            var values = routeDict.Values;

            Assert.Equal(1, values.Count(x => x.Method == Method.POST));
        }

        /// <summary>
        /// The get 1 invoke test.
        /// </summary>
        [Fact]
        public void Get1InvokeTest()
        {
            var routeDict = BaseTest();
            string routeKey = GetRouteKey(Method.GET, "/testcontroller/movies");
            var routeGet1 = routeDict[routeKey];
            Assert.NotNull(routeGet1);

            var response = routeGet1.Invoke(new ChromelyRequest(new RoutePath(Method.GET, "/testcontroller/movies"), null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is int);
            Assert.Equal(1000, (int)response.Data);
        }

        /// <summary>
        /// The get 2 invoke test.
        /// </summary>
        [Fact]
        public void Get2InvokeTest()
        {
            var routeDict = BaseTest();
            string routeKey = GetRouteKey(Method.GET, "/testcontroller/sitcoms");
            var routeGet2 = routeDict[routeKey];
            Assert.NotNull(routeGet2);

            ChromelyResponse response = routeGet2.Invoke(new ChromelyRequest(new RoutePath(Method.GET, "/testcontroller/sitcoms"), null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal("Test Get 2", (string)response.Data);
        }

        /// <summary>
        /// The save invoke test.
        /// </summary>
        [Fact]
        public void SaveInvokeTest()
        {
            var routeDict = BaseTest();
            string routeKey = GetRouteKey(Method.POST, "/testcontroller/movies");
            var routeSave = routeDict[routeKey];
            Assert.NotNull(routeSave);

            var parameter = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var response = routeSave.Invoke(new ChromelyRequest(new RoutePath(Method.POST, "/testcontroller/save"), null, parameter));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal(parameter, (string)response.Data);
        }

        /// <summary>
        /// The base test.
        /// </summary>
        /// <returns>
        /// The dictionary of routers.
        /// </returns>
        private Dictionary<string, Route> BaseTest()
        {
            Assert.NotNull(_testControllerMovies);
            var routeDict = _testControllerMovies.RouteDictionary;
            Assert.NotNull(routeDict);

            return routeDict;
        }

        /// <summary>
        /// The get route key.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetRouteKey(Method method, string path)
        {
            var routhPath = new RoutePath(method, path);
            return routhPath.Key;
        }
    }
}
