using System;
using Chromely.Native;
using Chromely.Core;

namespace Chromely.BrowserWindow
{
    public abstract class NativeWindow
    {
        private readonly IChromelyConfiguration _config;
        private readonly INativeHost _nativeHost;

        public NativeWindow(IChromelyConfiguration config, INativeHost nativeHost)
        {
            _config = config;
            _nativeHost = nativeHost;
            if (_nativeHost == null)
            {
                _nativeHost = NativeHostFactory.GetNativeHost(_config);
            }

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
        public static void MessageBox(IChromelyConfiguration config, string message, MessageType messageType = MessageType.Error)
        {
            var nativeGui = NativeHostFactory.GetNativeHost(config);
            nativeGui.MessageBox(message, messageType);
        }

        #endregion Message Box
    }
}
