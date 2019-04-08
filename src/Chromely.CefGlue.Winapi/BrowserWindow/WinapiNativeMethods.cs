// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinApiNativeMethods.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------


// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using WinApi.User32;
// ReSharper disable UnusedMember.Global

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    /// <summary>
    /// The native methods.
    /// </summary>
    public static class WinapiNativeMethods
    {
        /// <summary>
        /// The dll name.
        /// </summary>
        internal const string DllName = "user32.dll";

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
                return null;
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
    }
}
