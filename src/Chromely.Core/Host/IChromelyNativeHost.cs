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
        void SetWindowTitle(string title);

        /// <summary> Gets the current window state Maximised / Normal / Minimised etc. </summary>
        /// <returns> The window state. </returns>
        WindowState GetWindowState();

        /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
        /// <param name="state"> The state to set. </param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        bool SetWindowState(WindowState state);
    }
}
