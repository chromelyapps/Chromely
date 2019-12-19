using Chromely.Core.Host;

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
        bool WindowFrameless { get; set; }
        bool UseCustomStyle { get; set; }
        WindowCustomStyle CustomStyle { get; set; }
        WindowPosition Position { get; set; }
        WindowSize Size { get; set; }
        WindowState WindowState { get; set; }
    }
}