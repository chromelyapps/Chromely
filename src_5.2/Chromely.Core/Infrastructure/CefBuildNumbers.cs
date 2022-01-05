// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public class CefBuildNumbers
{
    public string CefVersion { get; }
    public int CefMajor { get; }
    public int CefMinor { get; }
    public int CefBuild { get; }

    public string ChromiumVersion { get; }
    public int ChromiumMajor { get; }
    public int ChromiumMinor { get; }
    public int ChromiumBuild { get; }
    public int ChromiumPatch { get; }

    public CefBuildNumbers(string cefVersion, string chromiumVersion)
    {
        CefVersion = cefVersion;
        ChromiumVersion = chromiumVersion;

        int number;
        var parts = cefVersion.Split('+');
        if (parts.Length > 1) parts = parts[0].Split('.');
        if (parts.Length == 3)
        {
            if (int.TryParse(parts[0], out number)) CefMajor = number;
            if (int.TryParse(parts[1], out number)) CefMinor = number;
            if (int.TryParse(parts[2], out number)) CefBuild = number;
        }

        parts = chromiumVersion.Split('.');
        if (parts.Length != 4) return;

        if (int.TryParse(parts[0], out number)) ChromiumMajor = number;
        if (int.TryParse(parts[1], out number)) ChromiumMinor = number;
        if (int.TryParse(parts[2], out number)) ChromiumBuild = number;
        if (int.TryParse(parts[3], out number)) ChromiumPatch = number;
    }

    public override string ToString()
    {
        return $"CEF {CefVersion}";
    }
}