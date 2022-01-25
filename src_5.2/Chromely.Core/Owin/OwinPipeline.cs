// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Owin;

// Shorthand for Owin pipeline func.
using AppFunc = Func<IDictionary<string, object>, Task>;

/// <summary>
/// Represents OWIN wrapper functionality for OWIN environment.
/// </summary>
public interface IOwinPipeline
{
    /// <summary>
    /// Gets or sets the shorthand for OWIN pipeline function.
    /// </summary>
    AppFunc AppFunc { get; set; }

    /// <summary>
    /// Gets or sets the error handling path.
    /// </summary>
    string? ErrorHandlingPath { get; set; }

    /// <summary>
    /// Gets or sets a collection routes - instances of <see cref="OwinRoute"/>.
    /// </summary>
    List<OwinRoute> Routes { get; set; }
}

public class OwinPipeline : IOwinPipeline
{
    /// <summary>
    /// Initializes a new instance of <see cref="OwinPipeline"/>.
    /// </summary>
    public OwinPipeline()
    {
        Routes = new List<OwinRoute>();
    }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    /// <inheritdoc/>
    public AppFunc? AppFunc { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

    /// <inheritdoc/>
    public string? ErrorHandlingPath { get; set; }

    /// <inheritdoc/>
    public List<OwinRoute> Routes { get; set; }
}

/// <summary>
/// Wrapper class for relevant OWIN route properties.
/// </summary>
public class OwinRoute
{
    /// <summary>
    /// Initializes a new instance of <see cref="OwinRoute"/>.
    /// </summary>
    /// <param name="displayName">The route display name.</param>
    /// <param name="routePath">The route path.</param>
    /// <param name="relativePath">The route relative path.</param>
    public OwinRoute(string? displayName, string routePath, string? relativePath = null)
    {
        DisplayName = displayName;
        RoutePath = routePath;
        RelativePath = relativePath;
    }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    public string RoutePath { get; set; }

    /// <summary>
    /// Gets or sets the route relative path.
    /// </summary>
    public string? RelativePath { get; set; }
}