// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDragHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueDragHandler : CefDragHandler
    {
        public Region DragRegion { get; private set; }

        public CefGlueDragHandler()
        {
            DragRegion = new Region(new Rectangle(0, 0, 0, 0));
        }

        protected override bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
        {
            return false;
        }

        protected override void OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions)
        {
            if (!browser.IsPopup)
            {
                lock (DragRegion)
                {
                    DragRegion = new Region(new Rectangle(0, 0, 0, 0));
                    foreach (var region in regions)
                    {
                        var rect = new Rectangle(region.Bounds.X, region.Bounds.Y, region.Bounds.Width, region.Bounds.Height);

                        if (region.Draggable)
                        {
                            DragRegion.Union(rect);
                        }
                        else
                        {
                            DragRegion.Exclude(rect);
                        }
                    }
                }
            }
        }
    }
}
