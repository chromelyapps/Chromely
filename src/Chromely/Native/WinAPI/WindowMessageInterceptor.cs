using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native
{
    public class WindowMessageInterceptor : IChromelyFramelessController
    {
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private readonly IChromelyConfiguration _config;
        private readonly DragWindowHandler[] _dragWindowHandlers;

        public WindowMessageInterceptor(IChromelyConfiguration config, IntPtr browserHandle, IChromelyNativeHost nativeHost)
        {
            _config = config;
            var framelessOption = _config?.WindowOptions?.FramelessOption ?? new FramelessOption();

            var childHandles = GetAllChildHandles(browserHandle);
            _dragWindowHandlers = childHandles
                .Concat(new[] { browserHandle })
                .Select(h => new DragWindowHandler(h, nativeHost, framelessOption, h == browserHandle))
                .ToArray();
        }

        protected virtual List<IntPtr> GetAllChildHandles(IntPtr handle)
        {
            var childHandles = new List<IntPtr>();
            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                var childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(handle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        protected virtual bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);
            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        private class DragWindowHandler
        {
            private readonly IntPtr _handle;
            private readonly IChromelyNativeHost _nativeHost;
            private readonly bool _isHost;
            private readonly IntPtr _originalWndProc;
            private readonly WndProc _wndProc;
            private readonly FramelessOption _framelessOption;
            private bool _hasCapture;
            private POINT _dragPoint;

            public DragWindowHandler(IntPtr handle, IChromelyNativeHost nativeHost, FramelessOption framelessOption, bool isHost)
            {
                _handle = handle;
                _nativeHost = nativeHost;
                _framelessOption = framelessOption ?? new FramelessOption();
                _isHost = isHost;
                _originalWndProc = GetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC);
                _wndProc = WndProc;
                var wndProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                SetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC, wndProcPtr);
            }

            private IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
            {
                var msg = (WM)message;
                var isDraggableArea = IsDraggableArea(msg, lParam);

                switch (msg)
                {
                    case WM.LBUTTONDOWN:
                    {
                        if (!isDraggableArea)
                        {
                            break;
                        }

                        var maximized = IsWindowMaximized(_nativeHost.Handle);
                        if (maximized)
                        {
                            break;
                        }

                        GetCursorPos(out var point);
                        ScreenToClient(_nativeHost.Handle, ref point);
                        _dragPoint = point;

                        // TODO: For some reason it only works when called twice in a row. Try to resolve it later.
                        SetCapture(hWnd);
                        SetCapture(hWnd);
                        return IntPtr.Zero;
                    }
                case WM.CAPTURECHANGED:
                    {
                        _hasCapture = lParam == hWnd;
                        return IntPtr.Zero;
                    }
                case WM.MOUSEMOVE:
                    {
                        if (_hasCapture)
                        {
                            var currentPoint = new Point((int)lParam);
                            var current = new Point(currentPoint.X, currentPoint.Y);
                            ClientToScreen(_nativeHost.Handle, ref current);

                            var position = new Point(current.X - _dragPoint.X, current.Y - _dragPoint.Y);
                            SetWindowPos(_nativeHost.Handle, IntPtr.Zero, position.X, position.Y, 0, 0,
                                SetWindowPosFlags.DoNotActivate
                                | SetWindowPosFlags.IgnoreZOrder
                                | SetWindowPosFlags.DoNotChangeOwnerZOrder
                                | SetWindowPosFlags.IgnoreResize);
                            return IntPtr.Zero;
                        }
                        break;
                    }
                case WM.LBUTTONUP:
                    {
                        if (_hasCapture)
                        {
                            ReleaseCapture();
                        }
                        break;
                    }
                }

                return CallWindowProc(_originalWndProc, hWnd, message, wParam, lParam);
            }

            private bool IsDraggableArea(WM message, IntPtr lParam)
            {
                if (message != WM.LBUTTONDOWN)
                {
                    return false;
                }

                var point = new Point((int)lParam);
                AdjustPointDpi(_nativeHost.Handle, ref point);
                return _framelessOption.IsDraggable(_nativeHost, point);
            }

            private bool IsWindowMaximized(IntPtr hWnd)
            {
                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                placement.Length = Marshal.SizeOf(placement);
                GetWindowPlacement(hWnd, ref placement);
                return placement.ShowCmd == ShowWindowCommands.Maximized;
            }

            /// <summary>
            /// Gets the correct point for the scaled window.
            /// </summary>
            private void AdjustPointDpi(IntPtr hWnd, ref Point point)
            {
                const int StandardDpi = 96;
                float scale = 1;
                var hdc = GetDC(hWnd);
                try
                {
                    var dpi = GetDeviceCaps(hdc, (int)DeviceCap.LOGPIXELSY);
                    scale = (float)dpi / StandardDpi;
                }
                finally
                {
                    ReleaseDC(hWnd, hdc);
                }

                point.X = (int)(point.X / scale);
                point.Y = (int)(point.Y / scale);
            }
        }
    }
}