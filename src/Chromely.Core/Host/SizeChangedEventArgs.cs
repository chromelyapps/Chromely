// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Application host size changed event argument class.
/// </summary>
public class SizeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of <see cref="SizeChangedEventArgs"/>.
    /// </summary>
    /// <param name="width">Host window width.</param>
    /// <param name="height">Host window height.</param>
    public SizeChangedEventArgs(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the host window width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the host window height.
    /// </summary>
    public int Height { get; }
}