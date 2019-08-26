// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlScheme.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Infrastructure
{
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
        /// <param name="type">
        /// The is external.
        /// </param>
        public UrlScheme(string scheme, string host, UrlSchemeType type)
        {
            Scheme = scheme;
            Host = host;
            UrlSchemeType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlScheme"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="type">
        /// The is url scheme type.
        /// </param>
        public UrlScheme(string url, UrlSchemeType type)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                Scheme = uri.Scheme;
                Host = uri.Host;
                UrlSchemeType = type;
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
        /// Gets or sets url scheme type.
        /// </summary>
        public UrlSchemeType UrlSchemeType { get; set; }

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
            if (string.IsNullOrEmpty(Scheme) ||
                string.IsNullOrEmpty(Host) ||
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

            if (Scheme.ToLower().Equals(uri.Scheme) &&
                Host.ToLower().Equals(uri.Host))
            {
                return true;
            }

            return false;
        }
    }
}
