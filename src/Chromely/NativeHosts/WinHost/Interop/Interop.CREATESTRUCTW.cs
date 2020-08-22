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
            public unsafe struct CREATESTRUCTW
            {
                public IntPtr lpCreateParams;
                public IntPtr hInstance;
                IntPtr hMenu;
                IntPtr hwndParent;
                public int cy;
                public int cx;
                public int y;
                public int x;
                public long style;
                public char* lpszName;
                public char* lpszClass;
                public uint dwExStyle;
            }
        }
    }
}

