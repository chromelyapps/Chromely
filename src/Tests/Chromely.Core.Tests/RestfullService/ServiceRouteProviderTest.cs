namespace Chromely.Core.Tests.RestfullService
{
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    public class ServiceRouteProviderTest
    {
        private readonly ITestOutputHelper m_testOutput;
        private readonly TestController m_testController;

        public ServiceRouteProviderTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
            m_testController = new TestController();
        }

        [Fact]
        public void RouteProviderTest()
        {
            var routeDict = BaseTest();
            Assert.Equal(3, routeDict.Count);

            foreach (var item in routeDict)
            {
                ServiceRouteProvider.AddRoute(item.Key, item.Value);
            }

            foreach (var item in routeDict)
            {
                var route = ServiceRouteProvider.GetRoute(item.Key);
                Assert.NotNull(route);
            }
        }

        private Dictionary<string, Route> BaseTest()
        {
            Assert.NotNull(m_testController);
            var routeDict = m_testController.RouteDictionary;
            Assert.NotNull(routeDict);

            return routeDict;
        }

        [ControllerProperty(Name = "TestController", Route = "testcontroller")]
        public class TestController : ChromelyController
        {
            public TestController()
            {
                RegisterGetRequest("/testcontroller/get1", Get1);
                RegisterGetRequest("/testcontroller/get2", Get2);
                RegisterPostRequest("/testcontroller/save", Save);
            }

            private ChromelyResponse Get1(ChromelyRequest request)
            {
                ChromelyResponse response = new ChromelyResponse();
                response.Data = 1000;
                return response;
            }

            private ChromelyResponse Get2(ChromelyRequest request)
            {
                ChromelyResponse response = new ChromelyResponse();
                response.Data = "Test Get 2";
                return response;
            }

            private ChromelyResponse Save(ChromelyRequest request)
            {
                ChromelyResponse response = new ChromelyResponse();
                response.Data = request.PostData;
                return response;
            }
        }
    }
}
