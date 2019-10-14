using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Infrastructure;

namespace Chromely.Native
{
    public class MacNativeGui : INativeGui
    {
        public event EventHandler<CreatedEventArgs> Created;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<CloseEventArgs> Close;

        private delegate void InitCallbackEvent(IntPtr app, IntPtr pool);
        private delegate void CreateCallbackEvent(IntPtr window, IntPtr view);
        private delegate void MovingCallbackEvent();
        private delegate void ResizeCallbackEvent(int width, int height);
        private delegate void QuitCallbackEvent();

        private ChromelyConfiguration _hostConfig;
        private IntPtr _appHandle;
        private IntPtr _poolHandle;
        private IntPtr _windowHandle;
        private IntPtr _viewHandle;
        private bool _isInitialized;

        public MacNativeGui()
        {
            _appHandle = IntPtr.Zero;
            _poolHandle = IntPtr.Zero;
            _windowHandle = IntPtr.Zero;
            _viewHandle = IntPtr.Zero;
            _isInitialized = false;
        }

        public void CreateWindow(ChromelyConfiguration hostConfig)
        {
            _hostConfig = hostConfig;
            NativeMethods.ChromelyParam configParam = InitParam(InitCallback,
                                                         CreateCallback,
                                                         MovingCallback,
                                                         ResizeCallback,
                                                         QuitCallback);


            var placement = _hostConfig.HostPlacement;
            configParam.centerscreen = placement.CenterScreen ? 1 : 0;
            configParam.frameless = placement.Frameless ? 1 : 0;
            configParam.fullscreen = placement.State == Core.Host.WindowState.Fullscreen ? 1 : 0;
            configParam.noresize = placement.NoResize ? 1 : 0;
            configParam.nominbutton = placement.NoMinMaxBoxes ? 1 : 0;
            configParam.nomaxbutton = placement.NoMinMaxBoxes ? 1 : 0;

            configParam.title = _hostConfig.HostTitle;

            configParam.x = placement.Left;
            configParam.y = placement.Top;
            configParam.width = placement.Width;
            configParam.height = placement.Height;

            NativeMethods.createwindow(ref configParam);
        }

        #region CreateWindow

        private static NativeMethods.ChromelyParam InitParam(InitCallbackEvent initCallback,
                                                    CreateCallbackEvent createCallback,
                                                    MovingCallbackEvent movingCallback,
                                                    ResizeCallbackEvent resizeCallback,
                                                    QuitCallbackEvent quitCallback)
        {

            NativeMethods.ChromelyParam configParam = new NativeMethods.ChromelyParam();
            configParam.initCallback = Marshal.GetFunctionPointerForDelegate(initCallback);
            configParam.createCallback = Marshal.GetFunctionPointerForDelegate(createCallback);
            configParam.movingCallback = Marshal.GetFunctionPointerForDelegate(movingCallback);
            configParam.resizeCallback = Marshal.GetFunctionPointerForDelegate(resizeCallback);
            configParam.exitCallback = Marshal.GetFunctionPointerForDelegate(quitCallback);

            return configParam;
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
                var size = GetWindowSize();
                SizeChanged?.Invoke(this, new SizeChangedEventArgs(size.Width, size.Height));
            }
        }

        private void QuitCallback()
        {
            Console.WriteLine("Quitting");
            Xilium.CefGlue.CefRuntime.QuitMessageLoop();
            Close?.Invoke(this, new CloseEventArgs());
        }

        #endregion

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
            try
            {
                NativeMethods.quit(_appHandle, _poolHandle);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::Quit");
                Log.Error(exception);
            }
        }

        public void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
        {
        }

        public void MessageBox(string message, MessageType messageType = MessageType.Error)
        {
        }

        /// <summary>
        /// The win.
        /// </summary>
        private static class NativeMethods
        {
            internal const string ChromelyMacLib = "libchromely.dylib";

            [StructLayout(LayoutKind.Sequential)]
            internal struct ChromelyParam
            {
                public int x;
                public int y;
                public int width;
                public int height;
                public int centerscreen;
                public int frameless;
                public int fullscreen;
                public int noresize;
                public int nominbutton;
                public int nomaxbutton;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string title;
                public IntPtr initCallback;
                public IntPtr createCallback;
                public IntPtr movingCallback;
                public IntPtr resizeCallback;
                public IntPtr exitCallback;
            }

            [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void createwindow(ref ChromelyParam chromelyParam);

            [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void run(IntPtr application);

            [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void quit(IntPtr application, IntPtr pool);
        }
    }
}