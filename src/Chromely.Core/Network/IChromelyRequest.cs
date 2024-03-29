﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// The Chromely request.
/// </summary>
public interface IChromelyRequest
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    string Id { get; set; }
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the route url.
    /// </summary>
    string RouteUrl { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    IDictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets the post data.
    /// </summary>
    object? PostData { get; set; }

    /// <summary>
    /// Gets or sets the raw json.
    /// Only used for CefGlue Generic Message Routing requests.
    /// </summary>
    string? RawJson { get; set; }
}