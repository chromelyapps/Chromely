using Chromely.Core;
using Chromely.Core.Infrastructure;
using System;
using System.Diagnostics;

namespace Chromely.CefGlue
{
    public static class BrowserLauncher
    {
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
                        Logger.Instance.Log.Error(exception);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }
    }
}
