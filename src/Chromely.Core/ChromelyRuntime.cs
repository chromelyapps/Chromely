// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyRuntime.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
        public static int GetExpectedChromiumBuildNumber(ChromelyCefWrapper wrapper)
        {
            try
            {
                switch (wrapper)
                {
                    case ChromelyCefWrapper.CefGlue:
                        return GetExpectedChromiumBuildNumberCefGlue(GetWrapperAssemblyName(wrapper));
                
                    case ChromelyCefWrapper.CefSharp:
                        return GetExpectedChromiumBuildNumberCefSharp(GetWrapperAssemblyName(wrapper));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // ignore
            }
            return 0;
        }

        private static int GetExpectedChromiumBuildNumberCefGlue(string dllName)
        {
            try
            {
                var directory = Path.GetDirectoryName(dllName);
                var fileNameWithExt = Path.GetFileName(dllName);

                // for CefGlue use common assembly
                fileNameWithExt = fileNameWithExt?
                    .Replace(".CefGlue.Gtk.", ".CefGlue.")
                    .Replace(".CefGlue.Winapi.", ".CefGlue.");

                if (directory != null) dllName = Path.Combine(directory, fileNameWithExt);
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
                    Log.Error("Could not get expected chromium build number: Unable to load CefRuntime.ChromeVersion"); 
                }
                else
                {
                    Log.Error($"Could not parse chromium build number '{version}'");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not get expected chromium build number: " + ex.Message);
            }
            return 0;
        }

        private static int GetExpectedChromiumBuildNumberCefSharp(string dllName)
        {
            try
            {
                var directory = Path.GetDirectoryName(dllName);
                var fileNameWithExt = Path.GetFileName(dllName);

                var arch = RuntimeInformation.ProcessArchitecture.ToString();
                fileNameWithExt = fileNameWithExt?
                    .Replace("Chromely.CefSharp.Winapi", Path.Combine(arch, "CefSharp.Core"));

                if (directory != null) dllName = Path.Combine(directory, fileNameWithExt);
                var assembly = Assembly.LoadFrom(dllName);
                var types = assembly.GetTypes();
                var type = types.FirstOrDefault(t => t.Name == "Cef");
                var versionProperty = type?.GetProperty("CefVersion");
                var version = versionProperty?.GetValue(null).ToString();
                if (!string.IsNullOrEmpty(version)
                    && int.TryParse(version.Split('.')[1], out var build))
                {
                    return build;
                }
                if (type == null)
                {
                    Log.Error("Could not get expected chromium build number: Unable to load Cef.CefVersion");
                }
                else
                {
                    Log.Error($"Could not parse chromium build number '{version}'");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not get expected chromium build number: " + ex.Message);
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
            var osName = Environment.OSVersion.VersionString;
            return osName.ToLower().Contains("darwin");
        }

        /// <summary>
        /// Returns the current platform's default api to use.
        /// </summary>
        public static ChromelyHostApi DefaultHostApi
        {
            get
            {
                switch (Platform)
                {
                    case ChromelyPlatform.Windows:
                        return ChromelyHostApi.Winapi;
                    case ChromelyPlatform.Linux:
                        return ChromelyHostApi.Gtk;
                    case ChromelyPlatform.MacOSX:
                        return ChromelyHostApi.Libui;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns the name of the assembly dll
        /// containing wrapper specific implementation
        /// of 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public static string GetWrapperAssemblyName(ChromelyCefWrapper wrapper)
        {
            var coreAssembly = typeof(ChromelyRuntime).Assembly;
            var path = Path.GetDirectoryName(new Uri(coreAssembly.CodeBase).LocalPath) ?? ".";

            var wrapperApi = (wrapper == ChromelyCefWrapper.CefSharp)
                ? ChromelyHostApi.Winapi.ToString()
                : DefaultHostApi.ToString();
            
            var dllName = Path.Combine(path, $"Chromely.{wrapper}.{wrapperApi}.dll");
            return dllName;
        }
        
        
        
    }
}