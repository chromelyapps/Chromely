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
            [DllImport(Libraries.User32, EntryPoint = "SetWindowLong")]
            private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

            [DllImport(Libraries.User32, EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

            // We only ever call this on 32 bit so IntPtr is correct
            [DllImport(Libraries.User32, ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr SetWindowLongW(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

            [DllImport(Libraries.User32, ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr SetWindowLongPtrW(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

            public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newWndProc)
            {
                if (Environment.Is64BitProcess)
                    return SetWindowLongPtr64(new HandleRef(null, hWnd), nIndex, newWndProc);
                else
                    return new IntPtr(SetWindowLong32(new HandleRef(null, hWnd), nIndex, newWndProc.ToInt32()));
            }

            public static IntPtr SetWindowLong(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong)
            {
                if (IntPtr.Size == 4)
                {
                    return SetWindowLongW(hWnd, nIndex, dwNewLong);
                }
                return SetWindowLongPtrW(hWnd, nIndex, dwNewLong);
            }


            public static IntPtr SetWindowLong(IntPtr hWnd, GWL nIndex, WNDPROC dwNewLong)
            {
                IntPtr pointer = Marshal.GetFunctionPointerForDelegate(dwNewLong);
                IntPtr result = SetWindowLong(hWnd, nIndex, pointer);
                GC.KeepAlive(dwNewLong);
                return result;
            }

            public static IntPtr SetWindowLong(HandleRef hWnd, GWL nIndex, WNDPROCINT dwNewLong)
            {
                IntPtr pointer = Marshal.GetFunctionPointerForDelegate(dwNewLong);
                IntPtr result = SetWindowLong(hWnd.Handle, nIndex, pointer);
                GC.KeepAlive(hWnd.Wrapper);
                return result;
            }

            public static IntPtr SetWindowLong(HandleRef hWnd, GWL nIndex, HandleRef dwNewLong)
            {
                IntPtr result = SetWindowLong(hWnd.Handle, nIndex, dwNewLong.Handle);
                GC.KeepAlive(hWnd.Wrapper);
                GC.KeepAlive(dwNewLong.Wrapper);
                return result;
            }

        }
    }
}
