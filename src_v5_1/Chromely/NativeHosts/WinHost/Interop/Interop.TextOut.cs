// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class Gdi32
        {
            [DllImport(Libraries.Gdi32)]
            public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

            [DllImport(Libraries.Gdi32)]
            public static extern IntPtr CreateSolidBrush(uint crColor);

            [DllImport(Libraries.Gdi32)]
            public static extern uint SetBkColor(IntPtr hdc, int crColor);

            [DllImport(Libraries.Gdi32)]
            public static extern uint SetTextColor(IntPtr hdc, int crColor);
        }
    }
}
