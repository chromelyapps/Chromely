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
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public unsafe struct MONITORINFOEXW
            {
                public uint cbSize;
                public RECT rcMonitor;
                public RECT rcWork;
                public MONITORINFOF dwFlags;
                public fixed char szDevice[32];

                public MONITORINFOEXW(Boolean? filler) : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
                {
                    cbSize = (UInt32)(Marshal.SizeOf(typeof(MONITORINFOEXW)));
                }
            }
        }
    }
}
