using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Chromely.Tests.ChromelyCore
{
    public class ChromelyControllerTest
    {
        private readonly TestController _testController;
        private readonly Dictionary<string, ActionRoute> _actionRouteDictionary;
        private readonly Dictionary<string, CommandRoute> _commandRouteDictionary;

        public ChromelyControllerTest()
        {
            _testController = new TestController();
            _actionRouteDictionary = _testController.ActionRouteDictionary;
            _commandRouteDictionary = _testController.CommandRouteDictionary;
        }


        [Fact]
        public void IsControllerTest()
        {
            Assert.NotNull(_testController);
            Assert.True(_testController != null);
            Assert.True(Attribute.IsDefined(_testController.GetType(), typeof(ControllerPropertyAttribute)));
        }

        /// <summary>
        /// The route count test.
        /// </summary>
        [Fact]
        public void RouteCountTest()
        {
            BaseTest();

            Assert.Equal(3, _actionRouteDictionary.Count);
            Assert.Single(_commandRouteDictionary);
        }

        [Fact]
        public void GetRouteCountTest()
        {
            BaseTest();

            var values = _actionRouteDictionary.Values;
            Assert.Equal(2, values.Count(x => x.Method == Method.GET));
        }
  
        [Fact]
        public void SaveRouteCountTest()
        {
            BaseTest();

            var values = _actionRouteDictionary.Values;

            Assert.Equal(1, values.Count(x => x.Method == Method.POST));
        }

        [Fact]
        public void Get1InvokeTest()
        {
            BaseTest();

            string routeKey = GetRouteKey(Method.GET, "/testcontroller/movies/1");
            var routeGet1 = _actionRouteDictionary[routeKey];
            Assert.NotNull(routeGet1);

            var response = routeGet1.Invoke(new ChromelyRequest(new RoutePath(Method.GET, "/testcontroller/movies/1"), null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is int);
            Assert.Equal(1000, (int)response.Data);
        }

     
        [Fact]
        public void Get2InvokeTest()
        {
            BaseTest();

            string routeKey = GetRouteKey(Method.GET, "/testcontroller/movies/2");
            var routeGet2 = _actionRouteDictionary[routeKey];
            Assert.NotNull(routeGet2);

            ChromelyResponse response = routeGet2.Invoke(new ChromelyRequest(new RoutePath(Method.GET, "/testcontroller/movies/2"), null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal("Test Get 2", (string)response.Data);
        }

        [Fact]
        public void SaveInvokeTest()
        {
            BaseTest();
            string routeKey = GetRouteKey(Method.POST, "/testcontroller/movies");
            var routeSave = _actionRouteDictionary[routeKey];
            Assert.NotNull(routeSave);

            var parameter = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var response = routeSave.Invoke(new ChromelyRequest(new RoutePath(Method.POST, "/testcontroller/movies"), null, parameter));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal(parameter, (string)response.Data);
        }

        [Fact]
        public void CommandTest()
        {
            BaseTest();
            string routeKey = CommandRoute.GetKeyFromPath("/testcontroller/cmd1");
            var commandRoute = _commandRouteDictionary[routeKey];
            Assert.NotNull(commandRoute);
        }

        private void BaseTest()
        {
            Assert.NotNull(_testController);
            Assert.NotNull(_actionRouteDictionary);
            Assert.NotNull(_commandRouteDictionary);
        }
      
        private string GetRouteKey(Method method, string path)
        {
            var routhPath = new RoutePath(method, path);
            return routhPath.Key;
        }
    }

    /// <summary>
    /// The test controller.
    /// </summary>
    [ControllerProperty(Name = "TestController", Route = "testcontroller")]
    public class TestController : ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestControllerGetSave"/> class.
        /// </summary>
        public TestController()
        {
            RegisterGetRequest("/testcontroller/movies/1", Get1);
            RegisterGetRequest("/testcontroller/movies/2", Get2);
            RegisterPostRequest("/testcontroller/movies", Save);
            RegisterCommand("/testcontroller/cmd1", Command1);
        }

        [Command(Route = "")]
        public void Command1(IDictionary<string, string> queryParameters)
        {
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