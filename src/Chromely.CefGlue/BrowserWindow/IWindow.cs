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
        /// Reference to the browser.
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