// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public abstract partial class NativeHostBase
{
    /// <summary>
    /// Togge fullscreen - either way Restore to Fullscreen; Fullscreen to Restore. 
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
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

                _windowStyles.WindowPlacement = wpPrev;
                _windowStyles.State = _options.WindowState;
            }

            var styles = _windowStyles.FullscreenStyles;
            var exStyles = _windowStyles.FullscreenExStyles;
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
            var styles = _windowStyles.Styles;
            var exStyles = _windowStyles.ExStyles;
            SetWindowLong(hWnd, GWL.STYLE, (IntPtr)styles);
            SetWindowLong(hWnd, GWL.EXSTYLE, (IntPtr)exStyles);
            _options.KioskMode = false;
            _options.Fullscreen = false;
            _options.WindowState = _windowStyles.State == WindowState.Fullscreen || _windowStyles.State == WindowState.Maximize ? WindowState.Maximize : WindowState.Normal;
            _windowStyles.State = _options.WindowState;
            var placement = _windowStyles.WindowPlacement;
            SetWindowPlacement(hWnd, ref placement);
            ShowWindow(hWnd, _options.WindowState == WindowState.Maximize ? SW.SHOWMAXIMIZED : SW.SHOWNORMAL);
            UpdateWindow(hWnd);
        }
    }


    /// <summary>
    /// Set the window to fullscreen.
    /// </summary>
    /// <remarks>
    /// https://www.youtube.com/watch?v=0GQSOZe_D4I
    /// </remarks>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="style">The Window Style.</param>
    /// <param name="styleEx">The Extended Window Style.</param>
    protected virtual void SetWindowToFullscreen(IntPtr hWnd, int style, int styleEx)
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

    /// <summary>
    /// Check if window is not full screen.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <returns>true if not full screen, otherwise false.</returns>
    protected virtual bool IsWindowed(IntPtr hWnd)
    {
        return _options.WindowState != WindowState.Fullscreen;
    }
}