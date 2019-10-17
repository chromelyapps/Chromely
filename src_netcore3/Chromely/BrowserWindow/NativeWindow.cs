using System;
using Chromely.Native;
using Chromely.Core;

namespace Chromely.BrowserWindow
{
    public class NativeWindow
    {
        private readonly ChromelyConfiguration _config;
        private readonly INativeHost _nativeHost;

        public NativeWindow(ChromelyConfiguration config)
        {
            _nativeHost = NativeHostFactory.GetNativeHost(config);
            _config = config;
            Handle = IntPtr.Zero;

            _nativeHost.Created += OnCreated;
            _nativeHost.Moving += OnMoving;
            _nativeHost.SizeChanged += OnSizeChanged;
            _nativeHost.Close += OnClose;
        }

        public IntPtr Handle { get; private set; }
        public IntPtr WinXID { get; private set; }

        public virtual void ShowWindow()
        {
            _nativeHost?.CreateWindow(_config);
        }

        public virtual void SetWindowPosition()
        {
        }

        public virtual void Run()
        {
            _nativeHost?.Run();
        }

        public virtual void Quit()
        {
            _nativeHost?.Exit();
        }

        public virtual void Resize(IntPtr window, int width, int height)
        {
            _nativeHost?.ResizeWindow(window, width, height);
        }

        #region

        protected virtual void OnCreated(object sender, CreatedEventArgs createdEventArgs)
        {
        }

        protected virtual void OnMoving(object sender, MovingEventArgs movingEventArgs)
        {
        }

        protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
        }

        protected virtual void OnClose(object sender, CloseEventArgs closeChangedEventArgs)
        {
        }

        #endregion

        #region Message Box
        public static void MessageBox(ChromelyConfiguration config, string message, MessageType messageType = MessageType.Error)
        {
            var nativeGui = NativeHostFactory.GetNativeHost(config);
            nativeGui.MessageBox(message, messageType);
        }

        #endregion Message Box
    }
}
