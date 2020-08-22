// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

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
        private readonly Dictionary<string, RequestActionRoute> _requestActionRouteDictionary;
        private readonly Dictionary<string, CommandActionRoute> _commandActionRouteDictionary;

        public ChromelyControllerTest()
        {
            _testController = new TestController();
            _requestActionRouteDictionary = _testController.ActionRouteDictionary;
            _commandActionRouteDictionary = _testController.CommandRouteDictionary;
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

            Assert.Equal(3, _requestActionRouteDictionary.Count);
            Assert.Single(_commandActionRouteDictionary);
        }

        [Fact]
        public void GetRouteCountTest()
        {
            BaseTest();

            var values = _requestActionRouteDictionary.Values;
            Assert.Equal(3, values.Count());
        }

        [Fact]
        public void Get1InvokeTest()
        {
            BaseTest();

            string routeKey = RouteKey.CreateRequestKey("/testcontroller/movies/1");
            var routeGet1 = _requestActionRouteDictionary[routeKey];
            Assert.NotNull(routeGet1);

            var response = routeGet1.Invoke(new ChromelyRequest("/testcontroller/movies/1", null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is int);
            Assert.Equal(1000, (int)response.Data);
        }

     
        [Fact]
        public void Get2InvokeTest()
        {
            BaseTest();

            string routeKey = RouteKey.CreateRequestKey("/testcontroller/movies/2");
            var routeGet2 = _requestActionRouteDictionary[routeKey];
            Assert.NotNull(routeGet2);

            IChromelyResponse response = routeGet2.Invoke(new ChromelyRequest("/testcontroller/movies/2", null, null));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal("Test Get 2", (string)response.Data);
        }

        [Fact]
        public void SaveInvokeTest()
        {
            BaseTest();
            string routeKey = RouteKey.CreateRequestKey("/testcontroller/movies");
            var routeSave = _requestActionRouteDictionary[routeKey];
            Assert.NotNull(routeSave);

            var parameter = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var response = routeSave.Invoke(new ChromelyRequest("/testcontroller/movies", null, parameter));
            Assert.NotNull(response);
            Assert.True(response.Data is string);
            Assert.Equal(parameter, (string)response.Data);
        }

        [Fact]
        public void CommandTest()
        {
            BaseTest();
            string routeKey = RouteKey.CreateCommandKey("/testcontroller/cmd1");
            var commandRoute = _commandActionRouteDictionary[routeKey];
            Assert.NotNull(commandRoute);
        }

        private void BaseTest()
        {
            Assert.NotNull(_testController);
            Assert.NotNull(_requestActionRouteDictionary);
            Assert.NotNull(_commandActionRouteDictionary);
        }
    }

    /// <summary>
    /// The test controller.
    /// </summary>
    [ControllerProperty(Name = "TestController")]
    public class TestController : ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestControllerGetSave"/> class.
        /// </summary>
        public TestController()
        {
            RegisterRequest("/testcontroller/movies/1", Get1);
            RegisterRequest("/testcontroller/movies/2", Get2);
            RegisterRequest("/testcontroller/movies", Save);
            RegisterCommand("/testcontroller/cmd1", Command1);
        }

        [CommandAction(RouteKey = "")]
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
        /// The <see cref="IChromelyResponse"/>.
        /// </returns>
        private IChromelyResponse Get1(IChromelyRequest request)
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
        private IChromelyResponse Get2(IChromelyRequest request)
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
        /// The <see cref="IChromelyResponse"/>.
        /// </returns>
        private IChromelyResponse Save(IChromelyRequest request)
        {
            var response = new ChromelyResponse { Data = request.PostData };
            return response;
        }
    }
}