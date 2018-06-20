// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProviderTest.cs" company="Chromely">
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
    using System.Collections.Generic;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The service route provider test.
    /// </summary>
    public class ServiceRouteProviderTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// The m test controller.
        /// </summary>
        private readonly TestController mTestController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRouteProviderTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ServiceRouteProviderTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
            this.mTestController = new TestController();
        }

        /// <summary>
        /// The route provider test.
        /// </summary>
        [Fact]
        public void RouteProviderTest()
        {
            var routeDict = this.BaseTest();
            Assert.Equal(3, routeDict.Count);

            foreach (var item in routeDict)
            {
                ServiceRouteProvider.AddRoute(item.Key, item.Value);
            }

            foreach (var item in routeDict)
            {
                var getRoute = ServiceRouteProvider.GetRoute(new RoutePath(Method.GET, item.Key));
                var postRoute = ServiceRouteProvider.GetRoute(new RoutePath(Method.GET, item.Key));
                Assert.True((getRoute != null) || (postRoute != null));
            }
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
                this.RegisterGetRequest("/testcontroller/get1", this.Get1);
                this.RegisterGetRequest("/testcontroller/get2", this.Get2);
                this.RegisterPostRequest("/testcontroller/save", this.Save);
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
                ChromelyResponse response = new ChromelyResponse();
                response.Data = 1000;
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
                ChromelyResponse response = new ChromelyResponse();
                response.Data = "Test Get 2";
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
                ChromelyResponse response = new ChromelyResponse();
                response.Data = request.PostData;
                return response;
            }
        }
    }
}
