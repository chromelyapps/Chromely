using System;
using System.Runtime.InteropServices;

namespace Chromely.Native
{
    internal class LinuxNativeMethods
    {
        internal const string GtkLib = "libgtk-3.so.0";
        internal const string GObjLib = "libgobject-2.0.so.0";
        internal const string GdkLib = "libgdk-3.so.0";
        internal const string GlibLib = "libglib-2.0.so.0";
        internal const string GioLib = "libgio-2.0.so.0";
        internal const string GdkPixBuf = "libgdk_pixbuf-2.0.so.0";

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
        internal static extern IntPtr gtk_widget_get_display(IntPtr window);

        [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

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

        #region Icon work 
        [DllImport(GdkPixBuf, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_pixbuf_new_from_file_utf8(IntPtr filename, out IntPtr error);
        #endregion

        #region X11

        internal delegate short XHandleXError(IntPtr display, ref XErrorEvent error_event);
        internal delegate short XHandleXIOError(IntPtr display);

        [DllImport("libX11.so")]
        internal static extern int XMoveWindow(IntPtr display, IntPtr w, int x, int y);

        [DllImport("libX11.so")]
        internal static extern int XResizeWindow(IntPtr display, IntPtr w, int width, int height);

        [DllImport("libX11.so")]
        internal static extern int XMoveResizeWindow(IntPtr display, IntPtr w, int x, int y, int width, int height);

        [DllImport("libX11.so")]
        internal static extern short XSetErrorHandler(XHandleXError err);

        [DllImport("libX11.so")]
        internal static extern short XSetIOErrorHandler(XHandleXIOError err);

        [StructLayout(LayoutKind.Sequential)]
        internal struct XErrorEvent
        {
            internal int type;
            internal IntPtr display;
            internal int resourceid;
            internal int serial;
            internal byte error_code;
            internal byte request_code;
            internal byte minor_code;
        }

        #endregion
    }
}
