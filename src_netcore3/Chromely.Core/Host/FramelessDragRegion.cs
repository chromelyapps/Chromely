// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FramelessDragRegion.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using System;
using System.Runtime.InteropServices;

namespace Chromely.Core.Host
{
    public class FramelessDragRegion
    {
        public FramelessDragRegion()
        {
            Height = 28;
            NoDragWidth = 140;
        }

        /// <summary>
        /// Gets or sets the height of the drag region.
        /// This is the usual title bar height.
        /// Normal height is 28 device units (pixels) common for windows.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the no drag width of the drag window (right-side buttons).
        /// This is the total width of the title bar buttons (Min, Max, Close, Help).
        /// Normal width is 140 device units (pixels) common for windows.
        /// </summary>
        public int NoDragWidth { get; set; }

        public void RecalculateForWinapi(IntPtr handle)
        {
            try
            {
                NativeMethods.TITLEBARINFOEX tbi = new NativeMethods.TITLEBARINFOEX();
                tbi.cbSize = Marshal.SizeOf(typeof(NativeMethods.TITLEBARINFOEX));

                // Send the WM_GETTITLEBARINFOEX message
                NativeMethods.SendMessage(handle, NativeMethods.WM_GETTITLEBARINFOEX, IntPtr.Zero, ref tbi);

                Height = tbi.rcTitleBar.Height;

                // Get buttons width
                int titlebarButtonsMinWidth = 9999;
                int titlebarButtonsMaxWidth = -9999;

                foreach (var item in tbi.rgrect)
                {
                    if (item.Left == 0 || item.Right == 0) continue;

                    if (item.Left < titlebarButtonsMinWidth) titlebarButtonsMinWidth = item.Left;
                    if (item.Right > titlebarButtonsMaxWidth) titlebarButtonsMaxWidth = item.Right;
                }

                NoDragWidth = titlebarButtonsMaxWidth - titlebarButtonsMinWidth;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

    static class NativeMethods
    {
        /// <summary>
        /// The dll name.
        /// </summary>
        internal const string User32DllName = "user32.dll";

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
        }

        internal const int WM_GETTITLEBARINFOEX = 0x033F;
        internal const int CCHILDREN_TITLEBAR = 5;

        [StructLayout(LayoutKind.Sequential)]
        internal struct TITLEBARINFOEX
        {
            public int cbSize;
            public RECT rcTitleBar;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHILDREN_TITLEBAR + 1)]
            public int[] rgstate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHILDREN_TITLEBAR + 1)]
            public RECT[] rgrect;
        }

        [DllImport(User32DllName, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(
                                          IntPtr hWnd,
                                          int uMsg,
                                          IntPtr wParam,
                                          ref TITLEBARINFOEX lParam);
    }
}
