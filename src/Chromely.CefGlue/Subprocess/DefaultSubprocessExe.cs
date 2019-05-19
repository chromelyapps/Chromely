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
using Chromely.Core.Infrastructure;

namespace Chromely.CefGlue.Subprocess
{
    internal class DefaultSubprocessExe
    {
        public static string FulPath
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
                var cpu = Environment.Is64BitOperatingSystem ? "x64" : "x86";

                string fullPath;
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Mac.Subprocess" + "_" + cpu + ".exe");
                        break;

                    case PlatformID.Unix:
                    case (PlatformID)128:   // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Linux.Subprocess" + "_" + cpu + ".exe");
                        break;

                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Win.Subprocess" + "_" + cpu + ".exe");
                        break;

                    default:
                        fullPath = Path.Combine(localFolder ?? string.Empty, "Chromely.CefGlue.Win.Subprocess" + "_" + cpu + ".exe");
                        break;
                }

                try
                {
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }

                    fullPath = null;
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }

                return fullPath;
            }
        }
    }
}
