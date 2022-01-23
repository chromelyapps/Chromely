// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary> 
/// Options associated with the Frameless mode. 
/// </summary>
public class FramelessOption
{
    private static readonly int DRAGZONE_HEIGHT = 32;
    private static readonly int DRAGZONE_TOP_OFFSET = 0;
    private static readonly int DRAGZONE_LEFT_OFFSET = 0;
    private static readonly int DRAGZONE_RIGHT_OFFSET = 140;

    /// <summary> 
    /// Initializes a new instance of <see cref="FramelessOption"/>. 
    /// </summary>
    public FramelessOption()
    {
        UseWebkitAppRegions = false;
        DragZones = new List<DragZoneConfiguration>
        {
            new DragZoneConfiguration(DRAGZONE_HEIGHT, DRAGZONE_TOP_OFFSET, DRAGZONE_LEFT_OFFSET, DRAGZONE_RIGHT_OFFSET)
        };
        IsDraggable = IsDraggableCallbackFunc;
        DblClick = DblClickCallbackFunc;
    }

    /// <summary>
    /// Gets or sets a value indicating whether Webkit region should be used to determine drag zone.
    /// </summary>
    public bool UseWebkitAppRegions { get; set; }

    /// <summary> 
    /// Callback function to determine if the point is within the draggable area. 
    /// </summary>
    /// <value> 
    /// True if the click point is within the draggable area. 
    /// </value>
    public Func<IChromelyNativeHost, Point, bool> IsDraggable { get; set; }

    /// <summary> 
    /// Callback function on double click events from the drag region. 
    /// </summary>
    /// <value> 
    /// The double click callback. 
    /// </value>
    public Action<IChromelyNativeHost> DblClick { get; set; }

    /// <summary> 
    /// List of draggable areas. 
    /// </summary>
    /// <value> 
    /// The drag zones. 
    /// </value>
    public List<DragZoneConfiguration> DragZones { get; set; }

    /// <summary>
    ///  Default callback function to determine if we're located within the drag region.
    /// </summary>
    /// <param name="nativeHost"> The Chromely native host interface. </param>
    /// <param name="point">      The click point. </param>
    /// <returns> True if in the drag region, false if not. </returns>
    public bool IsDraggableCallbackFunc(IChromelyNativeHost nativeHost, Point point)
    {
        var size = nativeHost.GetWindowClientSize();
        var scale = nativeHost.GetWindowDpiScale();

        var in_zone = false;
        foreach (var zone in DragZones)
        {
            if (zone.InZone(size, point, scale))
                in_zone = true;
        }
        return in_zone;
    }

    /// <summary> 
    /// Doubleclicking drag zone callback function. 
    /// </summary>
    /// <param name="nativeHost"> The Chromely native host interface. </param>
    public static void DblClickCallbackFunc(IChromelyNativeHost nativeHost)
    {
        // Toggle between normal (restore) and maximized
        var state = nativeHost.GetWindowState();
        if (state == WindowState.Maximize)
        {
            nativeHost.SetWindowState(WindowState.Normal);
        }
        else
        {
            nativeHost.SetWindowState(WindowState.Maximize);
        }
    }
}