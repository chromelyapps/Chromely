// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowState.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
