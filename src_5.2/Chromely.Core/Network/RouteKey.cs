// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network
{
    public static class RouteKey
    {
        public static string CreateRequestKey(string? url)
        {
            url = url?.Trim().TrimStart('/');
            var routeKey = $"action_{url}".Replace("/", "_").Replace("\\", "_");
            return routeKey.ToLower();
        }

        public static string CreateCommandKey(string? url)
        {
            url = url?.Trim().TrimStart('/');
            var routeKey = $"commmand_{url}".Replace("/", "_").Replace("\\", "_");
            return routeKey.ToLower();
        }
    }
}
