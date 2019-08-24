// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using WinApi.User32;

namespace Chromely.Common
{
    /// <summary>
    /// The native methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The dll name.
        /// </summary>
        internal const string DllName = "user32.dll";

        internal const int SC_DRAGMOVE = 61458;

        [DllImport(DllName)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(DllName, EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport(DllName, EntryPoint = "SetWindowLongPtr")]
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

        [DllImport(DllName, EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport(DllName, EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return Environment.Is64BitProcess ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);
        }

        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        /// <summary>
        /// The load image.
        /// </summary>
        /// <param name="hinst">
        /// The hinst.
        /// </param>
        /// <param name="lpszName">
        /// The lpsz name.
        /// </param>
        /// <param name="uType">
        /// The u type.
        /// </param>
        /// <param name="cxDesired">
        /// The cx desired.
        /// </param>
        /// <param name="cyDesired">
        /// The cy desired.
        /// </param>
        /// <param name="fuLoad">
        /// The fu load.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        /// <summary>
        /// The set window pos.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="hWndInsertAfter">
        /// The h wnd insert after.
        /// </param>
        /// <param name="X">
        /// The x.
        /// </param>
        /// <param name="Y">
        /// The y.
        /// </param>
        /// <param name="cx">
        /// The cx.
        /// </param>
        /// <param name="cy">
        /// The cy.
        /// </param>
        /// <param name="uFlags">
        /// The u flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]

        // ReSharper disable once InconsistentNaming
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            WindowPositionFlags uFlags);

        /// <summary>
        /// The get focus.
        /// </summary>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// The get window rect.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="lpRect">
        /// The lp rect.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        /// The adjust window rect ex.
        /// </summary>
        /// <param name="lpRect">
        /// The lp rect.
        /// </param>
        /// <param name="dwStyle">
        /// The dw style.
        /// </param>
        /// <param name="hasMenu">
        /// The has menu.
        /// </param>
        /// <param name="dwExStyle">
        /// The dw ex style.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern bool AdjustWindowRectEx(
            [In] [Out] ref RECT lpRect,
            WindowStyles dwStyle,
            bool hasMenu,
            WindowExStyles dwExStyle);

        /// <summary>
        /// The load icon from file.
        /// </summary>
        /// <param name="iconFullPath">
        /// The icon full path.
        /// </param>
        /// <returns>
        /// The image pointer.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:ClosingParenthesisMustBeOnLineOfLastParameter", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed. Suppression is OK here.")]
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

            return LoadImage(                                         // returns a HANDLE so we have to cast to HICON
                  IntPtr.Zero,                                        // hInstance must be NULL when loading from a file
                  iconFullPath,                                       // the icon file name
                  (uint)ResourceImageType.IMAGE_ICON,                 // specifies that the file is an icon
                  0,                                                  // width of the image (we'll specify default later on)
                  0,                                                  // height of the image
                  (uint)LoadResourceFlags.LR_LOADFROMFILE |           // we want to load a file (as opposed to a resource)
                  (uint)LoadResourceFlags.LR_DEFAULTSIZE |            // default metrics based on the type (IMAGE_ICON, 32x32)
                  (uint)LoadResourceFlags.LR_SHARED                   // let the system release the handle when it's no longer used
                                                                      // ReSharper disable once StyleCop.SA1009
            );
        }

        #region Delegates

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
                                                      LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
                                                   IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        public struct RECT
        {
            /// <summary>
            /// The left.
            /// // x position of upper-left corner
            /// </summary>
            public int Left;

            /// <summary>
            /// The top.
            /// // y position of upper-left corner
            /// </summary>
            public int Top;

            /// <summary>
            /// The right.
            /// // x position of lower-right corner
            /// </summary>
            public int Right;

            /// <summary>
            /// The bottom.
            /// y position of lower-right corner
            /// </summary>
            public int Bottom;
        }

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

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public RECT[] rgrc;
            public WINDOWPOS lppos;
        }
    }
       
    internal enum DeviceCapsParams
    {
        /// <summary>
        /// Device driver version
        /// </summary>
        DRIVERVERSION = 0,
        /// <summary>
        ///  Device classification   
        /// </summary>
        TECHNOLOGY = 2,
        /// <summary>
        /// Horizontal size in millimeters
        /// </summary>
        HORZSIZE = 4,
        /// <summary>
        /// Vertical size in millimeters
        /// </summary>
        VERTSIZE = 6,
        /// <summary>
        /// Horizontal width in pixels
        /// </summary>
        HORZRES = 8,
        /// <summary>
        /// Vertical height in pixels
        /// </summary>
        VERTRES = 10,
        /// <summary>
        /// Number of bits per pixel
        /// </summary>
        BITSPIXEL = 12,
        /// <summary>
        /// Number of planes
        /// </summary>
        PLANES = 14,
        /// <summary>
        /// Number of brushes the device has
        /// </summary>
        NUMBRUSHES = 16,
        /// <summary>
        /// Number of pens the device has
        /// </summary>
        NUMPENS = 18,
        /// <summary>
        /// Number of markers the device has
        /// </summary>
        NUMMARKERS = 20,
        /// <summary>
        /// Number of fonts the device has
        /// </summary>
        NUMFONTS = 22,
        /// <summary>
        /// Number of colors the device supports
        /// </summary>
        NUMCOLORS = 24,
        /// <summary>
        /// Size required for device descriptor
        /// </summary>
        PDEVICESIZE = 26,
        /// <summary>
        /// Curve capabilities
        /// </summary>
        CURVECAPS = 28,
        /// <summary>
        /// Line capabilities
        /// </summary>
        LINECAPS = 30,
        /// <summary>
        /// Polygonal capabilities
        /// </summary>
        POLYGONALCAPS = 32,
        /// <summary>
        /// Text capabilities
        /// </summary>
        TEXTCAPS = 34,
        /// <summary>
        /// Clipping capabilities
        /// </summary>
        CLIPCAPS = 36,
        /// <summary>
        /// Bitblt capabilities
        /// </summary>
        RASTERCAPS = 38,
        /// <summary>
        /// Length of the X leg
        /// </summary>
        ASPECTX = 40,
        /// <summary>
        /// Length of the Y leg
        /// </summary>
        ASPECTY = 42,
        /// <summary>
        /// Length of the hypotenuse
        /// </summary>
        ASPECTXY = 44,
        /// <summary>
        /// Logical pixels/inch in X
        /// </summary>

        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels/inch in Y
        /// </summary>
        LOGPIXELSY = 90,
        /// <summary>
        /// Number of entries in physical palette
        /// </summary>

        SIZEPALETTE = 104,
        /// <summary>
        /// Number of reserved entries in palette
        /// </summary>
        NUMRESERVED = 106,
        /// <summary>
        /// Actual color resolution
        /// </summary>
        COLORRES = 108,
    }
}
