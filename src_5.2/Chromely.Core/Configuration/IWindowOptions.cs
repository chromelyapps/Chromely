// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

public interface IWindowOptions
{
    /// <summary>
    /// Application title - used as window title
    /// </summary>
    string Title { get; set; }
    /// <summary>
    /// Path to application icon file.
    /// Relative from application directory.
    /// Note:
    /// Windows requires ICO files while Linus needs PNG format.
    /// </summary>
    string RelativePathToIconFile { get; set; }
    bool DisableResizing { get; set; }
    bool DisableMinMaximizeControls { get; set; }
    bool KioskMode { get; set; }
    bool StartCentered { get; set; }
    bool Fullscreen { get; set; }
    bool WindowFrameless { get; set; }
    bool UseCustomStyle { get; set; }

    /* Windows ONLY */
    bool UseOnlyCefMessageLoop { get; set; }

    Size MinimumSize { get; set; }
    Size MaximumSize { get; set; }
    WindowCustomStyle? CustomStyle { get; set; }
    WindowPosition Position { get; set; }
    WindowSize Size { get; set; }
    WindowState WindowState { get; set; }
    FramelessOption FramelessOption { get; set; }
}