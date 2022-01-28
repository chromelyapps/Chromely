// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Represents the scheme handler.
/// </summary>
public interface IChromelySchemeHandler
{
    /// <summary>
    /// Gets or sets the handler identifier name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the url scheme of type <see cref="UrlScheme"/>.
    /// </summary>
    UrlScheme Scheme { get; set; }

    /// <summary>
    /// Gets or sets the handler.
    /// </summary>
    object Handler { get; set; }

    /// <summary>
    /// Gets or sets the handler factory. 
    /// </summary>
    object HandlerFactory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the scheme handler should be CORS enabled.
    /// </summary>
    bool IsCorsEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the scheme handler should be Security enabled.
    /// </summary>
    bool IsSecure { get; set; }
}