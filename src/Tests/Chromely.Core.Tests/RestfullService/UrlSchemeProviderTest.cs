// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSchemeProviderTest.cs" company="Chromely">
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

namespace Chromely.Core.Tests.RestfullService
{
    using Chromely.Core.Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The url scheme provider test.
    /// </summary>
    public class UrlSchemeProviderTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlSchemeProviderTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public UrlSchemeProviderTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
            this.Init();
        }

        /// <summary>
        /// The is custom url test.
        /// </summary>
        [Fact]
        public void IsCustomUrlTest()
        {
            Assert.True(UrlSchemeProvider.IsUrlOfRegisteredCustomScheme("http://chromely.com/getvaluefromme"));
            Assert.True(UrlSchemeProvider.IsUrlOfRegisteredCustomScheme("https://test.com/savevaluetome"));
        }

        /// <summary>
        /// The is external url test.
        /// </summary>
        [Fact]
        public void IsExternalUrlTest()
        {
            Assert.True(UrlSchemeProvider.IsUrlRegisteredExternal("https://www.google.com/getvaluefromme"));
            Assert.True(UrlSchemeProvider.IsUrlRegisteredExternal("http://www.test.com/savevaluetome"));
        }

        /// <summary>
        /// The init.
        /// </summary>
        private void Init()
        {
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http", "chromely.com", false));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https", "test.com", false));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https://www.google.com", true));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http://www.test.com", true));
        }
    }
}
