// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable

namespace Chromely.NativeHosts;

/// <inheritdoc/>
public class ChromelyWinHost : NativeHostBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChromelyWinHost"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="messageInterceptor">Instance of <see cref="IWindowMessageInterceptor"/>.</param>
    /// <param name="keyboadHandler">Instance of <see cref="IKeyboadHookHandler"/>.</param>
    public ChromelyWinHost(IChromelyConfiguration config, IWindowMessageInterceptor messageInterceptor, IKeyboadHookHandler keyboadHandler)
        : base(config, messageInterceptor, keyboadHandler)
    {
    }

    private bool _maximized;

    /// <inheritdoc/>
    protected override Win32WindowStyles GetWindowStyles(WindowState state)
    {
        Win32WindowStyles windowStyles = base.GetWindowStyles(state);
        if (!_windowFrameless)
        {
            return windowStyles;
        }

        windowStyles.Styles = WS.POPUPWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS
            | WS.SIZEBOX | WS.MINIMIZEBOX | WS.MAXIMIZEBOX;


        return windowStyles;
    }

    /// <inheritdoc/>
    protected override IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
    {
        WM msg = (WM)message;
        if (!_windowFrameless)
        {
            return base.WndProc(hWnd, message, wParam, lParam);
        }

        switch (msg)
        {
            case WM.CREATE:
                _handle = hWnd;
                break;
            case WM.NCPAINT:
                return IntPtr.Zero;
            case WM.NCACTIVATE:
                return DefWindowProcW(hWnd, msg, wParam, new IntPtr(-1));
            case WM.SIZE:
                {
                    _maximized = wParam == (IntPtr)WINDOW_SIZE.MAXIMIZED
                        || wParam == (IntPtr)WINDOW_SIZE.MAXSHOW;
                    ForceRedraw(hWnd);
                    break;
                }

            case WM.NCCALCSIZE:
                {
                    var captionHeight = GetSystemMetrics(SystemMetric.SM_CYCAPTION);
                    var menuHeight = GetSystemMetrics(SystemMetric.SM_CYMENU);
                    var padderBorder = GetSystemMetrics(SystemMetric.SM_CXPADDEDBORDER);
                    // NB: 'menuHeight' substraction probably may brake it if window will have a Title in a caption.
                    var topFrameHeight = captionHeight - menuHeight + padderBorder;

                    var result = DefWindowProcW(hWnd, msg, wParam, lParam);
                    var csp = (NcCalcSizeParams)Marshal.PtrToStructure(lParam, typeof(NcCalcSizeParams));
                    csp.Region.Input.TargetWindowRect.top -= _maximized ? 0 : topFrameHeight;
                    Marshal.StructureToPtr(csp, lParam, false);
                    return result;
                }
            case WM.NCHITTEST:
                {
                    var result = DefWindowProcW(hWnd, msg, wParam, lParam);
                    if (BorderHitTestResults.Contains((HT)result))
                    {
                        return result;
                    }

                    // TODO: Try to find out why this is not enough for drag window functionality.
                    return (IntPtr)HT.CAPTION;
                }
        }

        return base.WndProc(hWnd, message, wParam, lParam);
    }

    private static void ForceRedraw(IntPtr hWnd)
    {
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                        SWP.FRAMECHANGED
                       | SWP.NOSIZE
                       | SWP.NOMOVE
                       | SWP.NOREDRAW
                       | SWP.NOZORDER
                       | SWP.NOOWNERZORDER);

    }
}