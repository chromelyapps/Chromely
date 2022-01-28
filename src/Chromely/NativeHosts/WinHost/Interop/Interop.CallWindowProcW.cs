// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Chromely;

public static partial class Interop
{
    public static partial class User32
    {
        internal delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport(Libraries.User32)]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(Libraries.User32)]
        internal static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(Libraries.User32, ExactSpelling = true)]
        internal static extern IntPtr CallWindowProcW(
            IntPtr wndProc,
            IntPtr hWnd,
            WM msg,
            IntPtr wParam,
            IntPtr lParam);
    }
}