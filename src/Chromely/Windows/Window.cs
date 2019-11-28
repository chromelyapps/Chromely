using System;
using Chromely.CefGlue.Browser;
using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.Windows
{
    public class Window : NativeWindow, IWindow
    {
        private readonly IChromelyContainer _container;
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyCommandTaskRunner _commandTaskRunner;
        private readonly CefMessageRouterBrowserSide _browserMessageRouter;

        private IntPtr _browserWindowHandle;

        public Window(IChromelyNativeHost nativeHost, IChromelyContainer container, IChromelyConfiguration config, IChromelyCommandTaskRunner commandTaskRunner, CefMessageRouterBrowserSide browserMessageRouter)
            : base(nativeHost, config)
        {
            _container = container;
            _config = config;
            _commandTaskRunner = commandTaskRunner;
            _browserMessageRouter = browserMessageRouter;
            Browser = new CefGlueBrowser(this, _container, config, _commandTaskRunner, _browserMessageRouter, new CefBrowserSettings());
            Browser.Created += OnBrowserCreated;

            // Set event handler
            Browser.SetEventHandlers(_container);

            ShowWindow();
        }

        public CefGlueBrowser Browser { get; private set; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr HostHandle => Handle;
         

        public void CenterToScreen()
        {
            //TODO: Implement
        }

        public void Exit()
        {
            Quit();
        }

        #region Dispose

        public void Dispose()
        {
            try
            {
                if (Browser != null)
                {
                    Browser.Dispose();
                    Browser = null;
                    _browserWindowHandle = IntPtr.Zero;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        #endregion Dispose

        protected override void OnCreated(object sender, CreatedEventArgs createdEventArgs)
        {
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(createdEventArgs.WinXID, new CefRectangle(0, 0, _config.WindowWidth, _config.WindowHeight));

            Browser.Create(windowInfo);
        }

        protected override void OnMoving(object sender, MovingEventArgs movingEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                Browser.CefBrowser.GetHost().NotifyMoveOrResizeStarted();
            }
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (_browserWindowHandle != IntPtr.Zero)
            {
                ResizeBrowser(_browserWindowHandle, sizeChangedEventArgs.Width, sizeChangedEventArgs.Height);
            }
        }

        protected override void OnClose(object sender, CloseEventArgs closeChangedEventArgs)
        {
            Dispose();
            chromely.App.Properties.Save(_config);
        }

        private void OnBrowserCreated(object sender, EventArgs e)
        {
            _browserWindowHandle = Browser.CefBrowser.GetHost().GetWindowHandle();
            if (_browserWindowHandle != IntPtr.Zero)
            {
                ResizeBrowser(_browserWindowHandle);
            }
        }
    }
}