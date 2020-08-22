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
            public static extern BOOL PostMessageW(
                IntPtr hWnd,
                WM Msg,
                IntPtr wParam = default,
                IntPtr lParam = default);

            public static BOOL PostMessageW(
                HandleRef hWnd,
                WM Msg,
                IntPtr wParam = default,
                IntPtr lParam = default)
            {
                BOOL result = PostMessageW(hWnd.Handle, Msg, wParam, lParam);
                GC.KeepAlive(hWnd.Wrapper);
                return result;
            }
        }
    }
}


