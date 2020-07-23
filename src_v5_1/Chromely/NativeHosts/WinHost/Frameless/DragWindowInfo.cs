// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using System;
using System.Drawing;
using System.Linq;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public class DragWindowInfo
    {
        protected FramelessOption _framelessOption;
        public DragWindowInfo(IntPtr parentHandle, FramelessOption framelessOption)
        {
            MainWindowHandle = parentHandle;
            _framelessOption = framelessOption;
        }

        public IntPtr MainWindowHandle { get; set; }
        public bool DragInitiated { get; set; }
        public POINT DragPoint { get; set; }
        public POINT DragWindowLocation { get; set; }

        public void Reset()
        {
            DragInitiated = false;
            DragPoint = new POINT();
            DragWindowLocation = new POINT();
        }

        public bool IsCursorInDraggableRegion(ref POINT cursorLoc, ref POINT windowTopLeftPoint)
        {
            // Cache location for zone
            var zonePt = new Point(cursorLoc.x, cursorLoc.y);

            var rectCurCursorLoc = new Point(cursorLoc.x, cursorLoc.y);
            ClientToScreen(MainWindowHandle, ref rectCurCursorLoc);
            cursorLoc = new POINT(rectCurCursorLoc.X, rectCurCursorLoc.Y);

            RECT rect = new RECT();
            GetWindowRect(MainWindowHandle, ref rect);

            Rectangle rectangle = rect;

            Point curCursorLoc = new Point(rectCurCursorLoc.X, rectCurCursorLoc.Y);

            // Mouse must be within Window
            if (!rectangle.Contains(curCursorLoc))
            {
                return false;
            }

            // If no drag zones are defined, by default we drag the entire window
            if (_framelessOption?.DragZones == null)
            {
                windowTopLeftPoint = new POINT(rectangle.Left, rectangle.Top);
                return true;
            }

            // This is using webkitapp regions as defined in the DragHandler
            if (_framelessOption.UseWebkitAppRegions)
            {
                if (_framelessOption.IsDraggable != null)
                {
                    bool isDraggable = _framelessOption.IsDraggable(zonePt);
                    if (isDraggable)
                    {
                        windowTopLeftPoint = new POINT(rectangle.Left, rectangle.Top);
                        return true;
                    }
                }

                return false;
            }

            // If no drag zones are defined, by default we drag the entire window
            if (!_framelessOption.DragZones.Any())
            {
                windowTopLeftPoint = new POINT(rectangle.Left, rectangle.Top);
                return true;
            }

            foreach (var zone in _framelessOption.DragZones)
            {
                var size = GetWindowClientSize();
                var scale = GetWindowDpiScale();
                if (zone.InZone(size, zonePt, scale))
                {
                    windowTopLeftPoint = new POINT(rectangle.Left, rectangle.Top);
                    return true;
                }
            }

            return false;
        }

        private Size GetWindowClientSize()
        {
            var size = new Size();
            if (MainWindowHandle != IntPtr.Zero)
            {
                RECT rect = new RECT();
                GetClientRect(MainWindowHandle, ref rect);
                size.Width = rect.Width;
                size.Height = rect.Height;
            }

            return size;
        }

        private float GetWindowDpiScale()
        {
            const int StandardDpi = 96;
            float scale = 1;
            var hdc = GetDC(MainWindowHandle);
            try
            {
                var dpi = Gdi32.GetDeviceCaps(hdc, Gdi32.DeviceCapability.LOGPIXELSY);
                scale = (float)dpi / StandardDpi;
            }
            finally
            {
                ReleaseDC(MainWindowHandle, hdc);
            }
            return scale;
        }
    }
}
