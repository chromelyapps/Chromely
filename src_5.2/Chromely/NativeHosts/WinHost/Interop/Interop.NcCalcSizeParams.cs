// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public unsafe struct NcCalcSizeParams
            {
                public NcCalcSizeRegionUnion Region;
                public WINDOWPOS* Position;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct NcCalcSizeRegionUnion
            {
                [FieldOffset(0)] public NcCalcSizeInput Input;
                [FieldOffset(0)] public NcCalcSizeOutput Output;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct NcCalcSizeInput
            {
                public RECT TargetWindowRect;
                public RECT CurrentWindowRect;
                public RECT CurrentClientRect;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct NcCalcSizeOutput
            {
                public RECT TargetClientRect;
                public RECT DestRect;
                public RECT SrcRect;
            }

        }
    }
}

