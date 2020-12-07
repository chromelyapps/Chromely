// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using System.Drawing;
using System.Linq;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultDragHandler : CefDragHandler
    {
        protected static readonly object objLock = new object();
        protected readonly IChromelyConfiguration _config;

        public DefaultDragHandler(IChromelyConfiguration config)
        {
            _config = config;
        }

        protected override bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
        {
            return false;
        }

        /*
            <html>
            <head>
                <title>Draggable Regions Test</title>
                <style>
                    .titlebar {
                        -webkit-app-region: drag;
                        -webkit-user-select: none;
                        position: absolute;
                        top: 0px;
                        left: 50px;
                        width: 100%;
                        height: 32px;
                    }

                    .titlebar-button {
                        -webkit-app-region: no-drag;
                        position: absolute;
                        top: 0px;
                        width: 140px;
                        height: 32px;
                    }
                </style>
            </head>
            <body bgcolor="white">
                Draggable regions can be defined using the -webkit-app-region CSS property.
                <br />In the below example the red region is draggable and the blue sub-region is non-draggable.
                <br />Windows can be resized by default and closed using JavaScript <a href="#" onClick="window.close(); return false;">window.close()</a>.
                <div class="titlebar">
                    <div class="titlebar-button"></div>
                </div>
            </body>
            </html>
         */
        protected override void OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions)
        {
            var framelessOption = _config?.WindowOptions?.FramelessOption;
            if (framelessOption == null ||
                !framelessOption.UseWebkitAppRegions)
            {
                return;
            }

            if (!browser.IsPopup)
            {
                lock (objLock)
                {
                    framelessOption.IsDraggable = (nativeHost, point) =>
                    {
                        var hitNoDrag = regions.Any(r => !r.Draggable && ContainsPoint(r, point));
                        if (hitNoDrag)
                        {
                            return false;
                        }

                        return regions.Any(r => r.Draggable && ContainsPoint(r, point));
                    };
                }
            }
        }

        private bool ContainsPoint(CefDraggableRegion region, Point point)
        {
            return point.X >= region.Bounds.X && point.X <= (region.Bounds.X + region.Bounds.Width)
                && point.Y >= region.Bounds.Y && point.Y <= (region.Bounds.Y + region.Bounds.Height);
        }
    }
}