// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyInjectionTests.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xunit;

namespace Chromely.Core.Tests.Controller
{
    public class DependencyInjectionTests
    {
        /// <summary>
        /// The scanner should find all controller in test assembly.
        /// </summary>
        [Fact]
        public void ScannerShouldInstantiateScannerControllerWithInjectedDependencies()
        {
            const string TestRoute = "/scannercontroller/get2";
            
            var test = new TestDependency();
            IoC.RegisterInstance<ITestDependency>(nameof(ITestDependency), test);

            var logger = new TestLogger(); 
            IoC.RegisterInstance<IChromelyLogger>(nameof(IChromelyLogger), logger);

            var scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            var routeCommands = scanner.Scan();
            foreach (var route in routeCommands.Item1)
            {
                ServiceRouteProvider.AddRoute(route.Key, route.Value);
            }

            foreach (var command in routeCommands.Item2)
            {
                ServiceRouteProvider.AddCommand(command.Key, command.Value);
            }

            var request = new ChromelyRequest(new RoutePath(Method.GET, TestRoute), null, null);

            var routePath = new RoutePath(Method.GET, TestRoute);
            var get2 = ServiceRouteProvider.GetRoute(routePath);
            var getResponse = get2.Invoke(request);

            Assert.Equal(TestDependency.TestDependencyResponse, getResponse.Data.ToString());
            Assert.Equal(ScannerControllerWithDependencyInjection.Get2Response, logger.Message);
        }
    }
}