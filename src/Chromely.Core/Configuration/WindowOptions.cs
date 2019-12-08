using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public class WindowOptions : IWindowOptions
    {
        /// <summary>Gets or sets the title of the window. If you leave them blank than the AppName will be used</summary>
        public string Title { get; set; }

        /// <summary>Gets or sets the relative path to icon that will be displayed on the window.</summary>
        public string RelativePathToIconFile { get; set; }

        /// <summary>Gets or sets a value indicating whether resizing should be disabled.</summary>
        public bool DisableResizing { get; set; }

        /// <summary>Gets or sets a value indicating whether the minimize and maximize controls should be disabled.</summary>
        public bool DisableMinMaximizeControls { get; set; }

        /// <summary>Gets or sets a value indicating whether kiosk mode is enabled or not.</summary>
        public bool KioskMode { get; set; }

        /// <summary>Gets or sets a value indicating whether the window should be centered when starting up for the first time.</summary>
        public bool StartCentered { get; set; }

        //TODO write summary for WindowFrameless prop
        public bool WindowFrameless { get; set; }

        /// <summary>Gets or sets a value indicating whether a custom style will be used.</summary>
        public bool UseCustomStyle { get; set; }

        /// <summary>Gets or sets the custom style.</summary>
        public WindowCustomStyle CustomStyle { get; set; }

        /// <summary>Gets or sets the position of the window.</summary>
        public WindowPosition Position { get; set; }

        /// <summary>Gets or sets the size of the window.</summary>
        public WindowSize Size { get; set; }

        /// <summary>Gets or sets the WindowState of the window.</summary>
        public WindowState WindowState { get; set; }

        public WindowOptions()
        {
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
        }
    }
}