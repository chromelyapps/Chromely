// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary> Represents a drag zone on the main window. </summary>
public class DragZoneConfiguration
{
    /// <summary> The height of the drag zone, typically at the top of the window. </summary>
    /// <value> The height of the drag zone. </value>
    public int Height { get; set; }

    /// <summary> The offset from the top of the frame to start the drag area. </summary>
    /// <value> The offset from the top of the frame. </value>
    public int TopOffset { get; set; }

    /// <summary> The offset from the left of the frame to start the drag area. </summary>
    /// <value> The offset from the left of the frame. </value>
    public int LeftOffset { get; set; }

    /// <summary> The offset from the right of the frame to start the drag area. </summary>
    /// <value> The offset from the right of the frame. </value>
    public int RightOffset { get; set; }

    /// <summary> Constructor. </summary>
    /// <param name="height">      The height of the drag zone. </param>
    /// <param name="topoffset">   The offset from the top of the frame. </param>
    /// <param name="leftoffset">  The offset from the left of the frame. </param>
    /// <param name="rightoffset"> The offset from the right of the frame. </param>
    public DragZoneConfiguration(int height, int topoffset, int leftoffset, int rightoffset)
    {
        Height = height;
        TopOffset = topoffset;
        LeftOffset = leftoffset;
        RightOffset = rightoffset;
    }

    /// <summary> Determines if the point is in the zone. </summary>
    /// <param name="size">  The size of the area to calculate the offsets. </param>
    /// <param name="point"> The point. </param>
    /// <param name="scale"> The scale to use for dpi / desktop scale compensation. </param>
    /// <returns> True if in the zone. </returns>
    public bool InZone(Size size, Point point, float scale)
    {
        var HeightScaled = Height * scale;
        var TopOffsetScaled = TopOffset * scale;
        var LeftOffsetScaled = LeftOffset * scale;
        var RightOffsetScaled = RightOffset * scale;

        var point_rightoffset = size.Width - point.X;
        // Define a bounding box for the drag area
        return point.Y <= (HeightScaled + TopOffsetScaled) &&
               point.Y >= TopOffsetScaled &&
               point.X >= LeftOffsetScaled &&
               point_rightoffset > RightOffsetScaled;
    }
}