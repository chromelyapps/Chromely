// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public class DefaultWindowMessageInterceptor : IWindowMessageInterceptor
    {
        protected readonly IChromelyConfiguration _config;
        protected IChromelyNativeHost? _nativeHost;
        protected IntPtr _browserHandle;
        protected DragWindowHandler[]? _dragWindowHandlers;
        protected FramelessOption? _framelessOption;

        public DefaultWindowMessageInterceptor(IChromelyConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Asynchronously wait for the Chromium widget window to be created for the given ChromiumWebBrowser,
        /// and when created hook into its Windows message loop.
        /// </summary>
        /// <param name="nativeHost">The native window.</param>
        /// <param name="browserHandle">The browser window to intercept Windows messages for.</param>
        public virtual void Setup(IChromelyNativeHost nativeHost, IntPtr browserHandle)
        {
            _nativeHost = nativeHost;
            _framelessOption = _config.WindowOptions?.FramelessOption ?? new FramelessOption();

            _browserHandle = browserHandle;

            try
            {
                bool foundWidget = false;
                while (!foundWidget)
                {
                    var childHandles = GetAllChildHandles(_browserHandle);
                    if (childHandles is not null && childHandles.Any())
                    {
                        foundWidget = true;
                        _dragWindowHandlers = childHandles
                            .Concat(new[] { browserHandle })
                            .Select(h => new DragWindowHandler(h, _nativeHost, _framelessOption))
                            .ToArray();
                    }
                    else
                    {
                        // Chrome hasn't yet set up its message-loop window.
                        Thread.Sleep(10);
                    }
                }
            }
            catch { }
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
            if (gcChildhandlesList.Target is null)
            {
                return false;
            }

            List<IntPtr>? childHandles = gcChildhandlesList.Target as List<IntPtr>;
            if (childHandles is not null)
            {
                childHandles.Add(hWnd);
            }

            return true;
        }

        protected class DragWindowHandler
        {
            private readonly IntPtr _handle;
            private readonly IChromelyNativeHost _nativeHost;
            private readonly IntPtr _originalWndProc;
            private readonly WndProc _wndProc;
            private readonly IntPtr _wndProcPtr;
            private readonly FramelessOption _framelessOption;
            private bool _hasCapture;
            private POINT _dragPoint;

            public DragWindowHandler(IntPtr handle, IChromelyNativeHost nativeHost, FramelessOption framelessOption)
            {
                _handle = handle;
                _nativeHost = nativeHost;
                _framelessOption = framelessOption ?? new FramelessOption();
                _originalWndProc = GetWindowLongPtr(_handle, (int)GWL.WNDPROC);
                _wndProc = WndProc;
                _wndProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                SetWindowLongPtr(_handle, (int)GWL.WNDPROC, _wndProcPtr);
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

                            var state = _nativeHost.GetWindowState();
                            if (state == WindowState.Maximize)
                            {
                                break;
                            }

                            GetCursorPos(out var point);
                            ScreenToClient(_nativeHost.Handle, ref point);
                            _dragPoint = new POINT(point.X, point.Y);

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

                                var position = new Point(current.X - _dragPoint.x, current.Y - _dragPoint.y);
                                SetWindowPos(_nativeHost.Handle, IntPtr.Zero, position.X, position.Y, 0, 0,
                                    SWP.NOACTIVATE
                                    | SWP.NOZORDER
                                    | SWP.NOOWNERZORDER
                                    | SWP.NOSIZE);

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
                    case WM.LBUTTONDBLCLK:
                        {
                            if (!isDraggableArea)
                            {
                                break;
                            }
                            _framelessOption.DblClick(_nativeHost);
                            break;
                        }
                }
                return CallWindowProc(_originalWndProc, hWnd, message, wParam, lParam);
            }

            private bool IsDraggableArea(WM message, IntPtr lParam)
            {
                if (message != WM.LBUTTONDOWN && message != WM.LBUTTONDBLCLK)
                {
                    return false;
                }

                var point = new Point((int)lParam);
                return _framelessOption.IsDraggable(_nativeHost, point);
            }
        }
    }
}