// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromelyRequestHandler"/>.
/// </summary>
public class DefaultActionRequestHandler : IChromelyRequestHandler
{
    protected readonly IChromelyRouteProvider _routeProvider;
    protected readonly IChromelyInfo _chromelyInfo;
    protected readonly IChromelyErrorHandler _chromelyErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultActionRequestHandler"/>.
    /// </summary>
    /// <param name="routeProvider">The router provider - instance of <see cref="IChromelyRouteProvider"/>.</param>
    /// <param name="chromelyInfo">The chromely info - instance of <see cref="IChromelyInfo"/>.</param>
    /// <param name="chromelyErrorHandler">Main Chromely error handler - instance of <see cref="IChromelyErrorHandler"/>.</param>
    public DefaultActionRequestHandler(IChromelyRouteProvider routeProvider, IChromelyInfo chromelyInfo, IChromelyErrorHandler chromelyErrorHandler)
    {
        _routeProvider = routeProvider;
        _chromelyInfo = chromelyInfo;
        _chromelyErrorHandler = chromelyErrorHandler;
    }

    /// <inheritdoc/>
    public void Execute(string url)
    {
        var routePath = url.GetPathFromUrl();
        var parameters = url.GetParameters();

        ExecuteRoute(string.Empty, routePath, parameters, null, string.Empty);
    }

    /// <inheritdoc/>
    public IChromelyResponse Execute(string requestId, string routePath, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        if (string.IsNullOrWhiteSpace(routePath))
        {
            return _chromelyErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        if (routePath.ToLower().Equals("/info"))
        {
            return _chromelyInfo.GetInfo(requestId);
        }

        var route = _routeProvider.GetRoute(routePath);
        if (route is null)
        {
            throw new Exception($"Route for path = {routePath} is null or invalid.");
        }

        return ExecuteRoute(requestId, routePath, parameters, postData, requestData);
    }

    /// <inheritdoc/>
    public async Task<IChromelyResponse> ExecuteAsync(string requestId, string routePath, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        if (string.IsNullOrWhiteSpace(routePath))
        {
            return _chromelyErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        if (routePath.ToLower().Equals("/info"))
        {
            return _chromelyInfo.GetInfo(requestId);
        }

        var route = _routeProvider.GetRoute(routePath);
        if (route is null)
        {
            return _chromelyErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        return await ExecuteRouteAsync(requestId, routePath, parameters, postData, requestData);
    }

    private IChromelyResponse ExecuteRoute(string requestId, string routeUrl, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        var route = _routeProvider.GetRoute(routeUrl);

        if (route is null)
        {
            return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
        }

        var request = new ChromelyRequest(requestId, routeUrl, parameters, postData, requestData);
        var response = route.Invoke(request);
        response.ReadyState = (int)ReadyState.ResponseIsReady;
        response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
        response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

        return response;
    }

    private async Task<IChromelyResponse> ExecuteRouteAsync(string requestId, string routeUrl, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        var route = _routeProvider.GetRoute(routeUrl);

        if (route is null)
        {
            return _chromelyErrorHandler.HandleRouteNotFound(requestId, routeUrl);
        }

        IChromelyResponse response;
        var request = new ChromelyRequest(requestId, routeUrl, parameters, postData, requestData);


        response = await route.InvokeAsync(request);

        response.ReadyState = (int)ReadyState.ResponseIsReady;
        response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
        response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

        return response;
    }
}