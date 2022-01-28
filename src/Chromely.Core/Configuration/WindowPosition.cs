// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// Application window position - left, top.
/// </summary>
public struct WindowPosition
{
    /// <summary>
    /// Gets the X position (left).
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the Y position (top).
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="WindowPosition"/>.
    /// </summary>
    /// <param name="x">The left (x) window position.</param>
    /// <param name="y">The right (y) window position.</param>
    public WindowPosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}