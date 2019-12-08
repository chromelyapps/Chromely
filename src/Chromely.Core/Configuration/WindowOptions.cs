using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core.Configuration
{
    public class WindowOptions : IWindowOptions
    {
        public int WindowLeft { get; set; }
        public int WindowTop { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public bool WindowNoResize { get; set; }

        public bool WindowNoMinMaxBoxes { get; set; }
        public bool WindowFrameless { get; set; }
        public bool WindowCenterScreen { get; set; }
        public bool WindowKioskMode { get; set; }

        public WindowState WindowState { get; set; }
        public string WindowTitle { get; set; }
        public string WindowIconFile { get; set; }
        public WindowCustomStyle WindowCustomStyle { get; set; }
        public bool UseWindowCustomStyle { get; set; }

        public WindowOptions()
        {
            WindowLeft = 0;
            WindowTop = 0;
            WindowWidth = 1200;
            WindowHeight = 900;
            WindowNoResize = false;
            WindowNoMinMaxBoxes = false;
            WindowFrameless = false;
            WindowCenterScreen = true;
            WindowKioskMode = false;
            WindowState = WindowState.Normal;
            WindowTitle = "chromely";
            WindowIconFile = "chromely.ico";
        }
    }
}
