// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinapiHostFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
