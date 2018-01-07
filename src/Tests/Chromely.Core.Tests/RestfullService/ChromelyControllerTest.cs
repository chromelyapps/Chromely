namespace Chromely.Core.Tests.RestfullService
{
    using Chromely.Core.RestfulService;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class ChromelyControllerTest
    {
        private readonly ITestOutputHelper m_testOutput;
        private readonly TestController m_testController;

        public ChromelyControllerTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
            m_testController = new TestController();
        }

        [Fact]
        public void IsControllerTest()
        {
            Assert.NotNull(m_testController);
            Assert.True(m_testController is ChromelyController);
            Assert.True(Attribute.IsDefined(m_testController.GetType(), typeof(ControllerPropertyAttribute)));
        }

        [Fact]
        public void RouteCountTest()
        {
            var routeDict = BaseTest();

            Assert.Equal(3, routeDict.Count);
        }

        [Fact]
        public void GetRouteCountTest()
        {
            var routeDict = BaseTest();

            var values = routeDict.Values;
            Assert.Equal(2, values.Count(x => x.Method == Method.GET));
        }

        [Fact]
        public void SaveRouteCountTest()
        {
            var routeDict = BaseTest();

            var values = routeDict.Values;
            Assert.Equal(1, values.Count(x => x.Method == Method.POST));
        }

        [Fact]
        public void Get1InvokeTest()
        {
            var routeDict = BaseTest();
            Route routeGet1 = routeDict["/testcontroller/get1"];
            Assert.NotNull(routeGet1);

            ChromelyResponse response = routeGet1.Invoke(new ChromelyRequest("/testcontroller/get1", null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is int);
            Assert.Equal(1000, (int)(response.Data));
        }

        [Fact]
        public void Get2InvokeTest()
        {
            var routeDict = BaseTest();
            Route routeGet2 = routeDict["/testcontroller/get2"];
            Assert.NotNull(routeGet2);

            ChromelyResponse response = routeGet2.Invoke(new ChromelyRequest("/testcontroller/get2", null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal("Test Get 2", (string)(response.Data));
        }

        [Fact]
        public void SaveInvokeTest()
        {
            var routeDict = BaseTest();
            Route routeSave = routeDict["/testcontroller/save"];
            Assert.NotNull(routeSave);

            string parameter = DateTime.Now.ToString();
            ChromelyResponse response = routeSave.Invoke(new ChromelyRequest("/testcontroller/save", null, parameter));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal(parameter, (string)(response.Data));
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
