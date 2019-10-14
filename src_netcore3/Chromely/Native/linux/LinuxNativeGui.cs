using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;

namespace Chromely.Native
{
    public class LinuxNativeGui : INativeGui
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

        public LinuxNativeGui()
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
            _gdkHandle = IntPtr.Zero;
            _xid = IntPtr.Zero;
        }

        public void CreateWindow(ChromelyConfiguration hostConfig)
        {
            _hostConfig = hostConfig;

            Init(0, null);

            var wndType = _hostConfig.HostPlacement.Frameless
                ? GtkWindowType.GtkWindowPopup
                : GtkWindowType.GtkWindowToplevel;

            _handle = CreateNewWindow(wndType);

            var placement = _hostConfig.HostPlacement;

            SetWindowTitle(_hostConfig.HostTitle);
            SetAppIcon(_handle, _hostConfig.HostIconFile);
            SetWindowDefaultSize(placement.Width, placement.Height);

            if (placement.CenterScreen)
            {
                SetWindowPosistion(GtkWindowPosition.GtkWinPosCenter);
            }

            switch (_hostConfig.HostPlacement.State)
            {
                case WindowState.Normal:
                    break;

                case WindowState.Maximize:
                    SetWindowMaximize();
                    break;

                case WindowState.Fullscreen:
                    SetFullscreen();
                    break;
            }

            ConnectRealizeSignal(OnRealized, FreeData);
            ConnectSizeAllocateSignal(OnSizeAllocate, FreeData);
            ConnectResizeSignal(OnResize, FreeData);
            ConnectDestroySignal(OnDestroy, FreeData);

            ShowWindow();
        }

        public IntPtr CreateNewWindow(GtkWindowType type)
        {
            try
            {
                _handle = NativeMethods.gtk_window_new(type);
                Utils.AssertNotNull("CreateNewWindow", _handle);
                return _handle;
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::CreateNewWindow");
                Log.Error(exception);
            }

            return IntPtr.Zero;
        }

        public IntPtr GetGdkHandle()
        {
            try
            {
                _gdkHandle = NativeMethods.gtk_widget_get_window(_handle);
                Utils.AssertNotNull("GetGdkHandle", _gdkHandle);
                return _gdkHandle;
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::GetGdkHandle");
                Log.Error(exception);
            }

            return IntPtr.Zero;
        }

        public IntPtr GetNativeHandle()
        {
            try
            {
                _gdkHandle = NativeMethods.gtk_widget_get_window(_handle);
                Utils.AssertNotNull("GetNativeHandle:gtk_widget_get_window", _gdkHandle);
                _xid = NativeMethods.gdk_x11_window_get_xid(_gdkHandle);
                Utils.AssertNotNull("GetNativeHandle:gdk_x11_window_get_xid", _xid);
                return _xid;
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::GetNativeHandle");
                Log.Error(exception);
            }

            return IntPtr.Zero;
        }

        public void Init(int argc, string[] argv)
        {
            try
            {
                NativeMethods.gtk_init(argc, argv);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::Init");
                Log.Error(exception);
            }
        }

        public void ShowWindow()
        {
            try
            {
                NativeMethods.gtk_widget_show_all(_handle);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::ShowWindow");
                Log.Error(exception);
            }
        }

        public void Run()
        {
            try
            {
                NativeMethods.gtk_main();
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::Run");
                Log.Error(exception);
            }
        }

        public Size GetWindowSize()
        {
            try
            {
                NativeMethods.gtk_window_get_size(_handle, out int width, out int height);
                return new Size(width, height);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::GetWindowSize");
                Log.Error(exception);
            }

            return new Size();
        }

        public void SetWindowTitle(string title)
        {
            try
            {
                NativeMethods.gtk_window_set_title(_handle, title);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::SetWindowTitle");
                Log.Error(exception);
            }
        }

        public void SetWindowDefaultSize(int width, int height)
        {
            try
            {
                NativeMethods.gtk_window_set_default_size(_handle, width, height);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::SetWindowDefaultSize");
                Log.Error(exception);
            }
        }

        public void ResizeWindow(IntPtr window, int width, int height)
        {
            /*
              RESIZE IS NOT SUPPORTED

              Resize is not available
              CefGlue implementation is not exposing: cef_get_xdisplay
              // See https://gitlab.com/xiliumhq/chromiumembedded/cefglue/blob/master/CefGlue/CefRuntime.cs

                Developers who want to look into this further can check with is done in CefClient
                Using libX11.so Should be something like:

                ::Window xwindow = browser->GetHost()->GetWindowHandle();
                ::Display* xdisplay = cef_get_xdisplay();
                XWindowChanges changes = {0};
                changes.x = x;
                changes.y = y;
                changes.width = static_cast<int>(width);
                changes.height = static_cast<int>(height);
                XConfigureWindow(xdisplay, xwindow,
                CWX | CWY | CWHeight | CWWidth, &changes);
             */
        }

        public void SetAppIcon(IntPtr window, string filename)
        {
            try
            {
                IntPtr ptrToAnsi = IconHandler.IconFileToPtr(filename);
                if (ptrToAnsi != IntPtr.Zero)
                {
                    IntPtr error = IntPtr.Zero;
                    NativeMethods.gtk_window_set_icon_from_file(window, ptrToAnsi, out error);
                    Marshal.FreeCoTaskMem(ptrToAnsi);
                    if (error != IntPtr.Zero)
                    {
                        Log.Error("Icon handle not successfully freed.");
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public void SetWindowPosistion(GtkWindowPosition position)
        {
            try
            {
                NativeMethods.gtk_window_set_position(_handle, position);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::SetWindowPosistion");
                Log.Error(exception);
            }
        }

        public void SetWindowMaximize()
        {
            try
            {
                NativeMethods.gtk_window_maximize(_handle);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::SetWindowMaximize");
                Log.Error(exception);
            }
        }

        public void SetFullscreen()
        {
            try
            {
                NativeMethods.gtk_window_fullscreen(_handle);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::SetFullscreen");
                Log.Error(exception);
            }
        }

        public void Quit()
        {
            try
            {
                NativeMethods.gtk_main_quit();
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::Quit");
                Log.Error(exception);
            }
        }

        public void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
        {
            try
            {
                NativeMethods.g_signal_connect_data(_handle, signalName, handler, data, destroyData, connectFlags);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::RegisterHandler");
                Log.Error(exception);
            }
        }

        public void MessageBox(string message, MessageType messageType = MessageType.Error)
        {
            try
            {
                var diag = NativeMethods.gtk_message_dialog_new(IntPtr.Zero, DialogFlags.Modal, messageType, ButtonsType.Ok, message, IntPtr.Zero);
                NativeMethods.gtk_dialog_run(diag);
                NativeMethods.gtk_widget_destroy(diag);
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxNativeMethods::MessageBox");
                Log.Error(exception);
            }
        }

        #region CreateWindow

        private delegate void SizeAllocateCallback(IntPtr window, IntPtr allocation, int baseline);
        private delegate bool ResizeCallback(IntPtr window, GtkEvent gtkEvent, IntPtr data);
        private delegate void RealizeCallback(IntPtr window);
        private delegate bool DestroyCallback(IntPtr window, IntPtr data);
        private delegate void QuitCallback();

        private void OnRealized(IntPtr window)
        {
            _xid = GetNativeHandle();
            var createdEvent = new CreatedEventArgs(IntPtr.Zero, window, _xid);
            Created?.Invoke(this, createdEvent);
            _isInitialized = true;
        }

        private void OnSizeAllocate(IntPtr window, IntPtr allocation, int baseline)
        {
            if (_handle != IntPtr.Zero && _isInitialized)
            {
                var size = GetWindowSize();
                SizeChanged?.Invoke(this, new SizeChangedEventArgs(size.Width, size.Height));
            }
        }

        private bool OnResize(IntPtr window, GtkEvent gtkEvent, IntPtr data)
        {
            if (_handle != IntPtr.Zero && _isInitialized)
            {
                Moving?.Invoke(this, new MovingEventArgs());
            }
            return false;
        }

        private bool OnDestroy(IntPtr window, IntPtr data)
        {
            Xilium.CefGlue.CefRuntime.QuitMessageLoop();
            if (_handle != IntPtr.Zero && _isInitialized)
            {
                Close?.Invoke(this, new CloseEventArgs());
            }

            return false;
        }

        private void ConnectRealizeSignal(RealizeCallback callback, GClosureNotify destroyData)
        {
            RegisterHandler(
                "realize",
                Marshal.GetFunctionPointerForDelegate(callback),
                destroyData,
                GConnectFlags.GConnectAfter,
                IntPtr.Zero);
        }

        private void ConnectSizeAllocateSignal(SizeAllocateCallback callback, GClosureNotify destroyData)
        {
            RegisterHandler(
                "size-allocate",
                Marshal.GetFunctionPointerForDelegate(callback),
                destroyData,
                GConnectFlags.GConnectAfter,
                IntPtr.Zero);
        }

        private void ConnectResizeSignal(ResizeCallback callback, GClosureNotify destroyData)
        {
            RegisterHandler(
                "configure-event",
                Marshal.GetFunctionPointerForDelegate(callback),
                destroyData,
                GConnectFlags.GConnectAfter,
                IntPtr.Zero);
        }

        private void ConnectDestroySignal(DestroyCallback callback, GClosureNotify destroyData)
        {
            RegisterHandler(
                "delete-event",
                Marshal.GetFunctionPointerForDelegate(callback),
                destroyData,
                GConnectFlags.GConnectAfter,
                IntPtr.Zero);
        }

        private void ConnectQuitSignal(QuitCallback callback, GClosureNotify destroyData)
        {
            RegisterHandler(
                "destroy",
                Marshal.GetFunctionPointerForDelegate(callback),
                destroyData,
                GConnectFlags.GConnectAfter,
                IntPtr.Zero);
        }

        private void FreeData()
        {
        }

        #endregion CreateWindow

        /// <summary>
        /// The win.
        /// </summary>
        private static class NativeMethods
        {
            internal const string GtkLib = "libgtk-3.so.0";
            internal const string GObjLib = "libgobject-2.0.so.0";
            internal const string GdkLib = "libgdk-3.so.0";
            internal const string GlibLib = "libglib-2.0.so.0";
            internal const string GioLib = "libgio-2.0.so.0";

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_init(int argc, string[] argv);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_window_new(GtkWindowType type);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_widget_get_window(IntPtr widget);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gdk_x11_window_get_xid(IntPtr raw);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_window_set_title(IntPtr window, string title);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_window_set_default_size(IntPtr window, int width, int height);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gdk_window_resize(IntPtr window, int width, int height);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gdk_window_move_resize(IntPtr window, int x, int y, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_set_icon_from_file(IntPtr raw, IntPtr filename, out IntPtr err);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool gtk_window_set_position(IntPtr window, GtkWindowPosition position);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool gtk_window_maximize(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool gtk_window_fullscreen(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_main_quit();

            // Signals
            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern uint g_signal_connect_data(IntPtr instance, string detailedSignal, IntPtr handler,
                IntPtr data, GClosureNotify destroyData, GConnectFlags connectFlags);

            // MessageBox
            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_message_dialog_new(IntPtr parent_window, DialogFlags flags, MessageType type, ButtonsType bt, string msg, IntPtr args);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int gtk_dialog_run(IntPtr raw);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_widget_destroy(IntPtr widget);
        }
    }
}