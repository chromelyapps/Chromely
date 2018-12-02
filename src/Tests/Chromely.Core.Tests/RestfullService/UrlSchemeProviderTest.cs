// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSchemeProviderTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
