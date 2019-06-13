// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProviderTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.Core.Tests.RestfulService
{
    /// <summary>
    /// The service route provider test.
    /// </summary>
    public class ServiceRouteProviderTest
    {
        /// <summary>
        /// The test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// The test controller.
        /// </summary>
        private readonly TestControllerGetSave mTestController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRouteProviderTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public ServiceRouteProviderTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            mTestController = new TestControllerGetSave();
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
    }
}
