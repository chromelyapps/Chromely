using Chromely.Core;
using System;
using System.Drawing;

namespace Chromely.Native
{
    public interface INativeHost
    {
        event EventHandler<CreatedEventArgs> Created;
        event EventHandler<MovingEventArgs> Moving;
        event EventHandler<SizeChangedEventArgs> SizeChanged;
        event EventHandler<CloseEventArgs> Close;

        void CreateWindow(IChromelyConfiguration config);
        IntPtr CreateNewWindow(int type);
        IntPtr GetNativeHandle();
        void Init(int argc, string[] argv);
        void ShowWindow();
        void Run();
        Size GetWindowSize();
        void SetWindowTitle(string title);
        void SetWindowDefaultSize(int width, int height);
        void ResizeWindow(IntPtr window, int width, int height);
        void SetAppIcon(IntPtr raw, string filename);
        void SetWindowPosistion(int position);
        void SetWindowMaximize();
        void SetFullscreen();
        void Exit();
        void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr));
        void MessageBox(string message, MessageType messageType = MessageType.Error);
    }
}
