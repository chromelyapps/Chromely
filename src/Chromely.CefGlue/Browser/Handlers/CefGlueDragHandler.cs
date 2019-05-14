// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDragHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    internal class CefGlueDragHandler : CefDragHandler
    {
      //  public Region DragRegion = new Region();

        protected override bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
        {
            return false;
        }

        protected override void OnDraggableRegionsChanged(CefBrowser browser, CefDraggableRegion[] regions)
        {
            if (!browser.IsPopup)
            {
                //lock (DragRegion)
                //{
                //    DragRegion = null;
                //    foreach (var region in regions)
                //    {
                //        var rect = new Rectangle(region.X, region.Y, region.Width, region.Height);

                //        if (DragRegion == null)
                //        {
                //            DragRegion = new Region(rect);
                //        }
                //        else
                //        {
                //            if (region.Draggable)
                //            {
                //                DragRegion.Union(rect);
                //            }
                //            else
                //            {
                //                DragRegion.Exclude(rect);
                //            }
                //        }
                //    }
                //}
            }
        }
    }
}
