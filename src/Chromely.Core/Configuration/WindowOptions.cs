using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public class WindowOptions : IWindowOptions
    {
        public string Title { get; set; }
        public string RelativePathToIconFile { get; set; }
        public bool DisableResizing { get; set; }
        public bool DisableMinMaximizeControls { get; set; }
        public bool KioskMode { get; set; }
        public bool StartCentered { get; set; }
        public bool WindowFrameless { get; set; }
        public bool UseCustomStyle { get; set; }
        public WindowCustomStyle CustomStyle { get; set; }
        public WindowPosition Position { get; set; }
        public WindowSize Size { get; set; }
        public WindowState WindowState { get; set; }

        public WindowOptions()
        {
            DisableResizing = false;
            DisableMinMaximizeControls = false;
            WindowFrameless = false;
            StartCentered = true;
            KioskMode = false;
            WindowState = WindowState.Normal;
            Title = "chromely";
            RelativePathToIconFile = "chromely.ico";

            Size = new WindowSize(1200, 900);
            Position = new WindowPosition(0, 0);
        }
    }
}