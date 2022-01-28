// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <inheritdoc/>
public class DefaultRequestSchemeProvider : IChromelyRequestSchemeProvider
{
    protected IDictionary<string, UrlScheme> _schemeMap;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultRequestSchemeProvider"/>.
    /// </summary>
    public DefaultRequestSchemeProvider()
    {
        _schemeMap = new Dictionary<string, UrlScheme>();
    }

    /// <inheritdoc/>
    public void Add(UrlScheme urlScheme)
    {
        string key = Key(urlScheme);
        if (!_schemeMap.ContainsKey(key))
        {
            _schemeMap[key] = urlScheme;
        }
    }

    /// <inheritdoc/>
    public UrlScheme? GetScheme(string url)
    {
        string key = Key(url);
        if (_schemeMap.ContainsKey(key))
        {
            return _schemeMap[key];
        }

        return default;
    }

    /// <inheritdoc/>
    public List<UrlScheme> GetAllSchemes()
    {
        var list = _schemeMap?.Values?.ToList();
        return list ?? new List<UrlScheme>();
    }

    /// <inheritdoc/>
    public bool IsSchemeRegistered(string url)
    {
        string key = Key(url);
        if (!string.IsNullOrWhiteSpace(key))
        {
            return _schemeMap.ContainsKey(key);
        }

        return false;
    }

    private static string Key(UrlScheme scheme)
    {
        if (scheme is null)
            return string.Empty;

        return Key(scheme.Scheme, scheme.Host);
    }

    private static string Key(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var uri = new Uri(url);
        return Key(uri.Scheme, uri.Host);
    }

    private static string Key(string scheme, string host)
    {
        return $"{scheme}::{host}";
    }
}