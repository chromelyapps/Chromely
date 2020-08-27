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
            [DllImport(Libraries.User32, ExactSpelling = true)]
            public static extern int SetWindowRgn(IntPtr hwnd, IntPtr hrgn, BOOL fRedraw);

            [DllImport(Libraries.Gdi32, SetLastError = true)]
            public static extern IntPtr CreateRectRgnIndirect([In] ref RECT lprc);
        }
    }
}
