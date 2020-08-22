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
            [Flags]
            public enum LWA : uint
            {
                COLORKEY = 0x00000001,
                ALPHA = 0x00000002
            }
        }
    }
}
