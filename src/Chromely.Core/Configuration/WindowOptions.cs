// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <inheritdoc/>
public class WindowOptions : IWindowOptions
{
    /// <summary>
    /// Initializes a new instance of <see cref="WindowOptions"/>.
    /// </summary>
    public WindowOptions()
    {
        UseOnlyCefMessageLoop = false;
        DisableResizing = false;
        DisableMinMaximizeControls = false;
        WindowFrameless = false;
        StartCentered = true;
        KioskMode = false;
        WindowState = WindowState.Normal;
        Title = "My Chromely App";
        RelativePathToIconFile = "chromely.ico";

        Size = new WindowSize(1200, 900);
        Position = new WindowPosition(0, 0);
        FramelessOption = new FramelessOption();
    }

    /// <inheritdoc/>
    public string Title { get; set; }

    /// <inheritdoc/>
    public string RelativePathToIconFile { get; set; }

    /// <inheritdoc/>
    public bool DisableResizing { get; set; }

    /// <inheritdoc/>
    public bool DisableMinMaximizeControls { get; set; }

    /// <inheritdoc/>
    public bool KioskMode { get; set; }

    /// <inheritdoc/>
    public bool StartCentered { get; set; }

    /// <inheritdoc/>
    public bool Fullscreen { get; set; }

    /// <inheritdoc/>
    public bool WindowFrameless { get; set; }

    /// <inheritdoc/>
    public bool UseCustomStyle { get; set; }

    /// <inheritdoc/>
    public bool UseOnlyCefMessageLoop { get; set; }

    /// <inheritdoc/>
    public Size MinimumSize { get; set; }

    /// <inheritdoc/>
    public Size MaximumSize { get; set; }

    /// <inheritdoc/>
    public WindowCustomStyle? CustomStyle { get; set; }

    /// <inheritdoc/>
    public WindowPosition Position { get; set; }

    /// <inheritdoc/>
    public WindowSize Size { get; set; }

    /// <inheritdoc/>
    public WindowState WindowState { get; set; }

    /// <inheritdoc/>
    public FramelessOption FramelessOption { get; set; }

}