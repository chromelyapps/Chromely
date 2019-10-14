using System;
using System.Drawing;
using Chromely.BrowserWindow;
using Chromely.Core;

namespace Chromely.Native
{
    public class WinNativeGui : INativeGui
    {
        public event EventHandler<CreatedEventArgs> Created;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<CloseEventArgs> Close;

        private ChromelyConfiguration _hostConfig;
        private IntPtr _handle;
        private IntPtr _gdkHandle;
        private IntPtr _xid;
        private bool _isInitialized;

        public WinNativeGui()
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
            _gdkHandle = IntPtr.Zero;
            _xid = IntPtr.Zero;
        }

        public void CreateWindow(ChromelyConfiguration hostConfig)
        {
        }

        public IntPtr CreateNewWindow(GtkWindowType type)
        {
            return IntPtr.Zero;
        }

        public IntPtr GetGdkHandle()
        {
            return IntPtr.Zero;
        }

        public IntPtr GetNativeHandle()
        {
            return IntPtr.Zero;
        }

        public void Init(int argc, string[] argv)
        {
        }

        public void ShowWindow()
        {
        }
        public void Run()
        {
        }

        public Size GetWindowSize()
        {
            return new Size();
        }

        public void SetWindowTitle(string title)
        {
        }

        public void SetWindowDefaultSize(int width, int height)
        {
        }

        public void ResizeWindow(IntPtr window, int width, int height)
        {
        }

        public void SetAppIcon(IntPtr window, string filename)
        {
        }

        public void SetWindowPosistion(GtkWindowPosition position)
        {
        }

        public void SetWindowMaximize()
        {
        }

        public void SetFullscreen()
        {
        }

        public void Quit()
        {
        }

        public void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
        {
        }

        public void MessageBox(string message, MessageType messageType = MessageType.Error)
        {
        }

        #region CreateWindow
        #endregion CreateWindow

        /// <summary>
        /// The win.
        /// </summary>
        private static class NativeMethods
        {
        }
    }
}