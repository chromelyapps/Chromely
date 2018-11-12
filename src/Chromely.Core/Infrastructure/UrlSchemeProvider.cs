// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSchemeProvider.cs" company="Chromely">
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

namespace Chromely.Core.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The url scheme provider.
    /// </summary>
    public static class UrlSchemeProvider
    {
        /// <summary>
        /// The lock object.
        /// </summary>
        private static readonly object ObjLock = new object();

        /// <summary>
        /// The url schemes.
        /// </summary>
        private static readonly List<UrlScheme> UrlSchemes = new List<UrlScheme>();

        /// <summary>
        /// Registers scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        public static void RegisterScheme(UrlScheme scheme)
        {
            lock (ObjLock)
            {
                if (!UrlSchemes.Contains(scheme))
                {
                    UrlSchemes.Add(scheme);
                }
            }
        }

        /// <summary>
        /// Checks if url registered is external.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUrlRegisteredExternal(string url)
        {
            lock (ObjLock)
            {
                return UrlSchemes.Any(x => (x.IsUrlOfSameScheme(url) && x.IsExternal));
            }
        }

        /// <summary>
        /// Checks if url registered is for custom scheme.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUrlOfRegisteredCustomScheme(string url)
        {
            lock (ObjLock)
            {
                return UrlSchemes.Any(x => (x.IsUrlOfSameScheme(url) && !x.IsExternal));
            }
        }
    }
}
