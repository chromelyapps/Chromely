// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Network
{
    /// <summary>
    /// The url scheme.
    /// </summary>
    public class UrlScheme
    {
        public UrlScheme(string scheme, string host, UrlSchemeType type)
        {
            Name = Guid.NewGuid().ToString();
            Scheme = scheme;
            Host = host;
            Folder = null;
            BaseUrl = null;
            UrlSchemeType = type;
            BaseUrlStrict = false;
            AssemblyOptions = null;

            if (!string.IsNullOrEmpty(BaseUrl))
            {
                var uri = new Uri(BaseUrl);
                Scheme = uri.Scheme;
                Host = uri.Host;
            }
        }

        public UrlScheme(string name, string scheme, string host, string folder, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions assemblyOptions = null)
        {
            Name = name;
            Scheme = scheme;
            Host = host;
            Folder = folder;
            BaseUrl = baseUrl;
            UrlSchemeType = type;
            BaseUrlStrict = baseUrlStrict;
            AssemblyOptions = assemblyOptions;

            if (!string.IsNullOrEmpty(BaseUrl))
            {
                var uri = new Uri(BaseUrl);
                Scheme = uri.Scheme;
                Host = uri.Host;
            }
        }

        public UrlScheme(string name, string scheme, string host, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions assemblyOptions = null)
        {
            Name = name;
            Scheme = scheme;
            Host = host;
            Folder = null;
            BaseUrl = baseUrl;
            UrlSchemeType = type;
            BaseUrlStrict = baseUrlStrict;
            AssemblyOptions = assemblyOptions;

            if (!string.IsNullOrEmpty(BaseUrl))
            {
                var uri = new Uri(BaseUrl);
                Scheme = uri.Scheme;
                Host = uri.Host;
            }
        }

        public UrlScheme(string scheme, string host, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions assemblyOptions = null)
        {
            Name = Guid.NewGuid().ToString();
            Scheme = scheme;
            Host = host;
            Folder = null;
            BaseUrl = baseUrl;
            UrlSchemeType = type;
            BaseUrlStrict = baseUrlStrict;
            AssemblyOptions = assemblyOptions;

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
        public string Folder { get; set; }
        public UrlSchemeType UrlSchemeType { get; set; }
        public AssemblyOptions AssemblyOptions { get; set; }
        public bool IsFolderResource => !string.IsNullOrWhiteSpace(Folder);

        /// <summary>
        /// Gets or sets a value indicating whether url must be relative to base.
        /// Only valid for external url.
        /// If base is http://a.com/me/you then 
        /// http://a.com/me/you/they is valid but
        /// http://a.com/me/they is not  valid
        /// </summary>
        public bool BaseUrlStrict { get; set; }
        public bool ValidSchemeHost
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Scheme) && !string.IsNullOrWhiteSpace(Host);
            }
        }

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
            return IsUrlOfSameScheme(url) && (UrlSchemeType == UrlSchemeType.ExternalBrowser);
        }

        public bool IsUrlRegisteredCommand(string url)
        {
            return IsUrlOfSameScheme(url) && (UrlSchemeType == UrlSchemeType.Command);
        }

        public bool IsUrlRegisteredAjax(string url)
        {
            return IsUrlOfSameScheme(url) && (UrlSchemeType == UrlSchemeType.ExternalRequest);
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
