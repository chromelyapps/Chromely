using System;
using Chromely.Native;
using Chromely.Core;
using Chromely.Core.Host;

namespace Chromely.Windows
{
    public abstract class NativeWindow
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyNativeHost _nativeHost;

        public NativeWindow(IChromelyNativeHost nativeHost, IChromelyConfiguration config)
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

        public virtual void ResizeBrowser(IntPtr window)
        {
            if (_nativeHost != null)
            {
                var clientSize = _nativeHost.GetWindowClientSize();
                if (clientSize.Width > 0 && clientSize.Height > 0)
                {
                    _nativeHost.ResizeBrowser(window, clientSize.Width, clientSize.Height);
                }
            }
        }

        public virtual void ResizeBrowser(IntPtr window, int width, int height)
        {
            _nativeHost?.ResizeBrowser(window, width, height);
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
        public static void MessageBox(IChromelyConfiguration config, string message, int messageType)
        {
            var nativeGui = NativeHostFactory.GetNativeHost(config);
            nativeGui.MessageBox(message, messageType);
        }

        #endregion Message Box
    }
}
