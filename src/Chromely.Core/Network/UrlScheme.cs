// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// The Chromely url scheme.
/// </summary>
/// <remarks>
/// The UrlScheme allows developers to configure how a scheme is handled.
/// This is dictated mostly be url scheme type as defined in <see cref="Network.UrlSchemeType"/>.
/// </remarks>
public class UrlScheme
{
    /// <summary>
    /// Initializes a new instance of <see cref="UrlScheme"/>.
    /// </summary>
    /// <param name="scheme">The scheme name.</param>
    /// <param name="host">The scheme host.</param>
    /// <param name="type">Instance of <see cref="Network.UrlSchemeType"/>.</param>
    public UrlScheme(string scheme, string host, UrlSchemeType type)
    {
        Name = Guid.NewGuid().ToString();
        Scheme = scheme;
        Host = host;
        BaseUrl = null;
        Folder = null;
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

    /// <summary>
    /// Initializes a new instance of <see cref="UrlScheme"/>.
    /// </summary>
    /// <param name="name">The scheme identifier name.</param>
    /// <param name="scheme">The scheme name.</param>
    /// <param name="host">The scheme host.</param>
    /// <param name="folder">The resource folder.</param>
    /// <param name="baseUrl">The base url.</param>
    /// <param name="type">Instance of <see cref="Network.UrlSchemeType"/>.</param>
    /// <param name="baseUrlStrict"></param>
    /// <param name="assemblyOptions">Instance of <see cref="Network.AssemblyOptions"/>.</param>
    public UrlScheme(string name, string scheme, string host, string folder, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions? assemblyOptions = null)
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

    /// <summary>
    /// Initializes a new instance of <see cref="UrlScheme"/>.
    /// </summary>
    /// <param name="name">The scheme identifier name.</param>
    /// <param name="scheme">The scheme name.</param>
    /// <param name="host">The scheme host.</param>
    /// <param name="baseUrl">The base url.</param>
    /// <param name="type">Instance of <see cref="Network.UrlSchemeType"/>.</param>
    /// <param name="baseUrlStrict"></param>
    /// <param name="assemblyOptions">Instance of <see cref="Network.AssemblyOptions"/>.</param>
    public UrlScheme(string name, string scheme, string host, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions? assemblyOptions = null)
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

    /// <summary>
    /// Initializes a new instance of <see cref="UrlScheme"/>.
    /// </summary>
    /// <param name="scheme">The scheme name.</param>
    /// <param name="host">The scheme host.</param>
    /// <param name="baseUrl">The base url.</param>
    /// <param name="type">Instance of <see cref="Network.UrlSchemeType"/>.</param>
    /// <param name="baseUrlStrict"></param>
    /// <param name="assemblyOptions">Instance of <see cref="Network.AssemblyOptions"/>.</param>
    public UrlScheme(string scheme, string host, string baseUrl, UrlSchemeType type, bool baseUrlStrict = false, AssemblyOptions? assemblyOptions = null)
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

    /// <summary>
    /// Gets or sets the scheme identifier name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the base url.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the scheme name.
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// Gets or sets the scheme host.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets a resource folder.
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Network.UrlSchemeType"/>.
    /// </summary>
    public UrlSchemeType UrlSchemeType { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Network.AssemblyOptions"/>.
    /// </summary>
    public AssemblyOptions? AssemblyOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether url must be relative to base.
    /// Only valid for external url.
    /// If base is http://a.com/me/you then
    /// http://a.com/me/you/they is valid but
    /// http://a.com/me/they is not  valid
    /// </summary>
    public bool BaseUrlStrict { get; set; }

    /// <summary>
    /// Gets a value indicating whether scheme name and host name are valid.
    /// </summary>
    public bool IsValidSchemeAndHost
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Scheme) && !string.IsNullOrWhiteSpace(Host);
        }
    }

    /// <summary>
    /// Check if the reference scheme name is a standard scheme.
    /// </summary>
    /// <param name="scheme">Scheme name to check.</param>
    /// <returns>true if standard scheme, otherwise false.</returns>
    public static bool IsStandardScheme(string scheme)
    {
        if (string.IsNullOrEmpty(scheme))
        {
            return false;
        }

        return scheme.ToLowerInvariant() switch
        {
            "http" or "https" or "file" or "ftp" or "about" or "data" => true,
            _ => false,
        };
    }

    /// <summary>
    /// Check if the reference Url is of same scheme.
    /// </summary>
    /// <param name="url">Url to check.</param>
    /// <returns>true if same scheme, otherwise false.</returns>
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

        if (Scheme.Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase) &&
            Host.Equals(uri.Host, StringComparison.OrdinalIgnoreCase))
        {
            return IsValidUrl(url);
        }

        return false;
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