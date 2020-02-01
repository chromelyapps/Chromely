using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native.WinAPI
{
    public class WindowMessageInterceptor : IChromelyFramelessController
    {
        private readonly IChromelyConfiguration _config;
    
        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowProc callback, IntPtr lParam);

        private readonly ForwardMesssageHandler[] _hitTestReplacers;

        public WindowMessageInterceptor(IChromelyConfiguration config, IntPtr handle, IFramelessWindowService windowService)
        {
            _config = config;
            var framelessOption = _config?.WindowOptions?.FramelessOption ?? new FramelessOption();

            var childHandles = GetAllChildHandles(handle);
            _hitTestReplacers = childHandles
                .Concat(new[] { handle })
                .Select(h => new ForwardMesssageHandler(h, windowService.Handle, framelessOption, h == handle))
                .ToArray();
        }

        public List<IntPtr> GetAllChildHandles(IntPtr handle)
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

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
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

        private class ForwardMesssageHandler
        {
            private readonly IntPtr _handle;
            private readonly IntPtr _mainHandle;
            private readonly bool _isHost;
            private readonly IntPtr _originalWndProc;
            private readonly WndProc _wndProc;
            private readonly FramelessOption _framelessOption;

            public ForwardMesssageHandler(IntPtr handle, IntPtr mainHandle, FramelessOption framelessOption, bool isHost)
            {
                _handle = handle;
                _mainHandle = mainHandle;
                _framelessOption = framelessOption ?? new FramelessOption();
                _isHost = isHost;
                _originalWndProc = GetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC);
                _wndProc = WndProc;
                var wndProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                SetWindowLongPtr(_handle, (int)WindowLongFlags.GWL_WNDPROC, wndProcPtr);
            }

            private IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
            {
                var isForwardedArea = IsForwardedArea();
                if (isForwardedArea)
                {
                    SendMessage(_mainHandle, (int)message, wParam, lParam);
                }

                var msg = (WM)message;
                switch (msg)
                {
                    case WM.NCHITTEST:
                        {
                            if (isForwardedArea && _isHost)
                            {
                                return (IntPtr)HitTestValues.HTNOWHERE;
                            }
                            break;
                        }
                    default:
                        {
                            if (isForwardedArea)
                            {
                                return IntPtr.Zero;
                            }
                            break;
                        }
                }

                return CallWindowProc(_originalWndProc, hWnd, message, wParam, lParam); 
            }

            // TODO: Enchance to configurable region.
            private bool IsForwardedArea()
            {
                GetCursorPos(out var point);
                ScreenToClient(_mainHandle, ref point);
                GetClientRect(_mainHandle, out var mainClientRect);
                var right = mainClientRect.Width - point.X;

                return point.Y <= _framelessOption.DraggableHeight && right > _framelessOption.NonDraggableRightOffsetWidth;
            }
        }
    }
}