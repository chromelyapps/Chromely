// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpDragHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using CefSharp;
using CefSharp.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    internal class CefSharpDragHandler : IDragHandler
    {
        public Region DragRegion = new Region();

        public bool OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
        {
            return false;
        }

        public void OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<DraggableRegion> regions)
        {
            if (!browser.IsPopup)
            {
                lock (DragRegion)
                {
                    DragRegion = null;
                    foreach (var region in regions)
                    {
                        var rect = new Rectangle(region.X, region.Y, region.Width, region.Height);

                        if (DragRegion == null)
                        {
                            DragRegion = new Region(rect);
                        }
                        else
                        {
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
}
