// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDependency.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

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