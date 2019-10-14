using System;
using Chromely.Native;
using Chromely.Core;

namespace Chromely.BrowserWindow
{
    public class NativeWindow
    {
        private readonly ChromelyConfiguration _hostConfig;
        private readonly INativeGui _nativeGui;

        public NativeWindow(ChromelyConfiguration hostConfig)
        {
            _hostConfig = hostConfig;
            Handle = IntPtr.Zero;
            _nativeGui = NativeGuiFactory.GetNativeGui(hostConfig.Platform);

            _nativeGui.Created += OnCreated;
            _nativeGui.Moving += OnMoving;
            _nativeGui.SizeChanged += OnSizeChanged;
            _nativeGui.Close += OnClose;
        }

        public IntPtr Handle { get; private set; }
        public IntPtr WinXID { get; private set; }
  
        public void ShowWindow()
        {
            _nativeGui.CreateWindow(_hostConfig);
        }

        public void SetWindowPosition()
        {
        }

        public static void Run(ChromelyPlatform chromelyPlatform)
        {
            var nativeGui = NativeGuiFactory.GetNativeGui(chromelyPlatform);
            nativeGui.Run();
        }

        public static void Quit(ChromelyPlatform chromelyPlatform)
        {
            var nativeGui = NativeGuiFactory.GetNativeGui(chromelyPlatform);
            nativeGui.Quit();
        }

        public static void Resize(ChromelyPlatform chromelyPlatform, IntPtr window, int width, int height)
        {
            var nativeGui = NativeGuiFactory.GetNativeGui(chromelyPlatform);
            nativeGui.ResizeWindow(window, width, height);
        }

        public static void MessageBox(ChromelyPlatform chromelyPlatform, string message, MessageType messageType = MessageType.Error)
        {
            var nativeGui = NativeGuiFactory.GetNativeGui(chromelyPlatform);
            nativeGui.MessageBox(message, messageType);
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
    }
}
