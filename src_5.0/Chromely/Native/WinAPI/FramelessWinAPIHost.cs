using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Chromely.Core.Host;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native
{
    public class FramelessWinAPIHost : WinAPIHost
    {
        private static readonly HitTestValue[] BorderHitTestResults =
        {
            HitTestValue.HTTOP,
            HitTestValue.HTTOPLEFT,
            HitTestValue.HTTOPRIGHT,
            HitTestValue.HTBOTTOM,
            HitTestValue.HTBOTTOMLEFT,
            HitTestValue.HTBOTTOMRIGHT,
            HitTestValue.HTLEFT,
            HitTestValue.HTRIGHT,
            HitTestValue.HTBORDER
        };

        private bool _maximized;

        protected override Tuple<WindowStyles, WindowExStyles, ShowWindowCommand> GetWindowStyles(WindowState state)
        {
            var baseStyles = base.GetWindowStyles(state);
            var styles = WindowStyles.WS_POPUPWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS
                | WindowStyles.WS_SIZEBOX | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX;

            return new Tuple<WindowStyles, WindowExStyles, ShowWindowCommand>(
                styles, baseStyles.Item2, baseStyles.Item3);
        }

        protected override IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            WM msg = (WM)message;
            switch (msg)
            {
                case WM.CREATE:
                    _handle = hWnd;
                    break;
                case WM.NCPAINT:
                    return IntPtr.Zero;
                case WM.NCACTIVATE:
                    return DefWindowProc(hWnd, message, wParam, new IntPtr(-1));
                case WM.SIZE:
                    {
                        _maximized = wParam == (IntPtr)WindowSizeFlag.SIZE_MAXIMIZED
                            || wParam ==(IntPtr)WindowSizeFlag.SIZE_MAXSHOW;
                        ForceRedraw(hWnd);
                        break;
                    }
                case WM.NCCALCSIZE:
                    {
                        var captionHeight = GetSystemMetrics(SystemMetrics.CYCAPTION);
                        var menuHeight = GetSystemMetrics(SystemMetrics.CYMENU);
                        var padderBorder = GetSystemMetrics(SystemMetrics.CXPADDEDBORDER);
                        // NB: 'menuHeight' substraction probably may broke it if window will have a Title in a caption.
                        var topFrameHeight = captionHeight - menuHeight + padderBorder;

                        var result = DefWindowProc(hWnd, message, wParam, lParam);
                        var csp = (NcCalcSizeParams)Marshal.PtrToStructure(lParam, typeof(NcCalcSizeParams));
                        csp.Region.Input.TargetWindowRect.Top -= _maximized ? 0 : topFrameHeight;
                        Marshal.StructureToPtr(csp, lParam, false);
                        return result;
                    }
                case WM.NCHITTEST:
                    {
                        var result = DefWindowProc(hWnd, message, wParam, lParam);
                        if (BorderHitTestResults.Contains((HitTestValue)result))
                        {
                            return result;
                        }

                        // TODO: Try to find out why this is not enough for drag window functionality.
                        return (IntPtr)HitTestValue.HTCAPTION;
                    }
            }

            return base.WndProc(hWnd, message, wParam, lParam);
        }

        private static void ForceRedraw(IntPtr hWnd)
        {
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                            SetWindowPosFlags.FrameChanged
                           | SetWindowPosFlags.IgnoreResize
                           | SetWindowPosFlags.IgnoreMove
                           | SetWindowPosFlags.DoNotRedraw
                           | SetWindowPosFlags.IgnoreZOrder
                           | SetWindowPosFlags.DoNotChangeOwnerZOrder
                           | SetWindowPosFlags.IgnoreZOrder);

        }
    }
}