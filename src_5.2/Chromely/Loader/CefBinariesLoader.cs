// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Loader;

/// <summary>
/// The cef binaries loader.
/// </summary>
public static class CefBinariesLoader
{
    /// <summary>Load Cef binaries</summary>
    /// <param name="cefBinariesDownloader">The cef binaries downloader.</param>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    public static void Load(ICefDownloader cefBinariesDownloader, IChromelyConfiguration config)
    {
        try
        {
            var platform = CefRuntime.Platform;
            var version = CefRuntime.ChromeVersion;
            Logger.Instance.Log.LogInformation("Running {platform} chromium {version}", platform, version);

            try
            {
                CefRuntime.Load();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.LogError(ex);

                if (config.CefDownloadOptions.AutoDownloadWhenMissing)
                {
                    if (config.CefDownloadOptions.DownloadSilently)
                    {
                        cefBinariesDownloader.Download(config);
                        CefLoader.SetMacOSAppName(config);
                    }
                    else
                    {
                        DownloadAndNotifyUser(cefBinariesDownloader, config);
                        CefLoader.SetMacOSAppName(config);
                        return;
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.Log.LogError(ex);
            Environment.Exit(0);
        }
    }

    /// <summary>
    /// The delete temp files.
    /// </summary>
    /// <param name="cefBinariesDownloader">The cef binaries downloader.</param>
    public static void Cleanup(ICefDownloader cefBinariesDownloader)
    {
        var downloadNotification = cefBinariesDownloader.Notification;
        downloadNotification.Cleanup();
    }

    /// <summary>
    /// The warn user on load.
    /// </summary>
    private static void DownloadAndNotifyUser(ICefDownloader cefBinariesDownloader, IChromelyConfiguration config)
    {
        var downloadNotification = cefBinariesDownloader.Notification;

        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            downloadNotification.OnDownloadStarted();

            cefBinariesDownloader.Download(config);

            stopwatch.Stop();

            downloadNotification.OnDownloadCompleted(stopwatch.Elapsed);

            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        catch (Exception ex)
        {
            Logger.Instance.Log.LogError(ex);
            downloadNotification.OnDownloadFailed(ex);
            Environment.Exit(0);
        }
    }
}