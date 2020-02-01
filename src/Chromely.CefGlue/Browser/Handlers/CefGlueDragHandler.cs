// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDragHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Configuration;
using System.Drawing;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueDragHandler : CefDragHandler
    {
        private static readonly object objLock = new object();
        private readonly IChromelyConfiguration _config;

        public CefGlueDragHandler(IChromelyConfiguration config)
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
                    foreach (var region in regions)
                    {
                        var rect = new Rectangle(region.Bounds.X, region.Bounds.Y, region.Bounds.Width, region.Bounds.Height);

                        if (region.Draggable)
                        {
                            // This will use the -webkit-app-region: drag parameter 
                            // For example if above sample is used will be set to
                            // framelessOption.DraggableHeight- 32
                            framelessOption.DraggableHeight = rect.Height;
                        }
                        else
                        {
                            // This will use the -webkit-app-region: no-dragparameter 
                            // For example if above sample is used will be set to
                            // framelessOption.NonDraggableRightOffsetWidth - 140
                            framelessOption.NonDraggableRightOffsetWidth = rect.Width;
                        }
                    }
                }
            }
        }
    }
}
