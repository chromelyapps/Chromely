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
            public delegate IntPtr SUBCLASSPROC(
              IntPtr hWnd,
              int msg,
              IntPtr wParam,
              IntPtr lParam,
              UIntPtr uIdSubclass,
              UIntPtr dwRefData
            );

            public delegate bool EnumWindowProc(
                IntPtr hWnd,
                IntPtr lParam);

            [System.Runtime.InteropServices.DllImport(Libraries.Comctl32, ExactSpelling = true)]
            public static extern BOOL SetWindowSubclass(
                IntPtr hWnd,
                IntPtr pfnSubclass,
                UIntPtr uIdSubclass,
                UIntPtr dwRefData
            );

            [System.Runtime.InteropServices.DllImport(Libraries.Comctl32, ExactSpelling = true)]
            public static extern BOOL RemoveWindowSubclass(
                IntPtr hWnd,
                IntPtr pfnSubclass,
                UIntPtr uIdSubclass
            );

            [System.Runtime.InteropServices.DllImport(Libraries.Comctl32, ExactSpelling = true)]
            public static extern IntPtr DefSubclassProc(
                IntPtr hWnd,
                int msg,
                IntPtr wParam,
                IntPtr lParam
            );

            [DllImport(Libraries.User32)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowProc callback, IntPtr lParam);
        }
    }
}
