// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlScheme.cs" company="Chromely">
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
    using System;

    /// <summary>
    /// The url scheme.
    /// </summary>
    public class UrlScheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlScheme"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="isExternal">
        /// The is external.
        /// </param>
        public UrlScheme(string scheme, string host, bool isExternal)
        {
            this.Scheme = scheme;
            this.Host = host;
            this.IsExternal = isExternal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlScheme"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="isExternal">
        /// The is external.
        /// </param>
        public UrlScheme(string url, bool isExternal)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                this.Scheme = uri.Scheme;
                this.Host = uri.Host;
                this.IsExternal = isExternal;
            }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is external.
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// Check if scheme is a standard type.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsStandardScheme(string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                return false;
            }

            switch (scheme.ToLower())
            {
                case "http":
                case "https":
                case "file":
                case "ftp":
                case "about":
                case "data":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if url is of same scheme as the object url.
        /// </summary>
        /// <param name="url">
        /// The url to check.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsUrlOfSameScheme(string url)
        {
            if (string.IsNullOrEmpty(this.Scheme) ||
                string.IsNullOrEmpty(this.Host) ||
                string.IsNullOrEmpty(url))
            {
                return false;
            }

            var uri = new Uri(url);

            if (string.IsNullOrEmpty(uri.Scheme) ||
                string.IsNullOrEmpty(uri.Host))
            {
                return false;
            }

            if (this.Scheme.ToLower().Equals(uri.Scheme) &&
                this.Host.ToLower().Equals(uri.Host))
            {
                return true;
            }

            return false;
        }
    }
}
