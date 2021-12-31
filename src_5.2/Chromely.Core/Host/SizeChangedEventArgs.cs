// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Host
{
    public class SizeChangedEventArgs : EventArgs
    {
        public SizeChangedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public IntPtr GdkHandle { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
