// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public abstract partial class NativeHostBase
{
    public virtual void ToggleFullscreen(IntPtr hWnd)
    {
        bool isWindowed = IsWindowed(hWnd);
        if (isWindowed)
        {
            WINDOWPLACEMENT wpPrev;
            var placement = GetWindowPlacement(hWnd, out wpPrev);
            if (placement == BOOL.TRUE)
            {
                switch (wpPrev.showCmd)
                {
                    case SW.RESTORE:
                    case SW.NORMAL:
                        _options.WindowState = WindowState.Normal;
                        break;
                    case SW.SHOWMAXIMIZED:
                        _options.WindowState = WindowState.Maximize;
                        break;
                }

                _windoStylePlacement.WindowPlacement = wpPrev;
                _windoStylePlacement.State = _options.WindowState;
            }

            var styles = _windoStylePlacement.FullscreenStyles;
            var exStyles = _windoStylePlacement.FullscreenExStyles;
            SetWindowLong(hWnd, GWL.STYLE, (IntPtr)styles);
            SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)exStyles);
            _options.KioskMode = false;
            _options.Fullscreen = true;
            _options.WindowState = WindowState.Fullscreen;
            ShowWindow(hWnd, SW.SHOWMAXIMIZED);
            UpdateWindow(hWnd);
        }
        else
        {
            var styles = _windoStylePlacement.Styles;
            var exStyles = _windoStylePlacement.ExStyles;
            SetWindowLong(hWnd, GWL.STYLE, (IntPtr)styles);
            SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)exStyles);
            _options.KioskMode = false;
            _options.Fullscreen = false;
            _options.WindowState = _windoStylePlacement.State == WindowState.Fullscreen || _windoStylePlacement.State == WindowState.Maximize ? WindowState.Maximize : WindowState.Normal;
            _windoStylePlacement.State = _options.WindowState;
            var placement = _windoStylePlacement.WindowPlacement;
            SetWindowPlacement(hWnd, ref placement);
            ShowWindow(hWnd, _options.WindowState == WindowState.Maximize ? SW.SHOWMAXIMIZED : SW.SHOWNORMAL);
            UpdateWindow(hWnd);
        }
    }

    // https://www.youtube.com/watch?v=0GQSOZe_D4I
    protected virtual void SetFullscreenScreen(IntPtr hWnd, int style, int styleEx)
    {
        Size fullscreenSize = new Size();
        var windowHDC = GetDC(hWnd);
        fullscreenSize.Width = Gdi32.GetDeviceCaps(windowHDC, Gdi32.DeviceCapability.HORZRES);
        fullscreenSize.Height = Gdi32.GetDeviceCaps(windowHDC, Gdi32.DeviceCapability.VERTRES);
        ReleaseDC(hWnd, windowHDC);

        SetWindowLong(hWnd, GWL.STYLE, (IntPtr)style);
        SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)styleEx);

        SetWindowPos(hWnd, HWND_TOP, 0, 0, fullscreenSize.Width, fullscreenSize.Height, SWP.NOZORDER | SWP.FRAMECHANGED);
    }

    protected virtual bool IsWindowed(IntPtr hWnd)
    {
        return _options.WindowState != WindowState.Fullscreen;
    }
}