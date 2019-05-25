// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostType.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Host
{
    public static class HostType
    {
        public static bool IsWindow
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                    case PlatformID.Unix:
                    case (PlatformID)128
                        : // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                        return false;

                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return true;
                }

                return false;
            }
        }

        public static bool IsLinux
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Unix:
                    case (PlatformID)128:   // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                        return true;

                    case PlatformID.MacOSX:
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return false;
                }

                return false;
            }
        }

        public static bool IsMac
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                        return true;

                    case PlatformID.Unix:
                    case (PlatformID)128:   // Framework (1.0 and 1.1) didn't include any PlatformID value for Unix, so Mono used the value 128.
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return false;
                }

                return false;
            }
        }
    }
}
