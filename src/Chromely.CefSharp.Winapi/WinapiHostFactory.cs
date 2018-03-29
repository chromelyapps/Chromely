// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinapiHostFactory.cs" company="Chromely">
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

namespace Chromely.CefSharp.Winapi
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using WinApi.User32;
    using WinApi.Windows;

    /// <summary>
    /// The winapi host factory.
    /// </summary>
    public class WinapiHostFactory : WindowFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinapiHostFactory"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="styles">
        /// The styles.
        /// </param>
        /// <param name="hInstance">
        /// The h instance.
        /// </param>
        /// <param name="hIcon">
        /// The h icon.
        /// </param>
        /// <param name="hCursor">
        /// The h cursor.
        /// </param>
        /// <param name="hBgBrush">
        /// The h bg brush.
        /// </param>
        /// <param name="wndProc">
        /// The wnd proc.
        /// </param>
        public WinapiHostFactory(string name, WindowClassStyles styles, IntPtr hInstance, IntPtr hIcon, IntPtr hCursor, IntPtr hBgBrush, WindowProc wndProc)
            : base(name, styles, hInstance, hIcon, hCursor, hBgBrush, wndProc)
        {
        }

        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="iconFullPath">
        /// The icon full path.
        /// </param>
        /// <returns>
        /// The <see cref="WindowFactory"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static WindowFactory Init(string iconFullPath = null)
        {
            IntPtr? hIcon = NativeMethods.LoadIconFromFile(iconFullPath);
            return Create(null, WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_HREDRAW, null, hIcon, null, null);
        }
    }
}
