// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Common;
using Chromely.Core;
using WinApi.User32;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    /// <summary>
    /// The native window.
    /// </summary>
    internal class NativeWindow : NativeWindowBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// Chromely configuration.
        /// </param>
        public NativeWindow(ChromelyConfiguration hostConfig) : base (hostConfig)
        {
        }

        /// <summary>
        /// The run message loop.
        /// </summary>
        public static void RunMessageLoop()
        {
            while (User32Methods.GetMessage(out Message msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (msg.Value == (uint)WM.CLOSE)
                {
                    DetachKeyboardHook();
                }

                if (ChromelyConfiguration.Instance.HostPlacement.KioskMode && msg.Value == (uint)WM.HOTKEY && msg.WParam == (IntPtr)1)
                {
                    User32Methods.PostMessage(NativeWindow.NativeInstance.Handle, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                if (ChromelyConfiguration.Instance.HostPlacement.Frameless || ChromelyConfiguration.Instance.HostPlacement.KioskMode)
                {
                    CefRuntime.DoMessageLoopWork();
                }

                User32Methods.TranslateMessage(ref msg);
                User32Methods.DispatchMessage(ref msg);
            }
        }
    }
}