// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public interface IChromelyRouteProvider
{
    /// <summary>
    /// Gets the action route keys. 
    /// </summary>
    IList<string> RouteKeys { get; }
    /// <summary>
    /// Gets action routes map.
    /// </summary>
    IDictionary<string, ControllerRoute> RouteMap { get; }

    /// <summary>
    /// Register all action routes.
    /// </summary>
    /// <param name="controllers">List of <see cref="ChromelyController"/> instance.</param>
    void RegisterAllRoutes(List<ChromelyController>? controllers);
    /// <summary>
    /// Register a single action route.
    /// </summary>
    /// <param name="key">The route key.</param>
    /// <param name="route">The <see cref="ControllerRoute"/> instance.</param>
    void RegisterRoute(string key, ControllerRoute route);
    /// <summary>
    /// Register multiple action routes.
    /// </summary>
    /// <param name="routeMap">The route map.</param>
    void RegisterRoutes(IDictionary<string, ControllerRoute>? routeMap);
    /// <summary>
    /// Gets a single action route based on route url.
    /// </summary>
    /// <param name="routeUrl">The route url.</param>
    /// <returns>The <see cref="ControllerRoute"/> object.</returns>
    ControllerRoute? GetRoute(string routeUrl);
    /// <summary>
    /// Checks if a route exists using the route url.
    /// </summary>
    /// <param name="routeUrl">The route url.</param>
    /// <returns>true or false.</returns>
    bool RouteExists(string routeUrl);
    /// <summary>
    /// Checks if the associated controller action is asynchronous.
    /// </summary>
    /// <param name="routeUrl">The route url.</param>
    /// <returns>true or false.</returns>
    bool IsRouteAsync(string routeUrl);
}