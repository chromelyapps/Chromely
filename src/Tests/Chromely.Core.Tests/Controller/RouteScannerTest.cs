// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScannerTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using Chromely.Core.RestfulService;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.Core.Tests.Controller
{
    /// <summary>
    /// The route scanner test.
    /// </summary>
    public class RouteScannerTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScannerTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public RouteScannerTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        /// <summary>
        /// The scanner should find all controller in test assembly.
        /// </summary>
        [Fact]
        public void ScannerShouldFindAllRoutersInTestAssembly()
        {
            // Note that the current assembly scan will include this file
            // And all other routes defined in other files in the assembly
            var scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            Assert.NotNull(scanner);

            var result = scanner.Scan().Item1;
            Assert.NotNull(result);

            Assert.Equal(9, result.Count);
        }
    }
}
