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
        public UrlScheme(string name, string scheme, string host, string baseUrl, UrlSchemeType type, bool baseUrlStrict = true)
        {
            Name = name;
            Scheme = scheme;
            Host = host;
            BaseUrl = baseUrl;
            UrlSchemeType = type;
            BaseUrlStrict = baseUrlStrict;

            if (!string.IsNullOrEmpty(BaseUrl))
            {
                var uri = new Uri(BaseUrl);
                Scheme = uri.Scheme;
                Host = uri.Host;
            }
        }

        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public UrlSchemeType UrlSchemeType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether url must be relative to base.
        /// Only valid for external url.
        /// If base is http://a.com/me/you then 
        /// http://a.com/me/you/they is valid but
        /// http://a.com/me/they is not  valid
        /// </summary>
        public bool BaseUrlStrict { get; set; }
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
                return IsValidUrl(url);
            }

            return false;
        }

        public bool IsUrlRegisteredExternal(string url)
        {
            return IsUrlOfSameScheme(url) && (UrlSchemeType == UrlSchemeType.External);
        }

        public bool IsUrlRegisteredCommand(string url)
        {
            return IsUrlOfSameScheme(url) && (UrlSchemeType == UrlSchemeType.Command);
        }

        private bool IsValidUrl(string url)
        {
            if (BaseUrlStrict &&
                !string.IsNullOrWhiteSpace(BaseUrl) &&
                !string.IsNullOrWhiteSpace(url))
            {
                if (url.StartsWith(BaseUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}
