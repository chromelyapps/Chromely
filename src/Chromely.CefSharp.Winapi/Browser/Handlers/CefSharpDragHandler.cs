using CefSharp;
using CefSharp.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
