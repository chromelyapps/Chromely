using System.Drawing;
using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    public class FramelessOption
    {
        public delegate bool IsDraggableCallback(IChromelyNativeHost nativeHost, Point point);

        public FramelessOption()
        {
            UseDefaultFramelessController = true;
            UseWebkitAppRegions = false;
            IsDraggable = (nativeHost, point) =>
            {
                const int DraggableHeight = 32;
                const int NonDraggableRightOffsetWidth = 140;

                var size = nativeHost.GetWindowClientSize();
                var right = size.Width - point.X;
                return point.Y <= DraggableHeight && right > NonDraggableRightOffsetWidth;
            };
        }

        public bool UseDefaultFramelessController { get; set; }
        public bool UseWebkitAppRegions { get; set; }
        public IsDraggableCallback IsDraggable { get; set; }
    }
}