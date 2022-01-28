// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// Application window size - width, height.
/// </summary>
public struct WindowSize
{
    /// <summary>
    /// Gets the application window width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the application window height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="WindowSize"/>.
    /// </summary>
    /// <param name="width">The application window width.</param>
    /// <param name="height">The application window height.</param>
    public WindowSize(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }
}