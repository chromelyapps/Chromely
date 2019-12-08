using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public interface IWindowOptions
    {
        bool UseWindowCustomStyle { get; set; }
        bool WindowCenterScreen { get; set; }
        WindowCustomStyle WindowCustomStyle { get; set; }
        bool WindowFrameless { get; set; }
        string WindowIconFile { get; set; }
        bool WindowKioskMode { get; set; }
        bool WindowNoMinMaxBoxes { get; set; }
        bool WindowNoResize { get; set; }
        WindowState WindowState { get; set; }
        string WindowTitle { get; set; }

        WindowPosition Position { get; set; }
        WindowSize Size { get; set; }
    }
}