// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using System;
using System.Drawing;

namespace Chromely.Core.Host
{
    public interface IChromelyNativeHost : IDisposable
    {
        event EventHandler<CreatedEventArgs> HostCreated;
        event EventHandler<MovingEventArgs> HostMoving;
        event EventHandler<SizeChangedEventArgs> HostSizeChanged;
        event EventHandler<CloseEventArgs> HostClose;
        IntPtr Handle { get; }
        void CreateWindow(IWindowOptions options, bool debugging);
        IntPtr GetNativeHandle();
        void Run();
        Size GetWindowClientSize();
        float GetWindowDpiScale();
        void SetupMessageInterceptor(IntPtr browserWindowHandle);
        void ResizeBrowser(IntPtr browserWindow, int width, int height);
        void Exit();
        void SetWindowTitle(string title);

        /// <summary> Gets the current window state Maximised / Normal / Minimised etc. </summary>
        /// <returns> The window state. </returns>
        WindowState GetWindowState();

        /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
        /// <param name="state"> The state to set. </param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        bool SetWindowState(WindowState state);

        void ToggleFullscreen(IntPtr hWnd);
    }
}
