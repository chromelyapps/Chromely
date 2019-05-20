namespace Chromely.Core.Tests.Controller
{
    public class TestDependency : ITestDependency
    {
        public const string TestDependencyResponse = "Dependency Get 2";

        public string ReturnTest()
        {
            return TestDependencyResponse;
        }
    }
}