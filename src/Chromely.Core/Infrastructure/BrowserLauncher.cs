// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

/// <summary>
/// Utility class for external browser launcher.
/// </summary>
public static class BrowserLauncher
{
    /// <summary>
    /// Launches an external browser using the url.
    /// </summary>
    /// <param name="platform">The OS platform type.</param>
    /// <param name="url">The web page address to launch.</param>
    public static void Open(ChromelyPlatform platform, string url)
    {
        try
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                try
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    switch (platform)
                    {
                        case ChromelyPlatform.Windows:
                            url = url.Replace("&", "^&");
                            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                            break;

                        case ChromelyPlatform.Linux:
                            Process.Start("xdg-open", url);
                            break;

                        case ChromelyPlatform.MacOSX:
                            Process.Start("open", url);
                            break;

                        default:
                            Process.Start(url);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.LogError(exception);
                }
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }
}