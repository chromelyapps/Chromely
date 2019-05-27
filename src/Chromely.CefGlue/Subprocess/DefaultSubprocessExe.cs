// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSubprocessExe.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Chromely.Core.Infrastructure;

namespace Chromely.CefGlue.Subprocess
{
    /// <summary>
    /// The default subprocess exe.
    /// </summary>
    internal class DefaultSubprocessExe
    {
        /// <summary>
        /// Gets the ful path.
        /// </summary>
        public static string FulPath
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);

                string fullPath;
                switch (Environment.OSVersion.Platform)
                {
                    // TODO
                    case PlatformID.MacOSX:
                        return null;

                    // TODO
                    case PlatformID.Unix:
                    case (PlatformID)128:   // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                        return null;

                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Win.Subprocess.exe");
                        break;

                    default:
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Win.Subprocess.exe");
                        break;
                }

                try
                {
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }

                    return LoadFileFromResource(fullPath);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        private static string ResourcePath
        {
            get
            {
                var isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                var isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

                if (isWin)
                {
                    var cpu = Environment.Is64BitProcess ? "win64" : "win32";
                    return $"{cpu}.Chromely.CefGlue.Win.Subprocess.exe";
                }

                if (isLinux)
                {
                    return $"linux.Chromely.CefGlue.Linux.Subprocess.exe";
                }

                if (isMac)
                {
                    return $"mac.Chromely.CefGlue.Mac.Subprocess.exe";
                }

                return "win64.Chromely.CefGlue.Win.Subprocess.exe";
            }
        }

        /// <summary>
        /// The load file from resource.
        /// </summary>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string LoadFileFromResource(string fullPath)
        {
            string resourcePath = $"Chromely.CefGlue.Subprocess.{ResourcePath}";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
            {
                if (resource != null)
                {
                    using (var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                    }
                }
            }

            if (File.Exists(fullPath))
            {
                return fullPath;
            }

            return null;
        }
    }
}
