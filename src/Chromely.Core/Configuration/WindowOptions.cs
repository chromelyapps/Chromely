using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public class WindowOptions : IWindowOptions
    {
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

        public WindowPosition Position { get; set; }
        public WindowSize Size { get; set; }

        public WindowOptions()
        {
            WindowNoResize = false;
            WindowNoMinMaxBoxes = false;
            WindowFrameless = false;
            WindowCenterScreen = true;
            WindowKioskMode = false;
            WindowState = WindowState.Normal;
            WindowTitle = "chromely";
            WindowIconFile = "chromely.ico";

            Size = new WindowSize(1200, 900);
            Position = new WindowPosition(0, 0);
        }
    }
}