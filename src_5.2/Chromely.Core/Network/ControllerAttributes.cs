// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

[AttributeUsage(AttributeTargets.Method)]
public class ChromelyRouteAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Description { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class ChromelyControllerAttribute : Attribute
{
    public ChromelyControllerAttribute()
    {
        RoutePath = string.Empty;
    }

    public string? Name { get; set; }
    public string RoutePath { get; set; }
    public string? Description { get; set; }
}