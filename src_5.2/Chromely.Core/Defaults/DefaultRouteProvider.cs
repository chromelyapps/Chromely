// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromelyRouteProvider"/>.
/// </summary>
public class DefaultRouteProvider : IChromelyRouteProvider
{
    protected readonly IChromelyModelBinder _routeParameterBinder;
    protected readonly IChromelyDataTransferOptions _dataTransferOptions;

    /// <summary>
    /// Initializes a new instance of <see cref="IChromelyRouteProvider"/>.
    /// </summary>
    /// <param name="routeParameterBinder">The <see cref="IChromelyModelBinder"/> instance.</param>
    /// <param name="dataTransferOptions">The <see cref="IChromelyDataTransferOptions"/> instance.</param>
    public DefaultRouteProvider(IChromelyModelBinder routeParameterBinder, IChromelyDataTransferOptions dataTransferOptions)
    {
        _routeParameterBinder = routeParameterBinder;
        _dataTransferOptions = dataTransferOptions;

        RouteMap = new Dictionary<string, ControllerRoute>();
    }

    /// <inheritdoc />
    public IDictionary<string, ControllerRoute> RouteMap { get; }

    /// <inheritdoc />
    public IList<string> RouteKeys
    {
        get
        {
            return RouteMap?.Keys?.ToList();
        }
    }

    /// <inheritdoc />
    public virtual void RegisterAllRoutes(List<ChromelyController> controllers)
    {
        if (controllers == null || !controllers.Any())
        {
            return;
        }

        try
        {

            foreach (var controller in controllers)
            {
                var controllerRoutesFactory = new ControllerRoutesFactory();
                controllerRoutesFactory.CreateAndRegisterRoutes(this, controller, _routeParameterBinder, _dataTransferOptions);
            }

        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

    /// <inheritdoc />
    public virtual void RegisterRoute(string key, ControllerRoute route)
    {
        RouteMap.Add(key, route);
    }

    /// <inheritdoc />
    public virtual void RegisterRoutes(IDictionary<string, ControllerRoute> routeMap)
    {
        if (routeMap != null && routeMap.Any())
        {
            foreach (var item in routeMap)
            {
                RegisterRoute(item.Key, item.Value);
            }
        }
    }

    /// <inheritdoc />
    public virtual ControllerRoute GetRoute(string routeUrl)
    {
        var key = Network.RouteKeys.CreateActionKey(routeUrl);
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        if (RouteMap.ContainsKey(key))
        {
            return RouteMap[key];
        }

        return null;
    }

    /// <inheritdoc />
    public virtual bool RouteExists(string routeUrl)
    {
        var keys = RouteKeys;
        if (!keys.IsNullOrEmpty())
        {
            var key = Network.RouteKeys.CreateActionKey(routeUrl);
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return keys.Contains(key);
        }

        return false;
    }

    /// <inheritdoc />
    public virtual bool IsRouteAsync(string routeUrl)
    {
        try
        {
            var route = GetRoute(routeUrl);
            return route == null ? false : route.IsAsync;
        }
        catch { }

        return false;
    }
}