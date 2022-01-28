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
            public enum StockObject : int
            {
                WHITE_BRUSH = 0,
                LTGRAY_BRUSH = 1,
                GRAY_BRUSH = 2,
                DKGRAY_BRUSH = 3,
                BLACK_BRUSH = 4,
                NULL_BRUSH = 5,
                HOLLOW_BRUSH = NULL_BRUSH,
                WHITE_PEN = 6,
                BLACK_PEN = 7,
                NULL_PEN = 8,
                OEM_FIXED_FONT = 10,
                ANSI_FIXED_FONT = 11,
                ANSI_VAR_FONT = 12,
                SYSTEM_FONT = 13,
                DEVICE_DEFAULT_FONT = 14,
                DEFAULT_PALETTE = 15,
                SYSTEM_FIXED_FONT = 16,
                DEFAULT_GUI_FONT = 17,
                DC_BRUSH = 18,
                DC_PEN = 19,
            }

            [DllImport(Libraries.Gdi32, ExactSpelling = true)]
            public static extern IntPtr GetStockObject(StockObject nIndex);
        }
    }
}
