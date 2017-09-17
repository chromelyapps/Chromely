using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinApi.Windows;

namespace Chromely.CefSharpBrowser.Extensions
{
    public static class BrowserExtensions
    {
        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WM_SETICON = 0x0080;
        private const uint WS_SYSMENU = 0x80000;

        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeAsyncIfPossible(this EventedWindowCore window, Action action)
        {
            try
            {
                Task.Run(() => action);
            }
            catch (Exception)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void RemoveTitleBar(this EventedWindowCore window)
        {
            if (window.Handle != null)
            {
                SetWindowLong(window.Handle, GWL_STYLE, GetWindowLong(window.Handle, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));
            }
        }

        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void RemoveTitleBarIcon(this EventedWindowCore window)
        {
            if (window.Handle != null)
            {
                SetWindowLong(window.Handle, GWL_STYLE, GetWindowLong(window.Handle, GWL_STYLE) & (0xFFFFFFFF ^ WM_SETICON));
            }
        }
    }
}
