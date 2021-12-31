// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network
{
    public static class UrlSchemeCollectionExtensions
    {
        private const char FORWARDSLASH = '/';
        private const char BACKWARDSLASH = '\\';

        public static bool IsUrlRegisteredExternalBrowserScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                !urlSchemes.Any() ||
                string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.ExternalBrowser)));
        }

        public static bool IsUrlRegisteredLocalRequestScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                 !urlSchemes.Any() ||
                 string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.LocalRequest)));
        }

        public static bool IsUrlRegisteredExternalRequestScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                 !urlSchemes.Any() ||
                 string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.LocalRequest)));
        }

        public static IEnumerable<UrlScheme> GetSchemesByType(this List<UrlScheme> urlSchemes, UrlSchemeType type)
        {
            if (urlSchemes == null || !urlSchemes.Any())
                return new List<UrlScheme>();

            return urlSchemes.Where((x => (x.UrlSchemeType == type))).ToList();
        }

        public static IEnumerable<UrlScheme> GetSchemesByType(this List<UrlScheme> urlSchemes, string url, UrlSchemeType type)
        {
            if (urlSchemes == null ||
                 urlSchemes.Count == 0 ||
                 string.IsNullOrWhiteSpace(url))
                return null;

            return urlSchemes.Where((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == type))).ToList();
        }

        public static UrlScheme GetScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                 urlSchemes.Count == 0 ||
                string.IsNullOrWhiteSpace(url))
                return null;

            var uri = new Uri(url);
            var scheme = string.IsNullOrWhiteSpace(uri.Scheme) ? string.Empty : uri.Scheme;
            var host = string.IsNullOrWhiteSpace(uri.Host) ? string.Empty : uri.Host;

            var itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                                 (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                                 (!string.IsNullOrWhiteSpace(x.Host) && x.Scheme.Equals(host, StringComparison.InvariantCultureIgnoreCase))));

            if (itemList != null && itemList.Any())
            {
                return itemList.FirstOrDefault();
            }


            itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                                 (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase))));

            if (itemList != null && itemList.Any())
            {
                return itemList.FirstOrDefault();
            }

            return null;
        }

        public static UrlScheme GetScheme(this List<UrlScheme> urlSchemes, string url, UrlSchemeType type)
        {
            if (urlSchemes == null ||
                 urlSchemes.Count == 0 ||
                string.IsNullOrWhiteSpace(url) )
                return null;

            var uri = new Uri(url);
            var scheme = string.IsNullOrWhiteSpace(uri.Scheme) ? string.Empty : uri.Scheme;
            var host = string.IsNullOrWhiteSpace(uri.Host) ? string.Empty : uri.Host;

            var itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                                 (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                                 (!string.IsNullOrWhiteSpace(x.Host) && x.Scheme.Equals(host, StringComparison.InvariantCultureIgnoreCase)) &&
                                                 (x.UrlSchemeType == type)));

            if (itemList != null && itemList.Any())
            {
                return itemList.FirstOrDefault();
            }


            itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                                 (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                                 (x.UrlSchemeType == type)));

            if (itemList != null && itemList.Any())
            {
                return itemList.FirstOrDefault();
            }

            return null;
        }

        public static bool IsUrlSchemeFolderResource(this UrlScheme urlScheme)
        {
            if (urlScheme == null)
            {
                return false;
            }

            return urlScheme.UrlSchemeType == UrlSchemeType.FolderResource;
        }

        public static string GetResourceFolderFile(this UrlScheme urlScheme, string routePath)
        {
            if (urlScheme == null ||
                string.IsNullOrWhiteSpace(urlScheme.Folder) ||
                string.IsNullOrWhiteSpace(routePath))
            {
                return string.Empty;
            }

            var folder = urlScheme.Folder.TrimEnd(FORWARDSLASH).TrimEnd(BACKWARDSLASH).Replace(FORWARDSLASH, Path.DirectorySeparatorChar).Replace(BACKWARDSLASH, Path.DirectorySeparatorChar);
            routePath = routePath.TrimStart(FORWARDSLASH).TrimStart(BACKWARDSLASH).Replace(FORWARDSLASH, Path.DirectorySeparatorChar).Replace(BACKWARDSLASH, Path.DirectorySeparatorChar);

            return $"{folder}{Path.DirectorySeparatorChar}{routePath}";
        }
    }
}
