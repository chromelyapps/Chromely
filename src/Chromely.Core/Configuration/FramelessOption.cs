using System;
using System.Collections.Generic;
using System.Drawing;
using Chromely.Core.Host;

namespace Chromely.Core.Configuration 
{

    /// <summary> Options associated with the Frameless mode. </summary>
    public class FramelessOption 
    {
        public bool UseDefaultFramelessController { get; set; }
        public bool UseWebkitAppRegions { get; set; }

        /// <summary> Callback function to determine if the point is within the draggable area. </summary>
        /// <value> True if the click point is within the draggable area. </value>
        public Func<IChromelyNativeHost, Point, bool> IsDraggable { get; set; }

        /// <summary> List of draggable areas. </summary>
        /// <value> The drag zones. </value>
        public List<DragZoneConfiguration> DragZones { get; set; }

        /// <summary> Default constructor. </summary>
        public FramelessOption() 
        {
            UseDefaultFramelessController = true;
            UseWebkitAppRegions = false;
            DragZones = new List<DragZoneConfiguration>();
            DragZones.Add(new DragZoneConfiguration(32,0,0,140));
            IsDraggable = IsDraggableCallbackFunc;
        }

        /// <summary>
        ///     Default callback function to determine if we're located within the drag region.
        /// </summary>
        /// <param name="nativeHost"> The Chromely native host interface. </param>
        /// <param name="point">      The click point. </param>
        /// <returns> True if in the drag region, false if not. </returns>
        public bool IsDraggableCallbackFunc(IChromelyNativeHost nativeHost, Point point) 
        {
            var size = nativeHost.GetWindowClientSize();
            var scale = nativeHost.GetWindowDpiScale();

            var in_zone = false;
            foreach (var zone in DragZones) {
                if (zone.InZone(size, point, scale))
                    in_zone = true;
            }
            return in_zone;
        }
    }
}
