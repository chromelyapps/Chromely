// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

public struct WindowPosition
{
    public int X { get; }
    public int Y { get; }

    public WindowPosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}