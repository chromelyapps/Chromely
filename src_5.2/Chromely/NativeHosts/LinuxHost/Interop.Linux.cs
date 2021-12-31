// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace Chromely.NativeHost
{
    public delegate void GClosureNotify();

    [Flags]
    public enum GApplicationFlags
    {
        None
    }

    [Flags]
    public enum GtkWindowType
    {
        GtkWindowToplevel,
        GtkWindowPopup
    }

    [Flags]
    public enum GtkEvent
    {
        GdkDestroy = 1,
        GdkExpose = 2,
        GdkMotionNotify = 3,
        GdkButtonPress = 4,
        Gdk_2ButtonPress = 5,
        Gdk_3ButtonPress = 6,
        GdkButtonRelease = 7,
        GdkKeyPress = 8,
        GdkKeyRelease = 9,
        GdkEnterNotify = 10,
        GdkLeaveNotify = 11,
        GdkFocusChange = 12,
        GdkConfigure = 13,
    }

    [Flags]
    public enum GtkWindowPosition
    {
        GtkWinPosNone,
        GtkWinPosCenter,
        GtkWinPosMouse,
        GtkWinPosCenterAlways,
        GtkWinPosCenterOnParent
    }

    [Flags]
    public enum GConnectFlags
    {
        /// <summary>
        /// whether the handler should be called before or after the default handler of the signal.
        /// </summary>
        GConnectAfter,

        /// <summary>
        /// whether the instance and data should be swapped when calling the handler; see g_signal_connect_swapped() for an example.
        /// </summary>
        GConnectSwapped
    }

    public partial class InteropLinux
    {
        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_set_allowed_backends(string backend);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_init(int argc, string[] argv);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_window_new(GtkWindowType type);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        public static extern void gtk_main();

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_widget_get_window(IntPtr widget);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_set_visual(IntPtr widget, IntPtr visual);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_window_get_xid(IntPtr raw);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_widget_get_display(IntPtr window);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_show_all(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_window_set_title(IntPtr window, string title);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_set_default_size(IntPtr window, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_window_resize(IntPtr window, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_window_move_resize(IntPtr window, int x, int y, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_visual_get_xvisual(IntPtr handle);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        public static extern bool gtk_window_set_icon_from_file(IntPtr raw, string filename, out IntPtr err);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_set_position(IntPtr window, GtkWindowPosition position);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_maximize(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_fullscreen(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_main_quit();

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_get_default();

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_screen_lookup_visual(IntPtr screen, IntPtr xvisualid);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_list_visuals(IntPtr raw);

        // Signals
        [DllImport(Library.GObjLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint g_signal_connect_data(IntPtr instance, string detailedSignal, IntPtr handler,
            IntPtr data, GClosureNotify destroyData, GConnectFlags connectFlags);

        // MessageBox
        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_message_dialog_new(IntPtr parent_window, DialogFlags flags, MessageType type, ButtonsType bt, string msg, IntPtr args);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gtk_dialog_run(IntPtr raw);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_destroy(IntPtr widget);

        #region Icon work 
        [DllImport(Library.GdkPixBuf, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_pixbuf_new_from_file_utf8(IntPtr filename, out IntPtr error);
        #endregion
    }
}
