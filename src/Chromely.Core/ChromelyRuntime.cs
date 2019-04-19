using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Chromely.Core
{
    /// <summary>
    /// This class provides operating system and runtime information
    /// used to host the application.
    /// </summary>
    public static class ChromelyRuntime
    {
        [DllImport("libcef", EntryPoint = "cef_build_revision", CallingConvention = CallingConvention.Cdecl)]
        private static extern int build_revision();
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public static int GetExpectedChromiumBuildNumber(ChromelyCefWrapper wrapper)
        {
            try
            {
                var dllName = GetWrapperAssemblyName(wrapper).Replace(".CefGlue.Gtk.", ".CefGlue.");
                var assembly = System.Reflection.Assembly.LoadFile(dllName);
                var types = assembly?.GetTypes();
                Type type;
                PropertyInfo versionProperty;
                string version;
                int build;
                switch (wrapper)
                {
                    case ChromelyCefWrapper.CefGlue:
                        type = types?.FirstOrDefault(t => t.Name == "CefRuntime"); 
                        versionProperty = type?.GetProperty("ChromeVersion");
                        version = versionProperty?.GetValue(null).ToString();
                        if (!string.IsNullOrEmpty(version) 
                            && int.TryParse(version.Split('.')[2], out build))
                        {
                            return build;
                        }
                        break;
                
                    case ChromelyCefWrapper.CefSharp:
                        type = types?.FirstOrDefault(t => t.Name == "Cef"); 
                        versionProperty = type?.GetProperty("ChromeVersion");
                        version = versionProperty?.GetValue(null).ToString();
                        if (!string.IsNullOrEmpty(version) 
                            && int.TryParse(version.Split('.')[2], out build))
                        {
                            return build;
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // ignore
            }
            return 0;
        } 

        public static int InstalledCefChromiumBuildNumber
        {
            get
            {
                var build = 0;
                try
                {
                    build = build_revision();
                }
                catch
                {
                    // ignore
                }
                return build;
            }
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

        private static string DefaultWrapperApi
        {
            get
            {
                switch (Platform)
                {
                    case ChromelyPlatform.Windows:
                        return "Winapi";
                    case ChromelyPlatform.Linux:
                        return "Gtk";
                    case ChromelyPlatform.MacOSX:
                        return "Libui";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string GetWrapperAssemblyName(ChromelyCefWrapper wrapper)
        {
            var coreAssembly = typeof(ChromelyRuntime).Assembly;
            var path = Path.GetDirectoryName(coreAssembly.Location) ?? ".";

            var wrapperApi = (wrapper == ChromelyCefWrapper.CefSharp)
                ? "Winapi"
                : DefaultWrapperApi;
            
            var dllName = Path.Combine(path, $"Chromely.{wrapper}.{wrapperApi}.dll");
            return dllName;
        }
        
        
        
    }
}