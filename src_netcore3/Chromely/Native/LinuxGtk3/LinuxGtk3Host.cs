using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using static Chromely.Native.LinuxNativeMethods;

namespace Chromely.Native
{
    public class LinuxGtk3Host : IChromelyNativeHost
    {
        public event EventHandler<CreatedEventArgs> Created;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<CloseEventArgs> Close;

        private IChromelyConfiguration _config;
        private IntPtr _handle;
        private IntPtr _gdkHandle;
        private IntPtr _xid;
        private bool _isInitialized;

        public LinuxGtk3Host()
        {
            _isInitialized = false;
            _handle = IntPtr.Zero;
            _gdkHandle = IntPtr.Zero;
            _xid = IntPtr.Zero;
        }

        public void CreateWindow(IChromelyConfiguration config)
        {
            _config = config;

            Init(0, null);

            var wndType = _config.WindowFrameless
                ? GtkWindowType.GtkWindowPopup
                : GtkWindowType.GtkWindowToplevel;

            _handle = CreateNewWindow((int)wndType);

            SetWindowTitle(_config.WindowTitle);
            SetAppIcon(_handle, _config.WindowIconFile);
            SetWindowDefaultSize(_config.WindowWidth, _config.WindowHeight);

            if (_config.WindowState == WindowState.Normal && _config.WindowCenterScreen)
            {
                SetWindowPosistion((int)GtkWindowPosition.GtkWinPosCenter);
            }

            switch (_config.WindowState)
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

        public IntPtr GetNativeHandle()
        {
            try
            {
                _gdkHandle = gtk_widget_get_window(_handle);
                Utils.AssertNotNull("GetNativeHandle:gtk_widget_get_window", _gdkHandle);
                _xid = gdk_x11_window_get_xid(_gdkHandle);
                Utils.AssertNotNull("GetNativeHandle:gdk_x11_window_get_xid", _xid);
                return _xid;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::GetNativeHandle");
                Logger.Instance.Log.Error(exception);
            }

            return IntPtr.Zero;
        }

        public void Run()
        {
            try
            {
                gtk_main();
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::Run");
                Logger.Instance.Log.Error(exception);
            }
        }

        public Size GetWindowClientSize()
        {
            throw new NotImplementedException();
        }

        public void ResizeBrowser(IntPtr window, int width, int height)
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

        public void SetWindowMaximize()
        {
            try
            {
                gtk_window_maximize(_handle);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::SetWindowMaximize");
                Logger.Instance.Log.Error(exception);
            }
        }

        public void SetFullscreen()
        {
            try
            {
                gtk_window_fullscreen(_handle);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::SetFullscreen");
                Logger.Instance.Log.Error(exception);
            }
        }

        public void Exit()
        {
            try
            {
                gtk_main_quit();
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::Quit");
                Logger.Instance.Log.Error(exception);
            }
        }

        public void MessageBox(string message, int type)
        {
            try
            {
                MessageType messageType = (MessageType)type;
                var diag = gtk_message_dialog_new(IntPtr.Zero, DialogFlags.Modal, messageType, ButtonsType.Ok, message, IntPtr.Zero);
                gtk_dialog_run(diag);
                gtk_widget_destroy(diag);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::MessageBox");
                Logger.Instance.Log.Error(exception);
            }
        }

        #region CreateWindow

        private delegate void SizeAllocateCallback(IntPtr window, IntPtr allocation, int baseline);
        private delegate bool ResizeCallback(IntPtr window, GtkEvent gtkEvent, IntPtr data);
        private delegate void RealizeCallback(IntPtr window);
        private delegate bool DestroyCallback(IntPtr window, IntPtr data);
        private delegate void QuitCallback();

        private void Init(int argc, string[] argv)
        {
            try
            {
                gtk_init(argc, argv);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::Init");
                Logger.Instance.Log.Error(exception);
            }
        }

        private IntPtr CreateNewWindow(int windowType)
        {
            try
            {
                GtkWindowType type = (GtkWindowType)windowType;
                _handle = gtk_window_new(type);
                Utils.AssertNotNull("CreateNewWindow", _handle);
                return _handle;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::CreateNewWindow");
                Logger.Instance.Log.Error(exception);
            }

            return IntPtr.Zero;
        }

        private IntPtr GetGdkHandle()
        {
            try
            {
                _gdkHandle = gtk_widget_get_window(_handle);
                Utils.AssertNotNull("GetGdkHandle", _gdkHandle);
                return _gdkHandle;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::GetGdkHandle");
                Logger.Instance.Log.Error(exception);
            }

            return IntPtr.Zero;
        }
        private void SetWindowTitle(string title)
        {
            try
            {
                gtk_window_set_title(_handle, title);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::SetWindowTitle");
                Logger.Instance.Log.Error(exception);
            }
        }

        private void SetWindowDefaultSize(int width, int height)
        {
            try
            {
                gtk_window_set_default_size(_handle, width, height);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::SetWindowDefaultSize");
                Logger.Instance.Log.Error(exception);
            }
        }

        private void SetWindowPosistion(int windowPosition)
        {
            try
            {
                GtkWindowPosition position = (GtkWindowPosition)windowPosition;
                gtk_window_set_position(_handle, position);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::SetWindowPosistion");
                Logger.Instance.Log.Error(exception);
            }
        }

        private void SetAppIcon(IntPtr window, string filename)
        {
            try
            {
                filename = IconHandler.IconFullPath(filename);
                if (string.IsNullOrWhiteSpace(filename))
                {
                    IntPtr error = IntPtr.Zero;
                    gtk_window_set_icon_from_file(window, filename, out error);
                    if (error != IntPtr.Zero)
                    {
                        Logger.Instance.Log.Error("Icon handle not successfully freed.");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        private void ShowWindow()
        {
            try
            {
                gtk_widget_show_all(_handle);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::ShowWindow");
                Logger.Instance.Log.Error(exception);
            }
        }

        private Size GetWindowSize()
        {
            try
            {
                gtk_window_get_size(_handle, out int width, out int height);
                return new Size(width, height);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::GetWindowSize");
                Logger.Instance.Log.Error(exception);
            }

            return new Size();
        }

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
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::OnRealized");
                Logger.Instance.Log.Error(exception);
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

        private void RegisterHandler(string signalName, IntPtr handler, GClosureNotify destroyData, GConnectFlags connectFlags = GConnectFlags.GConnectAfter, IntPtr data = default(IntPtr))
        {
            try
            {
                g_signal_connect_data(_handle, signalName, handler, data, destroyData, connectFlags);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error("Error in LinuxGtk3Host::RegisterHandler");
                Logger.Instance.Log.Error(exception);
            }
        }

        private void FreeData()
        {
        }

        #endregion CreateWindow
    }
}