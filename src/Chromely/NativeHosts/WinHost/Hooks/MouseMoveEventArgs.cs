// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Drawing;

namespace Chromely.NativeHost
{
    public class MouseMoveEventArgs : EventArgs
    {
        public MouseMoveEventArgs(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public MouseMoveEventArgs(Size deltaSize)
        {
            DeltaX = deltaSize.Width;
            DeltaY = deltaSize.Height;
        }

        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
    }
}
