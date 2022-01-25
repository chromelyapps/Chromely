// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable

namespace Chromely.Core.Network;

public static class UrlSchemeCollectionExtensions
{
    private const char FORWARDSLASH = '/';
    private const char BACKWARDSLASH = '\\';

    /// <summary>
    /// Check if url scheme is registered and of type <see cref="UrlSchemeType.ExternalBrowser"/>.
    /// </summary>
    /// <param name="urlSchemes">The collection of type <see cref="UrlScheme"/> to extend.</param>
    /// <param name="url">The url to check.</param>
    /// <returns>true if external browser scheme, otherwise false.</returns>
    public static bool IsUrlRegisteredExternalBrowserScheme(this List<UrlScheme> urlSchemes, string url)
    {
        if (urlSchemes is null ||
            !urlSchemes.Any() ||
            string.IsNullOrWhiteSpace(url))
            return false;

        return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.ExternalBrowser)));
    }

    /// <summary>
    /// Check if the url scheme is of type <see cref="UrlSchemeType.FolderResource"/>.
    /// </summary>
    /// <param name="urlScheme">The class of type <see cref="UrlScheme"/> to extend.</param>
    /// <returns>true if scheme is a folder resource scheme, otherwise false.</returns>
    public static bool IsUrlSchemeFolderResource(this UrlScheme urlScheme)
    {
        if (urlScheme is null)
        {
            return false;
        }

        return urlScheme.UrlSchemeType == UrlSchemeType.FolderResource;
    }

    /// <summary>
    /// Gets all instances of <see cref="UrlScheme"/> registered by specified type of <see cref="UrlSchemeType"/>.
    /// </summary>
    /// <param name="urlSchemes">The collection of type <see cref="UrlScheme"/> to extend.</param>
    /// <param name="type">Instance of <see cref="Network.UrlSchemeType"/>.</param>
    /// <returns>collection of <see cref="UrlScheme"/> instances.</returns>
    public static IEnumerable<UrlScheme> GetSchemesByType(this List<UrlScheme> urlSchemes, UrlSchemeType type)
    {
        if (urlSchemes is null || !urlSchemes.Any())
            return new List<UrlScheme>();

        return urlSchemes.Where((x => (x.UrlSchemeType == type))).ToList();
    }

    /// <summary>
    /// Get scheme based on specified url.
    /// </summary>
    /// <param name="urlSchemes">The collection of type <see cref="UrlScheme"/> to extend.</param>
    /// <param name="url">The specified url.</param>
    /// <returns>instance of <see cref="UrlScheme"/>.</returns>
    public static UrlScheme GetScheme(this List<UrlScheme> urlSchemes, string url)
    {
        if (urlSchemes is null ||
             urlSchemes.Count == 0 ||
            string.IsNullOrWhiteSpace(url))
            return null;

        var uri = new Uri(url);
        var scheme = string.IsNullOrWhiteSpace(uri.Scheme) ? string.Empty : uri.Scheme;
        var host = string.IsNullOrWhiteSpace(uri.Host) ? string.Empty : uri.Host;

        var itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                             (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                             (!string.IsNullOrWhiteSpace(x.Host) && x.Scheme.Equals(host, StringComparison.InvariantCultureIgnoreCase))));

        if (itemList is not null && itemList.Any())
        {
            return itemList.FirstOrDefault();
        }


        itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                             (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase))));

        if (itemList is not null && itemList.Any())
        {
            return itemList.FirstOrDefault();
        }

        return null;
    }

    /// <summary>
    /// Get scheme based on specified url and <see cref="UrlSchemeType"/>.
    /// </summary>
    /// <param name="urlSchemes">The collection of type <see cref="UrlScheme"/> to extend.</param>
    /// <param name="url">The specified url.</param>
    /// <param name="type">The specified type of <see cref="UrlSchemeType"/>.</param>
    /// <returns></returns>
    public static UrlScheme GetScheme(this List<UrlScheme> urlSchemes, string url, UrlSchemeType type)
    {
        if (urlSchemes is null ||
             urlSchemes.Count == 0 ||
            string.IsNullOrWhiteSpace(url))
            return null;

        var uri = new Uri(url);
        var scheme = string.IsNullOrWhiteSpace(uri.Scheme) ? string.Empty : uri.Scheme;
        var host = string.IsNullOrWhiteSpace(uri.Host) ? string.Empty : uri.Host;

        var itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                             (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                             (!string.IsNullOrWhiteSpace(x.Host) && x.Scheme.Equals(host, StringComparison.InvariantCultureIgnoreCase)) &&
                                             (x.UrlSchemeType == type)));

        if (itemList is not null && itemList.Any())
        {
            return itemList.FirstOrDefault();
        }


        itemList = urlSchemes.Where((x => x.IsUrlOfSameScheme(url) &&
                                             (!string.IsNullOrWhiteSpace(x.Scheme) && x.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)) &&
                                             (x.UrlSchemeType == type)));

        if (itemList is not null && itemList.Any())
        {
            return itemList.FirstOrDefault();
        }

        return null;
    }


    /// <summary>
    /// Gets the resource folder path for scheme of type <see cref="UrlSchemeType.FolderResource"/>.
    /// </summary>
    /// <remarks>
    /// This ensures that the folder path is formatted based on the OS platform.
    /// The <see cref="Path.DirectorySeparatorChar"/> ensures that the correct folder path separator is used.
    /// </remarks>
    /// <param name="urlScheme">The class of type <see cref="UrlScheme"/> to extend.</param>
    /// <param name="routePath">The controller route path.</param>
    /// <returns>the formatted resource folder path.</returns>
    public static string GetResourceFolderFile(this UrlScheme urlScheme, string routePath)
    {
        if (urlScheme is null ||
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