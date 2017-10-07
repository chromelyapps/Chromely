/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.CefSharp.Winapi.Browser.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using WinApi.Windows;

    public static class BrowserExtensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
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
