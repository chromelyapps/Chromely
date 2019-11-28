using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;
using static Chromely.Native.MacNativeMethods;

namespace Chromely.Native
{
    public class MacCocoaHost : IChromelyNativeHost
    {
        public event EventHandler<CreatedEventArgs> Created;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<CloseEventArgs> Close;

        private delegate void RunMessageLoopCallback();
        private delegate void CefShutdownCallback();
        private delegate void InitCallbackEvent(IntPtr app, IntPtr pool);
        private delegate void CreateCallbackEvent(IntPtr window, IntPtr view);
        private delegate void MovingCallbackEvent();
        private delegate void ResizeCallbackEvent(int width, int height);
        private delegate void QuitCallbackEvent();

        private IChromelyConfiguration _config;
        private IntPtr _appHandle;
        private IntPtr _poolHandle;
        private IntPtr _windowHandle;
        private IntPtr _viewHandle;
        private bool _isInitialized;

        public MacCocoaHost()
        {
            _appHandle = IntPtr.Zero;
            _poolHandle = IntPtr.Zero;
            _windowHandle = IntPtr.Zero;
            _viewHandle = IntPtr.Zero;
            _isInitialized = false;
        }

        public void CreateWindow(IChromelyConfiguration config)
        {
            _config = config;
            ChromelyParam configParam = InitParam(RunCallback,
                                                    ShutdownCallback,
                                                    InitCallback,
                                                    CreateCallback,
                                                    MovingCallback,
                                                    ResizeCallback,
                                                    QuitCallback);


            configParam.centerscreen = _config.WindowState == WindowState.Normal && _config.WindowCenterScreen ? 1 : 0;
            configParam.frameless = _config.WindowFrameless ? 1 : 0;
            configParam.fullscreen = _config.WindowState == Core.Host.WindowState.Fullscreen ? 1 : 0;
            configParam.noresize = _config.WindowNoResize ? 1 : 0;
            configParam.nominbutton = _config.WindowNoMinMaxBoxes ? 1 : 0;
            configParam.nomaxbutton = _config.WindowNoMinMaxBoxes ? 1 : 0;

            configParam.title = _config.WindowTitle;

            configParam.x = _config.WindowLeft;
            configParam.y = _config.WindowTop;
            configParam.width = _config.WindowWidth;
            configParam.height = _config.WindowHeight;

            createwindow(ref configParam);
        }

        #region CreateWindow

        private static ChromelyParam InitParam(RunMessageLoopCallback runCallback,
                                                    CefShutdownCallback cefShutdownCallback,
                                                    InitCallbackEvent initCallback,
                                                    CreateCallbackEvent createCallback,
                                                    MovingCallbackEvent movingCallback,
                                                    ResizeCallbackEvent resizeCallback,
                                                    QuitCallbackEvent quitCallback)
        {

            ChromelyParam configParam = new ChromelyParam();
            configParam.runMessageLoopCallback = Marshal.GetFunctionPointerForDelegate(runCallback);
            configParam.cefShutdownCallback = Marshal.GetFunctionPointerForDelegate(cefShutdownCallback);
            configParam.initCallback = Marshal.GetFunctionPointerForDelegate(initCallback);
            configParam.createCallback = Marshal.GetFunctionPointerForDelegate(createCallback);
            configParam.movingCallback = Marshal.GetFunctionPointerForDelegate(movingCallback);
            configParam.resizeCallback = Marshal.GetFunctionPointerForDelegate(resizeCallback);
            configParam.exitCallback = Marshal.GetFunctionPointerForDelegate(quitCallback);

            return configParam;
        }

        private void RunCallback()
        {
            CefRuntime.RunMessageLoop();
        }

        private void ShutdownCallback()
        {
            CefRuntime.Shutdown();
        }

        private void InitCallback(IntPtr app, IntPtr pool)
        {
            _appHandle = app;
            _poolHandle = pool;
            Console.WriteLine($"{DateTime.Now.ToLocalTime().ToString()}:initCallback");
        }

        private void CreateCallback(IntPtr window, IntPtr view)
        {
            _windowHandle = window;
            _viewHandle = view;
            var createdEvent = new CreatedEventArgs(IntPtr.Zero, _windowHandle, _viewHandle);
            Created?.Invoke(this, createdEvent);
            _isInitialized = true;
        }

        private void MovingCallback()
        {
            if (_viewHandle != IntPtr.Zero && _isInitialized)
            {
                Moving?.Invoke(this, new MovingEventArgs());
            }
        }

        private void ResizeCallback(int width, int height)
        {
            if (_viewHandle != IntPtr.Zero && _isInitialized)
            {
                SizeChanged?.Invoke(this, new SizeChangedEventArgs(width, height));
            }
        }

        private void QuitCallback()
        {
            try
            {
                CefRuntime.Shutdown();
                Close?.Invoke(this, new CloseEventArgs());
                Environment.Exit(0);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in MacCocoaHost::QuitCallback");
                Logger.Instance.Log.Error(exception);
            }
        }

        #endregion

        public IntPtr GetNativeHandle()
        {
            return _viewHandle;
        }

        public void Run()
        {
        }

        public Size GetWindowClientSize()
        {
            return new Size();
        }

        public void ResizeBrowser(IntPtr browserWindow, int width, int height)
        {
        }

        public void Exit()
        {
        }

        public void MessageBox(string message, int type)
        {
            throw new NotImplementedException();
        }
    }
}