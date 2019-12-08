using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public class WindowOptions : IWindowOptions
    {
        public int WindowLeft { get; set; }
        public int WindowTop { get; set; }
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

        public WindowSize Size { get; set; }

        public WindowOptions()
        {
            WindowLeft = 0;
            WindowTop = 0;
            WindowNoResize = false;
            WindowNoMinMaxBoxes = false;
            WindowFrameless = false;
            WindowCenterScreen = true;
            WindowKioskMode = false;
            WindowState = WindowState.Normal;
            WindowTitle = "chromely";
            WindowIconFile = "chromely.ico";

            Size = new WindowSize(1200, 900);
        }
    }
}