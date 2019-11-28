using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Chromely.Native
{
    internal class WinNativeMethods
    {
        #region "user32.dll"

        private const string User32DLL = "user32.dll";

        public const uint IDI_APPLICATION = 32512;
        public const uint IDC_ARROW = 32512;
        public const int HT_CAPTION = 0x2;

        public const uint MB_OK = 0x00000000;
        public const uint MB_ICONEXCLAMATION = 0x00000030;
        public const uint MB_SETFOREGROUND = 0x00010000;

        public const int CW_USEDEFAULT = unchecked((int)0x80000000);

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [Flags]
        public enum WindowExStyles : uint
        {
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_LAYERED = 0x00080000,
            WS_EX_LAYOUTRTL = 0x00400000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_NOACTIVATE = 0x08000000,
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_WINDOWEDGE = 0x00000100
        }

        [Flags]
        public enum WindowStyles : uint
        {
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP = 0x80000000u,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME = 0x40000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000,
            WS_THICKFRAME = 0x00040000,
            WS_SIZEBOX = 0x00040000
        }

        [Flags]
        public enum WindowLongFlags : int
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
        public enum ImageStyles : uint
        {
            IMAGE_BITMAP = 0,
            IMAGE_ICON = 1,
            IMAGE_CURSOR = 2,
            LR_CREATEDIBSECTION = 0x2000,
            LR_DEFAULTCOLOR = 0x0000,
            LR_DEFAULTSIZE = 0x0040,
            LR_LOADFROMFILE = 0x0010,
            LR_LOADMAP3DCOLORS = 0x1000,
            LR_LOADTRANSPARENT = 0x0020,
            LR_MONOCHROME = 0x0001,
            LR_SHARED = 0x8000,
            LR_VGACOLOR = 0x0080,
            LR_COPYFROMRESOURCE = 0x4000
        }

        public enum ShowWindowCommand : uint
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9
        }

        public enum MessageBoxResult : uint
        {
            Ok = 1,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No,
            Close,
            Help,
            TryAgain,
            Continue,
            Timeout = 32000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
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
        public struct SIZE
        {
            public int cx;
            public int cy;

            public SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
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

        public enum WM : uint
        {
            NULL = 0x0000,
            CREATE = 0x0001,
            DESTROY = 0x0002,
            MOVE = 0x0003,
            SIZE = 0x0005,
            ACTIVATE = 0x0006,
            SETFOCUS = 0x0007,
            KILLFOCUS = 0x0008,
            ENABLE = 0x000A,
            SETREDRAW = 0x000B,
            SETTEXT = 0x000C,
            GETTEXT = 0x000D,
            GETTEXTLENGTH = 0x000E,
            PAINT = 0x000F,
            CLOSE = 0x0010,
            QUERYENDSESSION = 0x0011,
            QUERYOPEN = 0x0013,
            ENDSESSION = 0x0016,
            QUIT = 0x0012,
            ERASEBKGND = 0x0014,
            SYSCOLORCHANGE = 0x0015,
            SHOWWINDOW = 0x0018,
            WININICHANGE = 0x001A,
            SETTINGCHANGE = WININICHANGE,
            DEVMODECHANGE = 0x001B,
            ACTIVATEAPP = 0x001C,
            FONTCHANGE = 0x001D,
            TIMECHANGE = 0x001E,
            CANCELMODE = 0x001F,
            SETCURSOR = 0x0020,
            MOUSEACTIVATE = 0x0021,
            CHILDACTIVATE = 0x0022,
            QUEUESYNC = 0x0023,
            GETMINMAXINFO = 0x0024,
            PAINTICON = 0x0026,
            ICONERASEBKGND = 0x0027,
            NEXTDLGCTL = 0x0028,
            SPOOLERSTATUS = 0x002A,
            DRAWITEM = 0x002B,
            MEASUREITEM = 0x002C,
            DELETEITEM = 0x002D,
            VKEYTOITEM = 0x002E,
            CHARTOITEM = 0x002F,
            SETFONT = 0x0030,
            GETFONT = 0x0031,
            SETHOTKEY = 0x0032,
            GETHOTKEY = 0x0033,
            QUERYDRAGICON = 0x0037,
            COMPAREITEM = 0x0039,
            GETOBJECT = 0x003D,
            COMPACTING = 0x0041,
            WINDOWPOSCHANGING = 0x0046,
            WINDOWPOSCHANGED = 0x0047,
            COPYDATA = 0x004A,
            CANCELJOURNAL = 0x004B,
            NOTIFY = 0x004E,
            INPUTLANGCHANGEREQUEST = 0x0050,
            INPUTLANGCHANGE = 0x0051,
            TCARD = 0x0052,
            HELP = 0x0053,
            USERCHANGED = 0x0054,
            NOTIFYFORMAT = 0x0055,
            CONTEXTMENU = 0x007B,
            STYLECHANGING = 0x007C,
            STYLECHANGED = 0x007D,
            DISPLAYCHANGE = 0x007E,
            GETICON = 0x007F,
            SETICON = 0x0080,
            NCCREATE = 0x0081,
            NCDESTROY = 0x0082,
            NCCALCSIZE = 0x0083,
            NCHITTEST = 0x0084,
            NCPAINT = 0x0085,
            NCACTIVATE = 0x0086,
            GETDLGCODE = 0x0087,
            SYNCPAINT = 0x0088,
            NCMOUSEMOVE = 0x00A0,
            NCLBUTTONDOWN = 0x00A1,
            NCLBUTTONUP = 0x00A2,
            NCLBUTTONDBLCLK = 0x00A3,
            NCRBUTTONDOWN = 0x00A4,
            NCRBUTTONUP = 0x00A5,
            NCRBUTTONDBLCLK = 0x00A6,
            NCMBUTTONDOWN = 0x00A7,
            NCMBUTTONUP = 0x00A8,
            NCMBUTTONDBLCLK = 0x00A9,
            NCXBUTTONDOWN = 0x00AB,
            NCXBUTTONUP = 0x00AC,
            NCXBUTTONDBLCLK = 0x00AD,
            INPUT_DEVICE_CHANGE = 0x00FE,
            INPUT = 0x00FF,
            KEYFIRST = 0x0100,
            KEYDOWN = 0x0100,
            KEYUP = 0x0101,
            CHAR = 0x0102,
            DEADCHAR = 0x0103,
            SYSKEYDOWN = 0x0104,
            SYSKEYUP = 0x0105,
            SYSCHAR = 0x0106,
            SYSDEADCHAR = 0x0107,
            COMMAND = 0x0111,
            SYSCOMMAND = 0x0112,
            TIMER = 0x0113,
            MOUSEFIRST = 0x0200,
            MOUSEMOVE = 0x0200,
            LBUTTONDOWN = 0x0201,
            LBUTTONUP = 0x0202,
            LBUTTONDBLCLK = 0x0203,
            RBUTTONDOWN = 0x0204,
            RBUTTONUP = 0x0205,
            RBUTTONDBLCLK = 0x0206,
            MBUTTONDOWN = 0x0207,
            MBUTTONUP = 0x0208,
            MBUTTONDBLCLK = 0x0209,
            MOUSEWHEEL = 0x020A,
            XBUTTONDOWN = 0x020B,
            XBUTTONUP = 0x020C,
            XBUTTONDBLCLK = 0x020D,
            MOUSEHWHEEL = 0x020E,
            MOUSELAST = 0x020E,
            PARENTNOTIFY = 0x0210,
            ENTERMENULOOP = 0x0211,
            EXITMENULOOP = 0x0212,
            NEXTMENU = 0x0213,
            SIZING = 0x0214,
            CAPTURECHANGED = 0x0215,
            MOVING = 0x0216,
            POWERBROADCAST = 0x0218,
            DEVICECHANGE = 0x0219,
            ENTERSIZEMOVE = 0x0231,
            EXITSIZEMOVE = 0x0232,
            MOUSEHOVER = 0x02A1,
            MOUSELEAVE = 0x02A3,
            NCMOUSEHOVER = 0x02A0,
            NCMOUSELEAVE = 0x02A2,
            COPY = 0x0301,
            PASTE = 0x0302,
            CLEAR = 0x0303,
            UNDO = 0x0304,
            RENDERFORMAT = 0x0305,
            RENDERALLFORMATS = 0x0306,
            DESTROYCLIPBOARD = 0x0307,
            DRAWCLIPBOARD = 0x0308,
            PAINTCLIPBOARD = 0x0309,
            VSCROLLCLIPBOARD = 0x030A,
            SIZECLIPBOARD = 0x030B,
            ASKCBFORMATNAME = 0x030C,
            CHANGECBCHAIN = 0x030D,
            HSCROLLCLIPBOARD = 0x030E,
            QUERYNEWPALETTE = 0x030F,
            PALETTEISCHANGING = 0x0310,
            PALETTECHANGED = 0x0311,
            HOTKEY = 0x0312,
            PRINT = 0x0317,
            PRINTCLIENT = 0x0318,
            APPCOMMAND = 0x0319,
        }

        [Flags]
        public enum SysCommand : int
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,
            //#if(WINVER >= 0x0400) //Win95
            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F,
            //#endif /* WINVER >= 0x0400 */

            //#if(WINVER >= 0x0600) //Vista
            SCF_ISSECURE = 0x00000001,
            //#endif /* WINVER >= 0x0600 */

            /*
              * Obsolete names
              */
            SC_ICON = SC_MINIMIZE,
            SC_ZOOM = SC_MAXIMIZE,
        }

        [Flags]
        public enum ClassStyles : uint
        {
            ByteAlignClient = 0x1000,
            ByteAlignWindow = 0x2000,
            ClassDC = 0x40,
            DoubleClicks = 0x8,
            DropShadow = 0x20000,
            GlobalClass = 0x4000,
            HorizontalRedraw = 0x2,
            NoClose = 0x200,
            OwnDC = 0x20,
            ParentDC = 0x80,
            SaveBits = 0x800,
            VerticalRedraw = 0x1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            public uint cbSize;
            public ClassStyles style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            AsynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }

        [DllImport(User32DLL)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);

        [DllImport(User32DLL)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport(User32DLL)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport(User32DLL)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, WindowStyles dwStyle, bool bMenu, WindowExStyles dwExStyle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(User32DLL, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL)]
        public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax);

        [DllImport(User32DLL)]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport(User32DLL)]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport(User32DLL)]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport(User32DLL)]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport(User32DLL)]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport(User32DLL)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(User32DLL)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(User32DLL)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(User32DLL, EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport(User32DLL, EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newWndProc)
        {
            if (Environment.Is64BitProcess)
                return SetWindowLongPtr64(new HandleRef(null, hWnd), nIndex, newWndProc);
            else
                return new IntPtr(SetWindowLong32(new HandleRef(null, hWnd), nIndex, newWndProc.ToInt32()));
        }

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newWndProc)
        {
            if (Environment.Is64BitProcess)
                return SetWindowLongPtr64(new HandleRef(null, hWnd), nIndex, newWndProc);
            else
                return new IntPtr(SetWindowLong32(new HandleRef(null, hWnd), nIndex, newWndProc.ToInt32()));
        }

        [DllImport(User32DLL, EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport(User32DLL, EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return Environment.Is64BitProcess ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport(User32DLL)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport(User32DLL)]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport(User32DLL, SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport(User32DLL, CharSet = CharSet.Auto)]
        public static extern MessageBoxResult MessageBox(IntPtr hWnd, String text, String caption, int options);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
           WindowExStyles dwExStyle,
           string lpClassName,
           string lpWindowName,
           WindowStyles dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport(User32DLL, SetLastError = true)]
        public static extern short RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport(User32DLL)]
        public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        public static IntPtr? LoadIconFromFile(string iconFullPath)
        {
            if (string.IsNullOrEmpty(iconFullPath))
            {
                return null;
            }

            if (!File.Exists(iconFullPath))
            {
                // If local file
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                iconFullPath = Path.Combine(appDirectory, iconFullPath);
                if (!File.Exists(iconFullPath))
                {
                    return null;
                }
            }

            return LoadImage(                                   // returns a HANDLE so we have to cast to HICON
                  IntPtr.Zero,                                  // hInstance must be NULL when loading from a file
                  iconFullPath,                                 // the icon file name
                  (uint)ImageStyles.IMAGE_ICON,                 // specifies that the file is an icon
                  0,                                            // width of the image (we'll specify default later on)
                  0,                                            // height of the image
                  (uint)ImageStyles.LR_LOADFROMFILE |           // we want to load a file (as opposed to a resource)
                  (uint)ImageStyles.LR_DEFAULTSIZE |            // default metrics based on the type (IMAGE_ICON, 32x32)
                  (uint)ImageStyles.LR_SHARED                   // let the system release the handle when it's no longer used
            );
        }

        #region Monitor

        const int MONITOR_DEFAULTTONULL = 0;
        const int MONITOR_DEFAULTTOPRIMARY = 1;
        const int MONITOR_DEFAULTTONEAREST = 2;

        // size of a device name string
        private const int CCHDEVICENAME = 32;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MONITORINFOEX
        {
            public int cbSize;
            public RectStruct monitor;
            public RectStruct workArea;
            public uint flags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public void Init()
            {
                this.cbSize = 40 + 2 * CCHDEVICENAME;
                this.DeviceName = string.Empty;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RectStruct
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport(User32DLL, CharSet = CharSet.Auto)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [DllImport(User32DLL, SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(User32DLL)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        public static Size GetWindowSize(IntPtr hwnd)
        {
            var size = new Size();
            if (hwnd != IntPtr.Zero)
            {
                GetWindowRect(hwnd, out var rect);
                size.Width = rect.Width;
                size.Height = rect.Height;
            }

            return size;
        }

        public static Rectangle FullScreenBounds(Rectangle configuredBounds)
        {
            try
            {
                IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR_DEFAULTTONEAREST);

                MONITORINFOEX monInfo = new MONITORINFOEX();
                monInfo.cbSize = Marshal.SizeOf(monInfo);
                monInfo.Init();

                GetMonitorInfo(handle, ref monInfo);

                int left = monInfo.monitor.left;
                int top = monInfo.monitor.top;
                int width = monInfo.monitor.right - monInfo.monitor.left;
                int height = monInfo.monitor.bottom - monInfo.monitor.top;

                if (width <= 0 || height <= 0) return configuredBounds;

                return new Rectangle(left, top, width, height);
            }
            catch { }
            {
            }

            return configuredBounds;
        }

        public static void CenterWindowToScreen(IntPtr hwnd, bool useWorkArea = true)
        {
            try
            {
                IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR_DEFAULTTONEAREST);

                MONITORINFOEX monInfo = new MONITORINFOEX();
                monInfo.cbSize = Marshal.SizeOf(monInfo);
                monInfo.Init();

                GetMonitorInfo(handle, ref monInfo);
                var screenRect = useWorkArea ? monInfo.workArea : monInfo.monitor;
                var midX = (monInfo.monitor.right - monInfo.monitor.left) / 2;
                var midY = (monInfo.monitor.bottom - monInfo.monitor.top) / 2;
                var size = GetWindowSize(hwnd);
                var left = midX - (size.Width / 2);
                var top = midY - (size.Height / 2);

                SetWindowPos(
                    hwnd,
                    IntPtr.Zero,
                    left,
                    top,
                    -1,
                    -1,
                    SetWindowPosFlags.DoNotActivate | SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.IgnoreZOrder);
            }
            catch { }
            {
            }
        }

        #region Keyboard Hooks

        #region Delegates

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            // Either WINDOWS key was held down. These keys are labeled with the Windows logo.
            // Keyboard shortcuts that involve the WINDOWS key are reserved for use by the
            // operating system.
            Windows = 8
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport(User32DLL, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(User32DLL, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(User32DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(User32DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        #endregion


        #endregion

        #endregion

        #region "kernel32.dll"

        private const string Kernel32DLL = "kernel32.dll";

        [DllImport(Kernel32DLL)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32DLL)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        #region "gdi32.dll"
        private const string Gdi32DLL = "gdi32.dll";

        public enum StockObjects
        {
            WHITE_BRUSH = 0,
            LTGRAY_BRUSH = 1,
            GRAY_BRUSH = 2,
            DKGRAY_BRUSH = 3,
            BLACK_BRUSH = 4,
            NULL_BRUSH = 5,
            HOLLOW_BRUSH = NULL_BRUSH,
            WHITE_PEN = 6,
            BLACK_PEN = 7,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            SYSTEM_FONT = 13,
            DEVICE_DEFAULT_FONT = 14,
            DEFAULT_PALETTE = 15,
            SYSTEM_FIXED_FONT = 16,
            DEFAULT_GUI_FONT = 17,
            DC_BRUSH = 18,
            DC_PEN = 19,
        }

        public enum DeviceCap
        {
            DRIVERVERSION = 0,
            TECHNOLOGY = 2,
            HORZSIZE = 4,
            VERTSIZE = 6,
            HORZRES = 8,
            VERTRES = 10,
            BITSPIXEL = 12,
            PLANES = 14,
            NUMBRUSHES = 16,
            NUMPENS = 18,
            NUMMARKERS = 20,
            NUMFONTS = 22,
            NUMCOLORS = 24,
            PDEVICESIZE = 26,
            CURVECAPS = 28,
            LINECAPS = 30,
            POLYGONALCAPS = 32,
            TEXTCAPS = 34,
            CLIPCAPS = 36,
            RASTERCAPS = 38,
            ASPECTX = 40,
            ASPECTY = 42,
            ASPECTXY = 44,
            SHADEBLENDCAPS = 45,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90,
            SIZEPALETTE = 104,
            NUMRESERVED = 106,
            COLORRES = 108,
            PHYSICALWIDTH = 110,
            PHYSICALHEIGHT = 111,
            PHYSICALOFFSETX = 112,
            PHYSICALOFFSETY = 113,
            SCALINGFACTORX = 114,
            SCALINGFACTORY = 115,
            VREFRESH = 116,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
            BLTALIGNMENT = 119
        }

        [DllImport(Gdi32DLL)]
        public static extern IntPtr GetStockObject(StockObjects fnObject);

        [DllImport(Gdi32DLL)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        #endregion

        public static class HwndZOrder
        {
            public static IntPtr
            NoTopMost = new IntPtr(-2),
            TopMost = new IntPtr(-1),
            Top = new IntPtr(0),
            Bottom = new IntPtr(1);
        }
    }
}
