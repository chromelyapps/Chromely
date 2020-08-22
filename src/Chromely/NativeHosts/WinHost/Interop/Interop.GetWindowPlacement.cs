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
            [DllImport(Libraries.User32, ExactSpelling = true, EntryPoint = "GetWindowPlacement")]
            private static extern BOOL GetWindowPlacementInternal(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

            public unsafe static BOOL GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl)
            {
                WINDOWPLACEMENT winplacement = new WINDOWPLACEMENT();
                var size = Marshal.SizeOf(winplacement);
                lpwndpl = new WINDOWPLACEMENT
                {

                    length = (uint)size
                };
                return GetWindowPlacementInternal(hWnd, ref lpwndpl);
            }
        }
    }
}
