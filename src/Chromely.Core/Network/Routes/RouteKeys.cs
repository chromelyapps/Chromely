// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public static class RouteKeys
{
    public static string CreateSchemeKey(string scheme, string host, string folder)
    {
        folder = string.IsNullOrWhiteSpace(folder) ? "none" : folder;
        var keyString = $"{scheme}_{host}_{folder}".ToLowerInvariant();
        return CreateMD5Hash(keyString);
    }

    public static string CreateActionKey(string? controllerPath, string? actionUrl)
    {
        var absolutePath = PathAndQuery.CreateUri(actionUrl)?.AbsolutePath;
        controllerPath = controllerPath?.Trim().TrimStart('/');
        absolutePath = absolutePath?.Trim().TrimStart('/');
        string? routeKey;
        if (string.IsNullOrWhiteSpace(controllerPath))
        {
            routeKey = $"routepath_{absolutePath}".Replace("/", "_").Replace("\\", "_");
        }
        else
        {
            routeKey = $"routepath_{controllerPath}_{absolutePath}".Replace("/", "_").Replace("\\", "_");
        }

        return CreateMD5Hash(routeKey);
    }

    public static string CreateActionKey(string url)
    {
        var absolutePath = PathAndQuery.CreateUri(url)?.AbsolutePath;
        absolutePath = absolutePath?.Trim().TrimStart('/');
        var routeKey = $"routepath_{absolutePath}".Replace("/", "_").Replace("\\", "_");
        return CreateMD5Hash(routeKey);
    }

    private static string CreateMD5Hash(string input)
    {
        var md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }

        return sb.ToString().ToLowerInvariant();
    }
}