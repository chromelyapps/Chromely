using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;

namespace Chromely.Native
{
    public class WinGtk3Host : INativeHost
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

        public WinGtk3Host()
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
            ConnectQuitSignal(NativeMethods.gtk_main_quit, FreeData);

            ShowWindow();
        }

        public IntPtr CreateNewWindow(GtkWindowType type)
        {
            _handle = NativeMethods.gtk_window_new(type);
            Utils.AssertNotNull("CreateNewWindow", _handle);
            return _handle;
        }

        public IntPtr GetGdkHandle()
        {
            return NativeMethods.gtk_widget_get_window(_handle);
        }

        public IntPtr GetNativeHandle()
        {
            IntPtr pGdkWindow = NativeMethods.gtk_widget_get_window(_handle);
            Utils.AssertNotNull("GetNativeHandle:gtk_widget_get_window", pGdkWindow);
            _xid = NativeMethods.gdk_win32_window_get_handle(pGdkWindow);
            Utils.AssertNotNull("GetNativeHandle:gdk_win32_window_get_handle", _xid);
            return _xid;
        }

        public void Init(int argc, string[] argv)
        {
            NativeMethods.gtk_init(argc, argv);
        }

        public void ShowWindow()
        {
            NativeMethods.gtk_widget_show_all(_handle);
        }
        public void Run()
        {
            NativeMethods.gtk_main();
        }

        public Size GetWindowSize()
        {
            NativeMethods.gtk_window_get_size(_handle, out int width, out int height);
            return new Size(width, height);
        }

        public void SetWindowTitle(string title)
        {
            NativeMethods.gtk_window_set_title(_handle, title);
        }

        public void SetWindowDefaultSize(int width, int height)
        {
            NativeMethods.gtk_window_set_default_size(_handle, width, height);
        }

        public void ResizeWindow(IntPtr window, int width, int height)
        {
            NativeMethods.SetWindowPos(window, IntPtr.Zero, 0, 0, width, height, SetWindowPosFlags.NoZOrder);
        }

        public void SetAppIcon(IntPtr window, string filename)
        {
            try
            {
                filename = IconHandler.IconFullPath(filename);
                if (string.IsNullOrWhiteSpace(filename))
                {
                    IntPtr error = IntPtr.Zero;
                    NativeMethods.gtk_window_set_icon_from_file(window, filename, out error);
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
            NativeMethods.gtk_window_set_position(_handle, position);
        }

        public void SetWindowMaximize()
        {
            NativeMethods.gtk_window_maximize(_handle);
        }

        public void SetFullscreen()
        {
            NativeMethods.gtk_window_fullscreen(_handle);
        }

        public void Exit()
        {
            Xilium.CefGlue.CefRuntime.Shutdown();
        }

        public void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
        {
            NativeMethods.g_signal_connect_data(_handle, signalName, handler, data, destroyData, connectFlags);
        }

        public void MessageBox(string message, MessageType messageType = MessageType.Error)
        {
            var diag = NativeMethods.gtk_message_dialog_new(IntPtr.Zero, DialogFlags.Modal, messageType, ButtonsType.Ok, message, IntPtr.Zero);
            NativeMethods.gtk_dialog_run(diag);
            NativeMethods.gtk_widget_destroy(diag);
        }

        #region CreateWindow

        private delegate void SizeAllocateCallback(IntPtr window, IntPtr allocation, int baseline);
        private delegate bool ResizeCallback(IntPtr window, GtkEvent gtkEvent, IntPtr data);
        private delegate void RealizeCallback(IntPtr window);
        private delegate bool DestroyCallback(IntPtr window, IntPtr data);
        private delegate void QuitCallback();

        private void OnRealized(IntPtr window)
        {
            try
            {
                _xid = GetNativeHandle();
                if (_xid == IntPtr.Zero)
                {
                    throw new Exception("Window XID is invalid");
                }

                var createdEvent = new CreatedEventArgs(IntPtr.Zero, window, _xid);
                Created?.Invoke(this, createdEvent);
                _isInitialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error in LinuxGtk3Host::OnRealized");
                Log.Error(exception);
            }
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
            if (_hostConfig.Platform != ChromelyPlatform.Windows)
            {
                Xilium.CefGlue.CefRuntime.QuitMessageLoop();
            }

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
        #endregion CreateWindo

        /// <summary>
        /// The win.
        /// </summary>
        private static class NativeMethods
        {
            internal const string GtkLib = "libgtk-3-0.dll";
            internal const string GObjLib = "libgobject-2.0-0.dll";
            internal const string GdkLib = "libgdk-3-0.dll";
            internal const string GlibLib = "libglib-2.0-0.dll";
            internal const string GioLib = "libgio-2.0-0.dll";

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gdk_win32_window_get_handle(IntPtr raw);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_init(int argc, string[] argv);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_window_new(GtkWindowType type);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_widget_get_window(IntPtr app);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gtk_window_set_title(IntPtr window, string title);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gtk_window_set_default_size(IntPtr window, int width, int height);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void gdk_window_move_resize(IntPtr window, int x, int y, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool gtk_window_set_icon_from_file(IntPtr raw, string filename, out IntPtr err);

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

            // Win32 API
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);
        }
    }
}