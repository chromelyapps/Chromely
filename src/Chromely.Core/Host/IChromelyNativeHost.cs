using Chromely.Core;
using Chromely.Core.Configuration;
using System;
using System.Drawing;

namespace Chromely.Core.Host
{
    public interface IChromelyNativeHost
    {
        event EventHandler<CreatedEventArgs> Created;
        event EventHandler<MovingEventArgs> Moving;
        event EventHandler<SizeChangedEventArgs> SizeChanged;
        event EventHandler<CloseEventArgs> Close;
        IntPtr Handle { get; }
        void CreateWindow(IWindowOptions options, bool debugging);
        IntPtr GetNativeHandle();
        void Run();
        Size GetWindowClientSize();
        float GetWindowDpiScale();
        void ResizeBrowser(IntPtr browserWindow, int width, int height);
        void Exit();
        void MessageBox(string message, int type);

        /// <summary> Determines if the window is maximized. </summary>
        /// <returns> True if maximized. </returns>
        bool GetWindowIsMaximized();

        /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
        /// <param name="state"> The state to set. </param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        bool SetWindowState(WindowState state);
    }
}
