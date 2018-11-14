// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinapiConstructionParams.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi
{
    using WinApi.User32;
    using WinApi.Windows;

    /// <summary>
    /// The winapi construction params.
    /// </summary>
    public static class WindowState
    {
        /// <summary>
        /// Gets the normal.
        /// </summary>
        public static ConstructionParams Normal => new FrameWindowConstructionParams();

        /// <summary>
        /// Gets the maximized.
        /// </summary>
        public static ConstructionParams Maximized => new MaximizedParams();

        /// <summary>
        /// Gets the fullscreen.
        /// </summary>
        public static ConstructionParams Fullscreen => new FullscreenParams();

        /// <summary>
        /// The maximized params.
        /// </summary>
        private class MaximizedParams : ConstructionParams
        {
            /// <summary>
            /// The styles.
            /// </summary>
            public override WindowStyles Styles
                => WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS | WindowStyles.WS_MAXIMIZE;

            /// <summary>
            /// The ex styles.
            /// </summary>
            public override WindowExStyles ExStyles
                => WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
        }

        /// <summary>
        /// The fullscreen params.
        /// </summary>
        private class FullscreenParams : ConstructionParams
        {
            /// <summary>
            /// The styles.
            /// </summary>
            public override WindowStyles Styles
                => WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS | WindowStyles.WS_MAXIMIZE;

            /// <summary>
            /// The ex styles.
            /// </summary>
            public override WindowExStyles ExStyles
                => WindowExStyles.WS_EX_TOOLWINDOW;
        }
    }
}
