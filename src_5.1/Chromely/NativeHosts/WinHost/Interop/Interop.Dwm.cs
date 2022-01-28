// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using static Chromely.Interop.UxTheme;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [DllImport(Libraries.Dwmapi, BestFitMapping = false)]
            public static extern int DwmIsCompositionEnabled(out bool enabled);

            [DllImport(Libraries.Dwmapi, PreserveSig = false)]
            public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

            [DllImport(Libraries.Dwmapi)]
            public static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);
        }
    }
}
