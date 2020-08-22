// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            private static readonly Version _osVersion = Environment.OSVersion.Version;

            /// <summary>Convert a native integer that represent a color with an alpha channel into a Color struct.</summary>
            /// <param name="color">The integer that represents the color.  Its bits are of the format 0xAARRGGBB.</param>
            /// <returns>A Color representation of the parameter.</returns>
            public static Color ColorFromArgbDword(uint color)
            {
                return Color.FromArgb(
                    (byte)((color & 0xFF000000) >> 24),
                    (byte)((color & 0x00FF0000) >> 16),
                    (byte)((color & 0x0000FF00) >> 8),
                    (byte)((color & 0x000000FF) >> 0));
            }

            public static int RGBtoInt(int red, int green, int blue)
            {
                return (red << 0) | (green << 8) | (blue << 16);
            }

            public static int GET_X_LPARAM(IntPtr lParam)
            {
                return LOWORD(lParam.ToInt32());
            }

            public static int GET_Y_LPARAM(IntPtr lParam)
            {
                return HIWORD(lParam.ToInt32());
            }

            public static int HIWORD(int i)
            {
                return (short)(i >> 16);
            }

            public static int LOWORD(int i)
            {
                return (short)(i & 0xFFFF);
            }

            public static bool IsFlagSet(int value, int mask)
            {
                return 0 != (value & mask);
            }

            public static bool IsFlagSet(uint value, uint mask)
            {
                return 0 != (value & mask);
            }

            public static bool IsFlagSet(long value, long mask)
            {
                return 0 != (value & mask);
            }

            public static bool IsFlagSet(ulong value, ulong mask)
            {
                return 0 != (value & mask);
            }

            public static bool IsOSVistaOrNewer
            {
                get { return _osVersion >= new Version(6, 0); }
            }

            public static bool IsOSWindows7OrNewer
            {
                get { return _osVersion >= new Version(6, 1); }
            }
        }
    }
}
