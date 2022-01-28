// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Custom window styles.
/// </summary>
public class WindowCustomStyle
{
    /// <summary>
    /// Initializes a new instance of <see cref="WindowCustomStyle"/>.
    /// </summary>
    /// <param name="styles">Window Styles.</param>
    /// <param name="exStyles">Extended Window Styles</param>
    public WindowCustomStyle(int styles, int exStyles)
    {
        WindowStyles = styles;
        WindowExStyles = exStyles;
    }

    /// <summary>
    /// Gets or sets the Window Styles.
    /// </summary>
    public int WindowStyles { get; set; }

    /// <summary>
    /// Gets or sets the Extended Window Styles.
    /// </summary>
    public int WindowExStyles { get; set; }

    /// <summary>
    /// Checks if the styles set are valid.
    /// </summary>
    /// <returns>true if valid, othewise false.</returns>
    public bool IsValid()
    {
        return ((WindowStyles != 0) && (WindowExStyles != 0));
    }
}