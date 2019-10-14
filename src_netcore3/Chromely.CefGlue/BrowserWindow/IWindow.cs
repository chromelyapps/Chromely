// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.CefGlue.Browser;

namespace Chromely.CefGlue.BrowserWindow
{
    /// <summary>
    /// Interface to a platform specific window.
    /// </summary>
    public interface IWindow : IDisposable
    {
        /// <summary>
        /// Gets the window handle.
        /// </summary>
        IntPtr HostHandle { get; }

        /// <summary>
        /// Gets a reference to the browser.
        /// </summary>
        CefGlueBrowser Browser { get; }

        /// <summary>
        /// Centers the main window on screen.
        /// </summary>
        void CenterToScreen();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void Exit();
    }
}