// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyRuntime.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.Core.Infrastructure;

namespace Chromely.Core
{
    /// <summary>
    /// This class provides operating system and runtime information
    /// used to host the application.
    /// </summary>
    public static class ChromelyRuntime
    {
        /// <summary>
        /// The get expected chromium build number.
        /// </summary>
        /// <param name="wrapper">
        /// The wrapper.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetExpectedChromiumBuildNumber()
        {
            try
            {
                var appExeLocation = AppDomain.CurrentDomain.BaseDirectory;
                string dllName = Path.Combine(appExeLocation, "Chromely.CefGlue.dll");
                var assembly = Assembly.LoadFrom(dllName);
                var types = assembly.GetTypes();
                var type = types.FirstOrDefault(t => t.Name == "CefRuntime");
                var versionProperty = type?.GetProperty("ChromeVersion");
                var version = versionProperty?.GetValue(null).ToString();
                if (!string.IsNullOrEmpty(version)
                    && int.TryParse(version.Split('.')[2], out var build))
                {
                    return build;
                }
                if (type == null)
                {
                    Logger.Instance.Log.Error("Could not get expected chromium build number: Unable to load CefRuntime.ChromeVersion"); 
                }
                else
                {
                    Logger.Instance.Log.Error($"Could not parse chromium build number '{version}'");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Could not get expected chromium build number: " + ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// Gets the runtime the application is running on.
        /// </summary>
        public static ChromelyPlatform Platform
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                        return ChromelyPlatform.MacOSX;
                    
                    case PlatformID.Unix:
                    case (PlatformID)128:   // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                        return IsRunningOnMac()
                        ? ChromelyPlatform.MacOSX
                        : ChromelyPlatform.Linux;

                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return ChromelyPlatform.Windows;

                    default:
                        return ChromelyPlatform.NotSupported;
                }
                
            }
        }

        private static bool IsRunningOnMac()
        {
            try
            {
                var osName = Environment.OSVersion.VersionString;
                if (osName.ToLower().Contains("darwin")) return true;
                if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist")) return true;
            }
            catch {}

            return false;
        }
    }
}