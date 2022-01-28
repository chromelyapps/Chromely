// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration
{
    public struct WindowSize
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSize( int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}