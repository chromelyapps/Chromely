// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using static Chromely.Interop;
using static Chromely.Interop.User32;
using static Chromely.Interop.UxTheme;
using Chromely.NativeHost;

namespace Chromely.NativeHost
{
    //https://github.com/rossy/borderless-window
    public partial class DwmFramelessController
    {
        protected IChromelyNativeHost _nativeHost;
        protected DwmFramelessOption _framelessInfo;
        protected IWindowOptions _options;
        protected FramelessOption _framelessOption;
        protected DragWindowInfo _dragWindowInfo;
        protected DragMouseLLHook _dragMouseHook;
        protected Action<SizeChangedEventArgs> _onSizeChanged;

        private IntPtr _windowHandle;
        public DwmFramelessController(IChromelyNativeHost nativeHost, IWindowOptions options, DwmFramelessOption framelessInfo, Action<SizeChangedEventArgs> onSizeChanged)
        {
            _nativeHost = nativeHost;
            _options = options;
            _windowHandle = framelessInfo.Handle;
            _framelessInfo =  framelessInfo;
            _framelessOption = _options?.FramelessOption ?? new FramelessOption();
            _dragWindowInfo = new DragWindowInfo(_nativeHost, framelessInfo.Handle, _framelessOption);
            _onSizeChanged = onSizeChanged;
        }

        public void InitiateWindowDrag(IntPtr hWnd, IntPtr lParam)
        {
            if (_dragWindowInfo != null)
            {
                ReleaseCapture();
                _dragWindowInfo.Reset();

                var dragPoint = new POINT();
                dragPoint.x = PARAM.SignedLOWORD(lParam);
                dragPoint.y = PARAM.SignedHIWORD(lParam);

                var windowTopLeftPoint = new POINT();
                if (_dragWindowInfo.IsCursorInDraggableRegion(ref dragPoint, ref windowTopLeftPoint))
                {
                    SetCapture(hWnd);

                    _dragWindowInfo.DragInitiated = true;
                    _dragWindowInfo.DragPoint = dragPoint;
                    _dragWindowInfo.DragWindowLocation = windowTopLeftPoint;

                    InstallDragMouseHook();
                }
            }
        }

        public void ResetDragOperation()
        {
            if (_dragWindowInfo != null)
            {
                _dragWindowInfo.Reset();
            }
        }

        public unsafe void HandleWindowPosChanged(WINDOWPOS windPos)
        {
            RECT client = new RECT();
            GetClientRect(_framelessInfo.Handle, ref client);
            int oldWidth = _framelessInfo.Width;
            int oldHeight = _framelessInfo.Height;
            _framelessInfo.Width = client.right;
            _framelessInfo.Height = client.bottom;
            bool clientChanged = _framelessInfo.Width != oldWidth || _framelessInfo.Height != oldHeight;

            if (clientChanged || ((windPos.flags & SWP.FRAMECHANGED) != 0))
            {
                UpdateRegion();
            }

            if (clientChanged)
            {
                /* Invalidate the changed parts of the rectangle drawn in WM_PAINT */
                if (_framelessInfo.Width > oldWidth)
                {
                    RECT rect = new RECT(oldWidth - 1, 0, oldWidth, oldHeight);
                    InvalidateRect(_framelessInfo.Handle, &rect, BOOL.TRUE);
                }
                else
                {
                    RECT rect = new RECT(_framelessInfo.Width - 1, 0, _framelessInfo.Width, _framelessInfo.Height);
                    InvalidateRect(_framelessInfo.Handle, &rect, BOOL.TRUE);
                }
                if (_framelessInfo.Height > oldHeight)
                {
                    RECT rect = new RECT(0, oldHeight - 1, oldWidth, oldHeight);
                    InvalidateRect(_framelessInfo.Handle, &rect, BOOL.TRUE);
                }
                else
                {
                    RECT rect = new RECT(0, _framelessInfo.Height - 1, _framelessInfo.Width, _framelessInfo.Height);
                    InvalidateRect(_framelessInfo.Handle, &rect, BOOL.TRUE);
                }

                // OnSizeChanged
                _onSizeChanged?.Invoke(new SizeChangedEventArgs(_framelessInfo.Width, _framelessInfo.Height));
            }
        }

        public IntPtr HandleMessageInvisible(WM wmMsg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr oldStylePtr = GetWindowLong(_framelessInfo.Handle, GWL.STYLE);

            /* Prevent Windows from drawing the default title bar by temporarily
               toggling the WS_VISIBLE style. This is recommended in:
               https://blogs.msdn.microsoft.com/wpfsdk/2008/09/08/custom-window-chrome-in-wpf/ */

            var oldStyle = (WS)oldStylePtr;
            SetWindowLong(_framelessInfo.Handle, GWL.STYLE, (IntPtr)(oldStyle & ~WS.VISIBLE));
            IntPtr result = DefWindowProcW(_framelessInfo.Handle, wmMsg, wParam, lParam);
            SetWindowLong(_framelessInfo.Handle, GWL.STYLE, oldStylePtr);

            return result;
        }

        public IntPtr HandleNCHittest(int x, int y)
        {
            if (IsMaximized(_framelessInfo.Handle))
            {
                return (IntPtr)HT.CLIENT;
            }

            Point mouse = new Point(x, y);
            ScreenToClient(_framelessInfo.Handle, ref mouse);

            /* The horizontal frame should be the same size as the vertical frame,
               since the NONCLIENTMETRICS structure does not distinguish between them */
            int frameSize = GetSystemMetrics(SystemMetric.SM_CXFRAME) + GetSystemMetrics(SystemMetric.SM_CXPADDEDBORDER);

            /* The diagonal size handles are wider than the frame */
            int diagonal_width = frameSize * 2 + GetSystemMetrics(SystemMetric.SM_CXBORDER);

            if (mouse.Y < frameSize)
            {
                if (mouse.X < diagonal_width)
                {
                    return (IntPtr)HT.TOPLEFT;
                }
                if (mouse.X >= _framelessInfo.Width - diagonal_width)
                {
                    return (IntPtr)HT.TOPRIGHT;
                }
                return (IntPtr)HT.TOP;
            }

            if (mouse.Y >= _framelessInfo.Height - frameSize)
            {
                if (mouse.X < diagonal_width)
                {
                    return (IntPtr)HT.BOTTOMLEFT;
                }

                if (mouse.X >= _framelessInfo.Width - diagonal_width)
                {
                    return (IntPtr)HT.BOTTOMRIGHT;
                }

                return (IntPtr)HT.BOTTOM;
            }

            if (mouse.X < frameSize)
            {
                return (IntPtr)HT.LEFT;
            }

            if (mouse.X >= _framelessInfo.Width - frameSize)
            {
                return (IntPtr)HT.RIGHT;
            }

            return (IntPtr)HT.CLIENT;
        }

        public void HandleNCCalcsize(IntPtr wParam, IntPtr lParam)
        {
            // lParam is an [in, out] that can be either a RECT* (wParam == FALSE) or an NCCALCSIZE_PARAMS*.
            // Since the first field of NCCALCSIZE_PARAMS is a RECT and is the only field we care about
            // we can unconditionally treat it as a RECT.

            /* DefWindowProc must be called in both the maximized and non-maximized
               cases, otherwise tile/cascade windows won't work */
            var nonclient = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));

            var setResizeGrip = !_options.DisableResizing;
            if (setResizeGrip)
            {
                /* For the non-maximized case, set the output RECT to what it was
                   before WM_NCCALCSIZE modified it. This will make the client size the
                   same as the non-client size. */

                // Why is this not working?
                // nonclient.top += _framelessOption.DwmResizeGrip;

                nonclient.left += _framelessOption.DwmResizeGrip;
                nonclient.right -= _framelessOption.DwmResizeGrip;
                nonclient.bottom -= _framelessOption.DwmResizeGrip;
            }

            Marshal.StructureToPtr(nonclient, lParam, false);
        }

        public void HandleCompositionchanged()
        {
            bool enabled = false;
            DwmIsCompositionEnabled(out enabled);
            _framelessInfo.IsCompositionEnabled = enabled;

            if (enabled)
            {
                /* The window needs a frame to show a shadow, so give it the smallest
                   amount of frame possible */

                var dwmMargin = new MARGINS
                {
                    cxLeftWidth = 0,
                    cxRightWidth = 0,
                    cyTopHeight = 0,
                    cyBottomHeight = 1,
                };
                DwmExtendFrameIntoClientArea(_framelessInfo.Handle, ref dwmMargin);
                int dWMNCRP = (int)DWMNCRP.ENABLED;
                DwmSetWindowAttribute(_framelessInfo.Handle, DWMWA.NCRENDERING_POLICY, ref dWMNCRP, sizeof(DWMNCRP));
            }

            UpdateRegion();
        }

        public void HandleThemechanged()
        {
            _framelessInfo.IsThemeEnabled = IsThemeActive();
        }

        public void UpdateRegion()
        {
            RECT oldRgn = _framelessInfo.Region;

            if (IsMaximized(_framelessInfo.Handle))
            {
                WINDOWINFO wi = new WINDOWINFO(null);
                GetWindowInfo(_framelessInfo.Handle, ref wi);

                /* For maximized windows, a region is needed to cut off the non-client
		           borders that hang over the edge of the screen */
                _framelessInfo.Region = new RECT()
                {
                    left = wi.rcClient.left - wi.rcWindow.left,
                    top = wi.rcClient.top - wi.rcWindow.top,
                    right = wi.rcClient.right - wi.rcWindow.left,
                    bottom = wi.rcClient.bottom - wi.rcWindow.top
                };
            }
            else if (!_framelessInfo.IsCompositionEnabled)
            {
                /* For ordinary themed windows when composition is disabled, a region
		           is needed to remove the rounded top corners. Make it as large as
		           possible to avoid having to change it when the window is resized. */
                _framelessInfo.Region = new RECT()
                {
                    left = 0,
                    top = 0,
                    right = 32767,
                    bottom = 32767
                };
            }
            else
            {
                /* Don't mess with the region when composition is enabled and the
		           window is not maximized, otherwise it will lose its shadow */
                _framelessInfo.Region = new RECT();
            }

            /* Avoid unnecessarily updating the region to avoid unnecessary redraws */
            if (_framelessInfo.Region.AreEqual(oldRgn))
            {
                return;
            }

            /* Treat empty regions as NULL regions */
            if (_framelessInfo.Region.AreEqual(new RECT()))
            {
                SetWindowRgn(_framelessInfo.Handle, IntPtr.Zero, BOOL.TRUE);
            }
            else
            {
                RECT region = _framelessInfo.Region;
                SetWindowRgn(_framelessInfo.Handle, CreateRectRgnIndirect(ref region), BOOL.TRUE);
            }
        }

        public static bool IsMaximized(IntPtr hWnd)
        {
            WINDOWPLACEMENT wpPrev;
            BOOL placement = GetWindowPlacement(hWnd, out wpPrev);
            if (placement == BOOL.TRUE)
            {
                return wpPrev.showCmd == SW.MAXIMIZE;
            }

            return false;
        }

        #region Install/Detach Hooks

        private void InstallDragMouseHook()
        {
            try
            {
                UninstallDragMouseHook();

                if (!_options.KioskMode)
                {
                    _dragMouseHook = new DragMouseLLHook(_dragWindowInfo);
                    _dragMouseHook.Install();
                    DragMouseLLHook.MouseMoveHandler += MouseLLHook_MouseMoveHandler;
                    DragMouseLLHook.LeftButtonUpHandler += MouseLLHook_LeftButtonUpHandler;
                }
            }
            catch
            {
                UninstallDragMouseHook();
            }
        }

        private void UninstallDragMouseHook()
        {
            try
            {
                _dragMouseHook?.Uninstall();
            }
            catch
            {
            }
        }

        private void MouseLLHook_MouseMoveHandler(object sender, MouseMoveEventArgs eventArgs)
        {
            if (_windowHandle != IntPtr.Zero &&
                eventArgs != null)
            {
                SetWindowPos(_windowHandle, IntPtr.Zero, eventArgs.DeltaX, eventArgs.DeltaY, 0, 0,
                                    SWP.NOACTIVATE
                                    | SWP.NOZORDER
                                    | SWP.NOOWNERZORDER
                                    | SWP.NOSIZE);
            }
        }

        private void MouseLLHook_LeftButtonUpHandler(object sender, LeftButtonUpEventArgs eventArgs)
        {
            UninstallDragMouseHook();
            ReleaseCapture();
            _dragWindowInfo.Reset();
        }

        #endregion
    }
}
