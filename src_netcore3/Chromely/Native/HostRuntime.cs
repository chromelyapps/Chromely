using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;

namespace Chromely.Native
{
    public static class HostRuntime
    {
        // Only MacOS required.
        private static string MacOSNativeDllFile = "libchromely.dylib";

        public static void LoadNativeGuiFile(ChromelyPlatform chromelyPlatform)
        {
            if (chromelyPlatform != ChromelyPlatform.MacOSX) return;

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPathNativeDll = Path.Combine(appDirectory, MacOSNativeDllFile);

            if (File.Exists(fullPathNativeDll))
            {
                return;
            }

            Task.Run(() =>
            {
                string resourcePath = $"Chromely.Native.mac.{MacOSNativeDllFile}";
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    using (var file = new FileStream(fullPathNativeDll, FileMode.Create, FileAccess.Write))
                    {
                        resource?.CopyTo(file);
                    }
                }
            });
        }

        public static void EnsureNativeGuiFileExists(ChromelyPlatform chromelyPlatform)
        {
            if (chromelyPlatform != ChromelyPlatform.MacOSX) return;

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPathNativeDll = Path.Combine(appDirectory, MacOSNativeDllFile);

            var timeout = DateTime.Now.Add(TimeSpan.FromSeconds(30));

            while (!File.Exists(fullPathNativeDll))
            {
                if (DateTime.Now > timeout)
                {
                    Log.Error($"File {fullPathNativeDll} does not exist.");
                    return;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
