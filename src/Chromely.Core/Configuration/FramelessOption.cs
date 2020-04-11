using System.Drawing;
using Chromely.Core.Host;

namespace Chromely.Core.Configuration
{
    /// <summary> Options associated with the Frameless mode. </summary>
    public class FramelessOption
    {
        public bool UseDefaultFramelessController { get; set; }
        public bool UseWebkitAppRegions { get; set; }

        /// <summary>
        ///     Delegate Callback function to determine if the point is within the draggable area.
        /// </summary>
        /// <param name="nativeHost"> The native host. </param>
        /// <param name="point">      The click point. </param>
        /// <returns> True if the click point is within the draggable area. </returns>
        public delegate bool IsDraggableCallback(IChromelyNativeHost nativeHost, Point point);

        /// <summary> Callback function to determine if the point is within the draggable area. </summary>
        /// <value> True if the click point is within the draggable area. </value>
        public IsDraggableCallback IsDraggable { get; set; }

        /// <summary> The height of the drag zone, typically at the top of the window. </summary>
        /// <value> The height of the drag zone. </value>
        public int DragZoneHeight { get; set; }

        /// <summary> The offset from the top of the frame to start the drag area. </summary>
        /// <value> The offset from the top of the frame. </value>
        public int DragZoneTopOffset { get; set; }

        /// <summary> The offset from the left of the frame to start the drag area. </summary>
        /// <value> The offset from the left of the frame. </value>
        public int DragZoneLeftOffset { get; set; }

        /// <summary> The offset from the right of the frame to start the drag area. </summary>
        /// <value> The offset from the right of the frame. </value>
        public int DragZoneRightOffset { get; set; }


        /// <summary> Default constructor. </summary>
        public FramelessOption()
        {
            UseDefaultFramelessController = true;
            UseWebkitAppRegions = false;
            DragZoneHeight = 32;
            DragZoneTopOffset = 0;
            DragZoneLeftOffset = 0;
            DragZoneRightOffset = 140;
            IsDraggable = (nativeHost, point) => {
                var scalingfactor = nativeHost.GetWindowDpiScalingFactor();
                var size = nativeHost.GetWindowClientSize();

                var scaledpoint = new Point((int) (point.X * scalingfactor), (int) (point.Y * scalingfactor));
                var right = size.Width - scaledpoint.X;

                // Define a bounding box for the drag area
                return scaledpoint.Y <= (DragZoneHeight + DragZoneTopOffset) &&
                       scaledpoint.Y >= DragZoneTopOffset &&
                       scaledpoint.X >= DragZoneLeftOffset &&
                       right > DragZoneRightOffset;
            };
        }
    }
}
