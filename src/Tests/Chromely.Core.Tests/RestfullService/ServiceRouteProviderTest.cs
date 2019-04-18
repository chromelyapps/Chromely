// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProviderTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
            mTestOutput = testOutput;
            mTestController = new TestController();
        }

        /// <summary>
        /// The route provider test.
        /// </summary>
        [Fact]
        public void RouteProviderTest()
        {
            var routeDict = BaseTest();
            Assert.Equal(3, routeDict.Count);

            foreach (var item in routeDict)
            {
                ServiceRouteProvider.AddRoute(item.Key, item.Value);
            }

            var getRoute1 = ServiceRouteProvider.GetRoute(new RoutePath(Method.GET, "/testcontroller/get1"));
            var getRoute2 = ServiceRouteProvider.GetRoute(new RoutePath(Method.GET, "/testcontroller/get2"));
            var postRoute = ServiceRouteProvider.GetRoute(new RoutePath(Method.POST, "/testcontroller/save"));
            Assert.True((getRoute1 != null) || (getRoute2 != null) || (postRoute != null));
        }

        /// <summary>
        /// The base test.
        /// </summary>
        /// <returns>
        /// The dictionary of routers.
        /// </returns>
        private Dictionary<string, Route> BaseTest()
        {
            Assert.NotNull(mTestController);
            var routeDict = mTestController.RouteDictionary;
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
                RegisterGetRequest("/testcontroller/get1", Get1);
                RegisterGetRequest("/testcontroller/get2", Get2);
                RegisterPostRequest("/testcontroller/save", Save);
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
