// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScannerTest.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Tests
{
    using System.Reflection;
    using Chromely.Core.RestfulService;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The route scanner test.
    /// </summary>
    public class RouteScannerTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScannerTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public RouteScannerTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
        }

        /// <summary>
        /// The scan test.
        /// </summary>
        [Fact]
        public void ScanTest()
        {
            // Note that the current assembly scan will include this file
            // And all other routes defined in other files in the assembly
            var scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            Assert.NotNull(scanner);

            var result = scanner.Scan();
            Assert.NotNull(result);

            Assert.Equal(9, result.Count);
        }

        /// <summary>
        /// The scanner controller.
        /// </summary>
        [ControllerProperty(Name = "ScannerController", Route = "scannercontroller")]
        public class ScannerController : ChromelyController
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScannerController"/> class.
            /// </summary>
            public ScannerController()
            {
                this.RegisterGetRequest("/scannercontroller/get1", this.Get1);
                this.RegisterGetRequest("/scannercontroller/get2", this.Get2);
                this.RegisterPostRequest("/scannercontroller/save", this.Save);
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
}
