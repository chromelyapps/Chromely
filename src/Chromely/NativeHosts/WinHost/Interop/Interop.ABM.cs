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
            public enum ABM
            {
                ABM_NEW = 0,
                ABM_REMOVE = 1,
                ABM_QUERYPOS = 2,
                ABM_SETPOS = 3,
                ABM_GETSTATE = 4,
                ABM_GETTASKBARPOS = 5,
                ABM_ACTIVATE = 6,
                ABM_GETAUTOHIDEBAR = 7,
                ABM_SETAUTOHIDEBAR = 8,
                ABM_WINDOWPOSCHANGED = 9,
                ABM_SETSTATE = 10,

                ABM_GETAUTOHIDEBAREX = 11
            }
        }
    }
}
