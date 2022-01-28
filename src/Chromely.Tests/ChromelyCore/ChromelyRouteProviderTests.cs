// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Tests.ChromelyCore;

public class ChromelyRouteProviderTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChromelyRouteProvider _routeProvider;
    private readonly List<ChromelyController> _controllers;

    public ChromelyRouteProviderTests(IServiceProvider serviceProvider, IChromelyRouteProvider routeProvider)
    {
        _serviceProvider = serviceProvider;
        _routeProvider = routeProvider;
        _controllers = _serviceProvider.GetServices<ChromelyController>().ToList();
        routeProvider.RegisterAllRoutes(_controllers);
    }

    [Fact]
    public void EnsureAllRoutesAreRegisteredTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            var key = RouteKeys.CreateActionKey(routePathItem.Value);
            Assert.True(_routeProvider.RouteMap.ContainsKey(key));
        }
    }

    [Fact]
    public void RouteExistsTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            Assert.True(_routeProvider.RouteExists("http://chromely.com" + routePathItem.Value));
        }
    }

    [Fact]
    public void GetRouteTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            Assert.NotNull(_routeProvider.GetRoute("http://chromely.com" + routePathItem.Value));
        }
    }

    [Fact]
    public void IsRouteAsyncTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            if (routePathItem.Value.Contains("/async/"))
            {
                Assert.True(_routeProvider.IsRouteAsync("http://chromely.com" + routePathItem.Value));
            }
            else
            {
                Assert.False(_routeProvider.IsRouteAsync("http://chromely.com" + routePathItem.Value));
            }
        }
    }
}