// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// Window creation options.
/// </summary>
public interface IWindowOptions
{
    /// <summary>
    /// Gets or sets the application title - used as window title
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Gets or sets the path to application icon file.
    /// </summary>
    /// <remarks>
    /// Relative from application directory.
    /// Note: Windows requires ICO files while Linus needs PNG format.
    /// </remarks>
    string RelativePathToIconFile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application window is resizeable.
    /// </summary>
    bool DisableResizing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Minimum and Maximum window buttons are disabled.
    /// </summary>
    bool DisableMinMaximizeControls { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application should be created in Kiosk mode.
    /// </summary>
    bool KioskMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application window should be centered when created.
    /// </summary>
    bool StartCentered { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application window should be fullscreen when created.
    /// </summary>
    bool Fullscreen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application window should be frameless when created.
    /// </summary>
    bool WindowFrameless { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application window should use custom style on creation.
    /// </summary>
    bool UseCustomStyle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application should use CEF message loop.
    /// </summary>
    /// <remarks>
    /// Applies to Windows ONLY. Will be ignored for Linux and MacOS.
    /// </remarks>
    bool UseOnlyCefMessageLoop { get; set; }

    /// <summary>
    /// Gets or sets the minimum size of the application window.
    /// </summary>
    /// <remarks>
    /// If the value is not set, it will be ignored. Window will be size set by the "Size" property.
    /// </remarks>
    Size MinimumSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum size of the application window.
    /// </summary>
    /// <remarks>
    /// If the value is not set, it will be ignored. Window will be size set by the "Size" property.
    /// </remarks>
    Size MaximumSize { get; set; }

    /// <summary>
    /// Get or sets application window custom style using <see cref="WindowCustomStyle"/>.
    /// </summary>
    /// <remarks>
    /// Usually used for Windows application ONLY.
    /// </remarks>
    WindowCustomStyle? CustomStyle { get; set; }

    /// <summary>
    /// Gets or sets the application window position using <see cref="WindowPosition"/>.
    /// </summary>
    WindowPosition Position { get; set; }

    /// <summary>
    /// Gets or sets the application window size using <see cref="WindowSize"/>.
    /// </summary>
    WindowSize Size { get; set; }

    /// <summary>
    /// Gets or sets the application window state using <see cref="WindowState"/>.
    /// </summary>
    WindowState WindowState { get; set; }

    /// <summary>
    /// Gets or sets the application window frameless options using <see cref="FramelessOption"/>.
    /// </summary>
    FramelessOption FramelessOption { get; set; }
}