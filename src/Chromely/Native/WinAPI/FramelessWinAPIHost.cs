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
            // TODO: Resizing from the top border causes glitches.
            // HitTestValue.HTTOP,
            // HitTestValue.HTTOPLEFT,
            // HitTestValue.HTTOPRIGHT,
            HitTestValue.HTBOTTOM,
            HitTestValue.HTBOTTOMLEFT,
            HitTestValue.HTBOTTOMRIGHT,
            HitTestValue.HTLEFT,
            HitTestValue.HTRIGHT,
            HitTestValue.HTBORDER
        };

        private bool _maximized;
        private bool _dragging;
        private POINT _dragPoint;

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
            // TODO: Find out how to calculate the caption size.
            const int CaptionHeight = 7;

            WM msg = (WM)message;
            switch (msg)
            {
                case WM.LBUTTONDOWN:
                    {
                        GetCursorPos(out var point);
                        ScreenToClient(hWnd, ref point);

                        // TODO: For some reason it only works when called twice in a row. Try to resolve it later.
                        SetCapture(hWnd);
                        SetCapture(hWnd);
                        _dragPoint = point;
                        break;
                    }
                case WM.CAPTURECHANGED:
                    {
                        _dragging = lParam == hWnd;
                        break;
                    }
                case WM.MOUSEMOVE:
                    {
                        if (_dragging)
                        {
                            var currentPoint = new System.Drawing.Point((int)lParam);
                            var current = new Point(currentPoint.X, currentPoint.Y);
                            ClientToScreen(hWnd, ref current);

                            var position = new Point(current.X - _dragPoint.X, current.Y - _dragPoint.Y);
                            SetWindowPos(hWnd, IntPtr.Zero, position.X, position.Y, 0, 0,
                                SetWindowPosFlags.DoNotActivate
                                | SetWindowPosFlags.IgnoreZOrder
                                | SetWindowPosFlags.DoNotChangeOwnerZOrder
                                | SetWindowPosFlags.IgnoreResize);
                        }
                        break;
                    }
                case WM.LBUTTONUP:
                    {
                        ReleaseCapture();
                        break;
                    }
                case WM.CREATE:
                    _handle = hWnd;
                    break;
                case WM.NCPAINT:
                    return IntPtr.Zero;
                case WM.NCACTIVATE:
                    return DefWindowProc(hWnd, message, wParam, new IntPtr(-1));
                case WM.SIZE:
                    {
                        if (wParam == (IntPtr)WindowSizeFlag.SIZE_MAXIMIZED)
                        {
                            _maximized = true;
                            ForceRedraw(hWnd);
                        }
                        if (wParam == (IntPtr)WindowSizeFlag.SIZE_RESTORED)
                        {
                            _maximized = false;
                            ForceRedraw(hWnd);
                        }
                        break;
                    }
                case WM.NCCALCSIZE:
                    {
                        var result = DefWindowProc(hWnd, message, wParam, lParam);
                        var csp = (NcCalcSizeParams)Marshal.PtrToStructure(lParam, typeof(NcCalcSizeParams));
                        csp.Region.Input.TargetWindowRect.Top -= _maximized ? 0 : CaptionHeight;
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