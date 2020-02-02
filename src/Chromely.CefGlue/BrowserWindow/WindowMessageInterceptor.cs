using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Chromely.CefGlue.BrowserWindow
{
    public class WindowMessageInterceptor : IChromelyFramelessController
    {
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        private readonly IChromelyConfiguration _config;
        private readonly ForwardMesssageHandler[] _hitTestReplacers;

        public WindowMessageInterceptor(IChromelyConfiguration config, IntPtr browserHandle, IntPtr windowHandle)
        {
            _config = config;
            var framelessOption = _config?.WindowOptions?.FramelessOption ?? new FramelessOption();

            var childHandles = GetAllChildHandles(browserHandle);
            _hitTestReplacers = childHandles
                .Concat(new[] { browserHandle })
                .Select(h => new ForwardMesssageHandler(h, windowHandle, framelessOption, h == browserHandle))
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
                                return (IntPtr)HitTestValue.HTNOWHERE;
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

        private const string User32DLL = "user32.dll";

        enum WM : uint
        {
            NULL = 0x0000,
            NCHITTEST = 0x0084
        }

        [Flags]
        enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }

        [Flags]
        enum HitTestValue : int
        {
            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0
        }


        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }
            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }
            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }
            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        [DllImport(User32DLL)]
        static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowProc callback, IntPtr lParam);

        [DllImport(User32DLL, EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport(User32DLL, EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return Environment.Is64BitProcess ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport(User32DLL, EntryPoint = "SetWindowLong")]
        static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport(User32DLL, EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newWndProc)
        {
            if (Environment.Is64BitProcess)
                return SetWindowLongPtr64(new HandleRef(null, hWnd), nIndex, newWndProc);
            else
                return new IntPtr(SetWindowLong32(new HandleRef(null, hWnd), nIndex, newWndProc.ToInt32()));
        }

        [DllImport(User32DLL)]
        static extern IntPtr CallWindowProc(WndProc lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL)]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(User32DLL)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(User32DLL, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL)]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport(User32DLL)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport(User32DLL)]
        static extern bool ReleaseCapture();
                   
        [DllImport(User32DLL)]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
  }
}