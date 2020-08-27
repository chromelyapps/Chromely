// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Chromely.NativeHosts
{
    public static class MacHostRuntime
    {
        // Required Only for MacOS.
        private static string MacOSNativeDllFile = "libchromely.dylib";

        public static void LoadNativeHostFile(IChromelyConfiguration config)
        {
            if (config.Platform != ChromelyPlatform.MacOSX) return;

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPathNativeDll = Path.Combine(appDirectory, MacOSNativeDllFile);

            if (File.Exists(fullPathNativeDll))
            {
                return;
            }

            Task.Run(() =>
            {
                string resourcePath = $"Chromely.NativeHosts.MacHost.{MacOSNativeDllFile}";
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (resource != null)
                    {
                        using (var file = new FileStream(fullPathNativeDll, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }
            });
        }

        public static void EnsureNativeHostFileExists(IChromelyConfiguration config)
        {
            if (config.Platform != ChromelyPlatform.MacOSX) return;

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPathNativeDll = Path.Combine(appDirectory, MacOSNativeDllFile);

            var timeout = DateTime.Now.Add(TimeSpan.FromSeconds(30));

            while (!File.Exists(fullPathNativeDll))
            {
                if (DateTime.Now > timeout)
                {
                    Logger.Instance.Log.LogError($"File {fullPathNativeDll} does not exist.");
                    return;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
