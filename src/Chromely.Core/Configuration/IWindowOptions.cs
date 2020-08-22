// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using System.Drawing;

namespace Chromely.Core.Configuration
{
    public interface IWindowOptions
    {
        string Title { get; set; }
        string RelativePathToIconFile { get; set; }
        bool DisableResizing { get; set; }
        bool DisableMinMaximizeControls { get; set; }
        bool KioskMode { get; set; }
        bool StartCentered { get; set; }
        bool Fullscreen { get; set; }
        bool WindowFrameless { get; set; }
        bool UseCustomStyle { get; set; }
        Size MinimumSize { get; set; }
        Size MaximumSize { get; set; }
        WindowCustomStyle CustomStyle { get; set; }
        WindowPosition Position { get; set; }
        WindowSize Size { get; set; }
        WindowState WindowState { get; set; }
        FramelessOption FramelessOption { get; set; }
    }
}