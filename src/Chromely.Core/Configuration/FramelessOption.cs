// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chromely.Core.Configuration 
{
    /// <summary> Options associated with the Frameless mode. </summary>
    public class FramelessOption
    {
        public bool UseWebkitAppRegions { get; set; }

        /// <summary> Callback function to determine if the point is within the draggable area. </summary>
        /// <value> True if the click point is within the draggable area. </value>
        public Func<Point, bool> IsDraggable { get; set; }

        /// <summary> List of draggable areas. </summary>
        /// <value> The drag zones. </value>
        public List<DragZoneConfiguration> DragZones { get; set; }
        public int ResizeGrip { get; set; }

        /// <summary> Default constructor. </summary>
        public FramelessOption()
        {
            ResizeGrip = 1;
            UseWebkitAppRegions = false;
            IsDraggable = null;
            DragZones = new List<DragZoneConfiguration>();
        }
    }
}
