// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode)]
            public static extern bool IsWindows7OrGreater();

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode)]
            public static extern bool IsWindows8OrGreater();

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode)]
            public static extern bool IsWindows8Point1OrGreater();

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode)]
            public static extern bool IsWindows10OrGreater();

            private static RTL_OSVERSIONINFOEX s_versionInfo = InitVersion();

            private static RTL_OSVERSIONINFOEX InitVersion()
            {
                // We use RtlGetVersion as it isn't subject to version lie. GetVersion
                // won't tell you the real version unless the launching exe is manifested
                // with the latest OS version.

                RTL_OSVERSIONINFOEX info = new RTL_OSVERSIONINFOEX();
                RtlGetVersion(ref info);
                return info;
            }

            /// <summary>
            ///  Is Windows 10 Anniversary Update or later. (Redstone 1, build 14393, version 1607)
            /// </summary>
            public static bool IsWindows10_1607OrGreater
                => s_versionInfo.dwMajorVersion >= 10 && s_versionInfo.dwBuildNumber >= 14393;

            /// <summary>
            ///  Is Windows 10 Creators Update or later. (Redstone 2, build 15063, version 1703)
            /// </summary>
            public static bool IsWindows10_1703OrGreater
                => s_versionInfo.dwMajorVersion >= 10 && s_versionInfo.dwBuildNumber >= 15063;

            /// <summary>
            ///  Is Windows 8.1 or later.
            /// </summary>
            public static bool IsWindows8_1OrGreater
                => s_versionInfo.dwMajorVersion >= 10
                    || (s_versionInfo.dwMajorVersion == 6 && s_versionInfo.dwMinorVersion == 3);

            [DllImport(Libraries.NtDll)]
            public static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public unsafe struct RTL_OSVERSIONINFOEX
            {
                internal uint dwOSVersionInfoSize;
                internal uint dwMajorVersion;
                internal uint dwMinorVersion;
                internal uint dwBuildNumber;
                internal uint dwPlatformId;
                internal fixed char szCSDVersion[128];
            }
        }
    }
}
