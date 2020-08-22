// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using System;
using System.Drawing;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    abstract partial class NativeHostBase
    {
        protected virtual void SetWindowFullscreen(IntPtr hWnd, int style, int styleEx)
        {
            Size fullscreenSize = new Size();

            if (_windoStylePlacement != null &&
                _windoStylePlacement.FullscreenSize.Width != 0 &&
                _windoStylePlacement.FullscreenSize.Height != 0)
            {
                fullscreenSize = _windoStylePlacement.FullscreenSize;
            }
            else
            {
                var windowHDC = GetDC(hWnd);
                fullscreenSize.Width = Gdi32.GetDeviceCaps(windowHDC, Gdi32.DeviceCapability.HORZRES);
                fullscreenSize.Height = Gdi32.GetDeviceCaps(windowHDC, Gdi32.DeviceCapability.VERTRES);
                ReleaseDC(hWnd, windowHDC);
            }

            SetWindowLong(hWnd, GWL.STYLE, (IntPtr)style);
            SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)styleEx);

            SetWindowPos(hWnd, HWND_TOP, 0, 0, fullscreenSize.Width, fullscreenSize.Height, SWP.NOOWNERZORDER | SWP.FRAMECHANGED);

            if (_windoStylePlacement != null)
            {
                _windoStylePlacement.FullscreenSize = fullscreenSize;
            }
        }

        protected virtual bool IsWindowed(IntPtr hWnd)
        {
            WS dwStyle = (WS)GetWindowLong(hWnd, GWL.STYLE);
            return ((dwStyle & WS.CAPTION) != 0);
        }

        protected virtual void ToggleFullscreen(IntPtr hWnd)
        {
            bool isWindowed = IsWindowed(hWnd);
            if (isWindowed)
            {
                WINDOWPLACEMENT wpPrev;
                var placement = GetWindowPlacement(hWnd, out wpPrev);
                if (placement == BOOL.TRUE)
                {
                    if (_windoStylePlacement == null) _windoStylePlacement = new WindowStylePlacement();

                    _windoStylePlacement.WindowPlacement = wpPrev;
                    var styles = _windoStylePlacement.FullscreenStyles;
                    var exStyles = _windoStylePlacement.FullscreenExStyles;
                    SetWindowFullscreen(hWnd, (int)styles, (int)exStyles);
                    _options.WindowState = WindowState.Fullscreen;

                    ShowWindow(hWnd, SW.SHOWMAXIMIZED);
                    UpdateWindow(hWnd);
                }
            }
            else
            {
                if (_windoStylePlacement != null)
                {
                    SetWindowLong(hWnd, GWL.STYLE, (IntPtr)_windoStylePlacement.Styles);
                    SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)_windoStylePlacement.ExStyles);
                    var placement = _windoStylePlacement.WindowPlacement;
                    SetWindowPlacement(hWnd, ref placement);
                    SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.NOOWNERZORDER | SWP.FRAMECHANGED);
                    _options.WindowState = WindowState.Normal;
                    ShowWindow(hWnd, SW.SHOWNORMAL);
                    UpdateWindow(hWnd);
                }
                else
                {
                    _windoStylePlacement = new WindowStylePlacement(_options);
                    WS styles = _windoStylePlacement.Styles;
                    WS_EX exStyles = _windoStylePlacement.ExStyles;
                    SetWindowLong(hWnd, GWL.STYLE, (IntPtr)styles);
                    SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)exStyles);
                    _options.WindowState = WindowState.Maximize;
                    ShowWindow(hWnd, SW.SHOWMAXIMIZED);
                    UpdateWindow(hWnd);
                }
            }
        }
    }
}
