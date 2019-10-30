// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredExternalUrl.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The registered external url.
    /// </summary>
    public static class RegisteredExternalUrl
    {
        /// <summary>
        /// The launch.
        /// </summary>
        /// <param name="requestUrl">
        /// The request url.
        /// </param>
        public static void Launch(string requestUrl)
        {
            // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
            try
            {
                Process.Start(requestUrl);
            }
            catch
            {
                try
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (CefRuntime.Platform == CefRuntimePlatform.Windows)
                    {
                        string url = requestUrl.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    }
                    else if (CefRuntime.Platform == CefRuntimePlatform.Linux)
                    {
                        Process.Start("xdg-open", requestUrl);
                    }
                    else if (CefRuntime.Platform == CefRuntimePlatform.MacOSX)
                    {
                        Process.Start("open", requestUrl);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.Error(exception);
                }
            }
        }
    }
}
