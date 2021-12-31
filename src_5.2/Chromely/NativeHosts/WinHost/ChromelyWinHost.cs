// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public class ChromelyWinHost : NativeHostBase
    {
        public ChromelyWinHost(IWindowMessageInterceptor messageInterceptor, IKeyboadHookHandler keyboadHandler) 
            : base(messageInterceptor, keyboadHandler)
        {
        }

        private bool _maximized;

        protected override WindowStylePlacement GetWindowStylePlacement(WindowState state)
        {
            WindowStylePlacement windowStylePlacement = base.GetWindowStylePlacement(state);
            if (!_windowFrameless)
            {
                return windowStylePlacement;
            }

            windowStylePlacement.Styles = WS.POPUPWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS
                | WS.SIZEBOX | WS.MINIMIZEBOX | WS.MAXIMIZEBOX;


            return windowStylePlacement;
        }

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
                        // NB: 'menuHeight' substraction probably may broke it if window will have a Title in a caption.
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
}