// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSchemeProviderTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Chromely.Core.Tests.RestfulService
{
    /// <summary>
    /// The url scheme provider test.
    /// </summary>
    public class UrlSchemeProviderTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _testOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlSchemeProviderTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public UrlSchemeProviderTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            Init();
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
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http", "chromely.com", UrlSchemeType.Custom));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https", "test.com", UrlSchemeType.Custom));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("https://www.google.com", UrlSchemeType.External));
            UrlSchemeProvider.RegisterScheme(new UrlScheme("http://www.test.com", UrlSchemeType.External));
        }
    }
}
