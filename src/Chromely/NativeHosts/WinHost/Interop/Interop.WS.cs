// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            /// <summary>
            ///  Window styles
            /// </summary>
            [Flags]
            public enum WS : uint
            {
                BORDER = 0x800000,
                CAPTION = 0xc00000,
                CHILD = 0x40000000,
                CLIPCHILDREN = 0x2000000,
                CLIPSIBLINGS = 0x4000000,
                DISABLED = 0x8000000,
                DLGFRAME = 0x400000,
                GROUP = 0x20000,
                HSCROLL = 0x100000,
                MAXIMIZE = 0x1000000,
                MAXIMIZEBOX = 0x10000,
                MINIMIZE = 0x20000000,
                MINIMIZEBOX = 0x20000,
                OVERLAPPED = 0x0,
                OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | SIZEFRAME | MINIMIZEBOX | MAXIMIZEBOX,
                POPUP = 0x80000000u,
                POPUPWINDOW = POPUP | BORDER | SYSMENU,
                SIZEFRAME = 0x40000,
                SYSMENU = 0x80000,
                TABSTOP = 0x10000,
                VISIBLE = 0x10000000,
                VSCROLL = 0x200000,
                THICKFRAME = 0x00040000,
                SIZEBOX = 0x00040000
            }
        }
    }
}
