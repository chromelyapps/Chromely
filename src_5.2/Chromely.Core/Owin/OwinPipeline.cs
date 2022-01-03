// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Owin;

// Shorthand for Owin pipeline func
using AppFunc = Func<IDictionary<string, object>, Task>;

public interface IOwinPipeline
{
    AppFunc AppFunc { get; set; }
    string? ErrorHandlingPath { get; set; }
    List<OwinRoute> Routes { get; set; }
}

public class OwinPipeline : IOwinPipeline
{
    public OwinPipeline()
    {
        Routes = new List<OwinRoute>();
    }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public AppFunc? AppFunc { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public string? ErrorHandlingPath { get; set; }
    public List<OwinRoute> Routes { get; set; }
}

public class OwinRoute
{
    public OwinRoute(string? displayName, string routePath, string? relativePath = null)
    {
        DisplayName = displayName;
        RoutePath = routePath;
        RelativePath = relativePath;
    }

    public string? DisplayName { get; set; }
    public string RoutePath { get; set; }
    public string? RelativePath { get; set; }
}