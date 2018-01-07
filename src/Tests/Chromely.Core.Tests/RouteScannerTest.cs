namespace Chromely.Core.Tests
{
    using Chromely.Core.RestfulService;
    using System.Reflection;
    using Xunit;
    using Xunit.Abstractions;

    public class RouteScannerTest
    {
        private readonly ITestOutputHelper m_testOutput;

        public RouteScannerTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
        }

        [Fact]
        public void ScanTest()
        {
            // Note that the current assembly scan will include this file
            // And all other routes defined in other files in the assembly

            RouteScanner scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            Assert.NotNull(scanner);

            var result = scanner.Scan();
            Assert.NotNull(result);

            Assert.Equal(6, result.Count);
        }

        [ControllerProperty(Name = "ScannerController", Route = "scannercontroller")]
        public class ScannerController : ChromelyController
        {
            public ScannerController()
            {
                RegisterGetRequest("/scannercontroller/get1", Get1);
                RegisterGetRequest("/scannercontroller/get2", Get2);
                RegisterPostRequest("/scannercontroller/save", Save);
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
