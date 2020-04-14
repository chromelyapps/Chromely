using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
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

        private ChromelyParam _configParam;
        private IWindowOptions _options;
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
        public virtual IntPtr Handle => _windowHandle;

        public virtual void CreateWindow(IWindowOptions options, bool debugging)
        {
            _options = options;
            _configParam = InitParam(RunCallback,
                                                    ShutdownCallback,
                                                    InitCallback,
                                                    CreateCallback,
                                                    MovingCallback,
                                                    ResizeCallback,
                                                    QuitCallback);


            _configParam.centerscreen = _options.WindowState == WindowState.Normal && _options.StartCentered ? 1 : 0;
            _configParam.frameless = _options.WindowFrameless ? 1 : 0;
            _configParam.fullscreen = _options.WindowState == Core.Host.WindowState.Fullscreen ? 1 : 0;
            _configParam.noresize = _options.DisableResizing ? 1 : 0;
            _configParam.nominbutton = _options.DisableMinMaximizeControls ? 1 : 0;
            _configParam.nomaxbutton = _options.DisableMinMaximizeControls ? 1 : 0;

            _configParam.title = _options.Title;

            _configParam.x = _options.Position.X;
            _configParam.y = _options.Position.Y;
            _configParam.width = _options.Size.Width;
            _configParam.height = _options.Size.Height;

            createwindow(ref _configParam);
        }

        public virtual IntPtr GetNativeHandle()
        {
            return _viewHandle;
        }

        public virtual void Run()
        {
        }

        public virtual Size GetWindowClientSize()
        {
            return new Size();
        }

        public virtual float GetWindowDpiScale() {
            return 1.0f;
        }

        /// <summary> Gets the current window state Maximised / Normal / Minimised etc. </summary>
        /// <returns> The window state. </returns>
        public virtual WindowState GetWindowState() {
            // TODO required for frameless Maccocoa mode
            return WindowState.Normal;
        }

        /// <summary> Sets window state. Maximise / Minimize / Restore. </summary>
        /// <param name="state"> The state to set. </param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool SetWindowState(WindowState state) {
            // TODO required for frameless Maccocoa mode
            return false;
        }

        public virtual void ResizeBrowser(IntPtr browserWindow, int width, int height)
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void MessageBox(string message, int type)
        {
            throw new NotImplementedException();
        }

        #region CreateWindow

        protected virtual void RunCallback()
        {
            CefRuntime.RunMessageLoop();
        }

        protected virtual void ShutdownCallback()
        {
            CefRuntime.Shutdown();
        }

        protected virtual void InitCallback(IntPtr app, IntPtr pool)
        {
            _appHandle = app;
            _poolHandle = pool;
            Console.WriteLine($"{DateTime.Now.ToLocalTime().ToString()}:initCallback");
        }

        protected virtual void CreateCallback(IntPtr window, IntPtr view)
        {
            _windowHandle = window;
            _viewHandle = view;
            var createdEvent = new CreatedEventArgs(IntPtr.Zero, _windowHandle, _viewHandle);
            Created?.Invoke(this, createdEvent);
            _isInitialized = true;
        }

        protected virtual void MovingCallback()
        {
            if (_viewHandle != IntPtr.Zero && _isInitialized)
            {
                Moving?.Invoke(this, new MovingEventArgs());
            }
        }

        protected virtual void ResizeCallback(int width, int height)
        {
            if (_viewHandle != IntPtr.Zero && _isInitialized)
            {
                SizeChanged?.Invoke(this, new SizeChangedEventArgs(width, height));
            }
        }

        protected virtual void QuitCallback()
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

        #endregion

    }
}