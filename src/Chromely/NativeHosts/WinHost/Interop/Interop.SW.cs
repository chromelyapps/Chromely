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
            ///  Show window flags for <see cref="ShowWindow(IntPtr, SW)"/> and
            ///  <see cref="Shell32.ShellExecuteW(IntPtr, string, string, string, string, User32.SW)"/>
            /// </summary>
            public enum SW : int
            {
                Hide = 0,
                Normal = 1,
                Minimized = 2,
                Maximized = 3,

                HIDE = 0,
                SHOWNORMAL = 1,
                NORMAL = 1,
                SHOWMINIMIZED = 2,
                SHOWMAXIMIZED = 3,
                MAXIMIZE = 3,
                SHOWNOACTIVATE = 4,
                SHOW = 5,
                MINIMIZE = 6,
                SHOWMINNOACTIVE = 7,
                SHOWNA = 8,
                RESTORE = 9,
                SHOWDEFAULT = 10,
                FORCEMINIMIZE = 11,
                MAX = 11
            }
        }
    }
}
