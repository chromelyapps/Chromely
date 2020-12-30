// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Host
{
    public class SizeChangedEventArgs : EventArgs
    {
        public SizeChangedEventArgs(int width, int height, int outerWidth, int outerHeight)
        {
            Width = width;
            Height = height;
            OuterWidth = outerWidth;
            OuterHeight = outerHeight;
        }

        public IntPtr GdkHandle { get; }
        public int Width { get; }
        public int Height { get; }
        public int OuterWidth { get; }
        public int OuterHeight { get; }
    }
}
