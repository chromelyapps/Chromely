// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyControllerTest.cs" company="Chromely">
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

namespace Chromely.Core.Tests.RestfullService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chromely.Core.RestfulService;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The chromely controller test.
    /// </summary>
    public class ChromelyControllerTest
    {
        /// <summary>
        /// The m test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// The m test controller.
        /// </summary>
        private readonly TestController mTestController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyControllerTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ChromelyControllerTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
            this.mTestController = new TestController();
        }

        /// <summary>
        /// The is controller test.
        /// </summary>
        [Fact]
        public void IsControllerTest()
        {
            Assert.NotNull(this.mTestController);
            Assert.True(this.mTestController != null);
            Assert.True(Attribute.IsDefined(this.mTestController.GetType(), typeof(ControllerPropertyAttribute)));
        }

        /// <summary>
        /// The route count test.
        /// </summary>
        [Fact]
        public void RouteCountTest()
        {
            var routeDict = this.BaseTest();

            Assert.Equal(3, routeDict.Count);
        }

        /// <summary>
        /// The get route count test.
        /// </summary>
        [Fact]
        public void GetRouteCountTest()
        {
            var routeDict = this.BaseTest();

            var values = routeDict.Values;
            Assert.Equal(2, values.Count(x => x.Method == Method.GET));
        }

        /// <summary>
        /// The save route count test.
        /// </summary>
        [Fact]
        public void SaveRouteCountTest()
        {
            var routeDict = this.BaseTest();
            var values = routeDict.Values;

            Assert.Equal(1, values.Count(x => x.Method == Method.POST));
        }

        /// <summary>
        /// The get 1 invoke test.
        /// </summary>
        [Fact]
        public void Get1InvokeTest()
        {
            var routeDict = this.BaseTest();
            string routeKey = this.GetRouteKey(Method.GET, "/testcontroller/movies");
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
            var routeDict = this.BaseTest();
            string routeKey = this.GetRouteKey(Method.GET, "/testcontroller/sitcoms");
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
            var routeDict = this.BaseTest();
            string routeKey = this.GetRouteKey(Method.POST, "/testcontroller/movies");
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
            Assert.NotNull(this.mTestController);
            var routeDict = this.mTestController.RouteDictionary;
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

        /// <summary>
        /// The test controller.
        /// </summary>
        [ControllerProperty(Name = "TestController", Route = "testcontroller")]
        public class TestController : ChromelyController
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestController"/> class.
            /// </summary>
            public TestController()
            {
                this.RegisterGetRequest("/testcontroller/movies", this.Get1);
                this.RegisterGetRequest("/testcontroller/sitcoms", this.Get2);
                this.RegisterPostRequest("/testcontroller/movies", this.Save);
            }

            /// <summary>
            /// The get 1.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Get1(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = 1000 };
                return response;
            }

            /// <summary>
            /// The get 2.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Get2(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = "Test Get 2" };
                return response;
            }

            /// <summary>
            /// The save.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Save(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = request.PostData };
                return response;
            }
        }
    }
}
