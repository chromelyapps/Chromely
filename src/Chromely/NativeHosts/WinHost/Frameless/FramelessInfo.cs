// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using static Chromely.Interop;

namespace Chromely.NativeHost
{
    public class FramelessInfo
    {
        public FramelessInfo(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public IntPtr Handle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RECT Region { get; set; }
        public bool ResizeInMotion { get; set; }
        public bool IsThemeEnabled { get; set; }
        public bool IsCompositionEnabled { get; set; }
    }
}
