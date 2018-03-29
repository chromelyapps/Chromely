// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace Chromely.CefGlue.Winapi
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using Chromely.Core.Host;
    using WinApi.User32;

    /// <summary>
    /// The native methods.
    /// </summary>
    public static class NativeMethods
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
            WinapiConstants uFlags);

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
            public int Left; // x position of upper-left corner

            public int Top; // y position of upper-left corner

            public int Right; // x position of lower-right corner

            public int Bottom; // y position of lower-right corner
        }
    }
}
