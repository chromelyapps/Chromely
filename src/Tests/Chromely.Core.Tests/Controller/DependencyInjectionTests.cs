using System.Reflection;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using Xunit;
// ReSharper disable StringLiteralTypo

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
            const string testRoute = "/scannercontroller/get2";
            
            var test = new TestDependency();
            IoC.RegisterInstance<ITestDependency>(nameof(ITestDependency), test);

            var logger = new TestLogger(); 
            IoC.RegisterInstance<IChromelyLogger>(nameof(IChromelyLogger), logger);

            var scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            var routes = scanner.Scan();
            foreach (var route in routes)
            {
                ServiceRouteProvider.AddRoute(route.Key, route.Value);
            }
            
            var request = new ChromelyRequest(new RoutePath(Method.GET, testRoute), null, null);

            var routePath = new RoutePath(Method.GET, testRoute);
            var get2 = ServiceRouteProvider.GetRoute(routePath);
            var getResponse = get2.Invoke(request);

            Assert.Equal(TestDependency.TestDependencyResponse, getResponse.Data.ToString());
            Assert.Equal(ScannerControllerWithDependencyInjection.Get2Response, logger.Message);
        }
    }
}