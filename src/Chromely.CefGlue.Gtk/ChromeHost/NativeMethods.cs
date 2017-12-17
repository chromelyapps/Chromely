namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue;

    internal static class NativeMethods
    {
        internal static void InitWindow(int argc, string[] argv)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_init(argc, argv);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_init(argc, argv);
            }
        }

        internal static IntPtr GetWindowXid(IntPtr handle)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                IntPtr pGdkWindow = Win.gtksharp_gtk_widget_get_window(handle);
                return Win.gdk_win32_drawable_get_handle(pGdkWindow);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                IntPtr pGdkWindow = Linux.gtk_widget_get_window(handle);
                return Linux.gdk_x11_drawable_get_xid(pGdkWindow);
            }

            return IntPtr.Zero;
        }

        internal static void ShowAll(IntPtr window)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_widget_show_all(window);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_widget_show_all(window);
            }
        }

        internal static void Run()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_main();
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_main();
            }
        }

        internal static void Quit()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_main_quit();
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_main_quit();
            }
        }

        internal static IntPtr NewWindow(GtkWindowType type)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                 return Win.gtk_window_new(type);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                return Linux.gtk_window_new(type);
            }

            return IntPtr.Zero;
        }


        internal static IntPtr CreateCefHost(IntPtr window)
        {
            IntPtr vBox = IntPtr.Zero;

            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                vBox = Win.gtk_vbox_new(true, 0);
                Win.gtk_container_add(window, vBox);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                vBox = Linux.gtk_vbox_new(true, 0);
                Linux.gtk_container_add(window, vBox);
            }

            return vBox;
        }

        internal static void SetTitle(IntPtr window, string title)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_set_title(window, title);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_set_title(window, title);
            }
        }

        internal static void SetWindowSize(IntPtr window, int width, int height)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_resize(window, width, height);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_resize(window, width, height);
            }
        }

        internal static void SetWindowResizable(IntPtr window, bool resizable)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_set_resizable(window, resizable);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_set_resizable(window, resizable);
            }
        }

        internal static void SetWindowPosition(IntPtr window, GtkWindowPosition position)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_set_position(window, position);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_set_position(window, position);
            }
        }

        internal static void AddConfigureEvent(IntPtr window)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_widget_add_events(window, GtkEvent.GDK_CONFIGURE);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_widget_add_events(window, GtkEvent.GDK_CONFIGURE);
            }
        }

        internal static void GetWindowSize(IntPtr window, out int width, out int height)
        {
            width = 0;
            height = 0;

            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_get_size(window, out width, out height);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_get_size(window, out width, out height);
            }
        }

        internal static void ConnectSignal(IntPtr window, string name, Delegate callback, int key, IntPtr data, int flags)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.g_signal_connect_data(window, name, callback, key, data, flags);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.g_signal_connect_data(window, name, callback, key, data, flags);
            }
        }

        internal static void SetIconFromFile(IntPtr handle, string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                if (!File.Exists(filename))
                {
                    // log error
                    return;
                }

                IntPtr ptrToAnsi = Marshal.StringToHGlobalAnsi(filename);
                IntPtr zero = IntPtr.Zero;
                if (CefRuntime.Platform == CefRuntimePlatform.Windows)
                {
                    Win.gtk_window_set_icon_from_file(handle, ptrToAnsi, out zero);
                }
                else
                {
                    Linux.gtk_window_set_icon_from_file(handle, ptrToAnsi, out zero);
                }

                Marshal.FreeHGlobal(ptrToAnsi);
                if (zero != IntPtr.Zero)
                {
                    // log error
                }
            }
            catch (Exception exception)
            {
                // log error
            }
        }

        private static class Win
        {
            internal const string GtkLib = "libgtk-win32-2.0-0.dll";
            internal const string GObjLib = "libgobject-2.0-0.dll";
            internal const string GdkLib = "libgdk-win32-2.0-0.dll";
            internal const string GlibLib = "libglib-2.0-0.dll";

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_init(int argc, string[] argv);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_window_new(GtkWindowType type);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_widget_get_window(IntPtr widget);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_vbox_new(bool homogeneous, int spacing);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_container_add(IntPtr container, IntPtr widget);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gdk_win32_drawable_get_handle(IntPtr raw);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_window_set_title(IntPtr window, string title);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_set_icon_from_file(IntPtr raw, IntPtr filename, out IntPtr err);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_set_position(IntPtr window, GtkWindowPosition position);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_add_events(IntPtr window, GtkEvent eventType);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_resize(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_set_resizable(IntPtr window, bool resizable);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main_quit();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_destroy(IntPtr window);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_connect_data(IntPtr window, string name, Delegate cb, int key, IntPtr p, int flags);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_handler_disconnect(IntPtr instance, uint handler_id);

            [DllImport("gtksharpglue-2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern IntPtr gtksharp_gtk_widget_get_window(IntPtr widget);
        }

        private static class Linux
        {
            internal const string GtkLib = "libgtk-x11-2.0.so.0";
            internal const string GObjLib = "libgobject-2.0.so.0";
            internal const string GdkLib = "libgdk-x11-2.0.so.0";
            internal const string GlibLib = "libglib-2.0.so.0";

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_init(int argc, string[] argv);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_window_new(GtkWindowType type);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_widget_get_window(IntPtr widget);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_vbox_new(bool homogeneous, int spacing);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_container_add(IntPtr container, IntPtr widget);

            [DllImport(GdkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gdk_x11_drawable_get_xid(IntPtr raw);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_window_set_title(IntPtr window, string title);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_set_icon_from_file(IntPtr raw, IntPtr filename, out IntPtr err);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_set_position(IntPtr window, GtkWindowPosition position);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_add_events(IntPtr window, GtkEvent eventType);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_resize(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_set_resizable(IntPtr window, bool resizable);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main_quit();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_destroy(IntPtr window);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_connect_data(IntPtr window, string name, Delegate cb, int key, IntPtr p, int flags);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_handler_disconnect(IntPtr instance, uint handler_id);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        internal struct StructEventArgs
        {
            IntPtr Handle;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void EventHandler(StructEventArgs eventArgs);

        internal enum GtkWindowType
        {
            GTK_WINDOW_TOPLEVEL,
            GTK_WINDOW_POPUP
        }

        internal enum GtkEvent
        {
            GDK_DESTROY = 1,
            GDK_EXPOSE = 2,
            GDK_MOTION_NOTIFY = 3,
            GDK_BUTTON_PRESS = 4,
            GDK_2BUTTON_PRESS = 5,
            GDK_3BUTTON_PRESS = 6,
            GDK_BUTTON_RELEASE = 7,
            GDK_KEY_PRESS = 8,
            GDK_KEY_RELEASE = 9,
            GDK_ENTER_NOTIFY = 10,
            GDK_LEAVE_NOTIFY = 11,
            GDK_FOCUS_CHANGE = 12,
            GDK_CONFIGURE = 13,
        }

        internal enum GtkWindowPosition
        {
            GTK_WIN_POS_NONE,
            GTK_WIN_POS_CENTER,
            GTK_WIN_POS_MOUSE,
            GTK_WIN_POS_CENTER_ALWAYS,
            GTK_WIN_POS_CENTER_ON_PARENT
        }

        internal enum GConnectFlags
        {
            /// <summary>
            /// whether the handler should be called before or after the default handler of the signal.
            /// </summary>
            G_CONNECT_AFTER,

            /// <summary>
            /// whether the instance and data should be swapped when calling the handler; see g_signal_connect_swapped() for an example.
            /// </summary>
            G_CONNECT_SWAPPED
        }

        [Flags]
        internal enum SetWindowPosFlags : uint
        {
            /// <summary>
            /// If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.
            /// </summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsyncWindowPosition = 0x4000,

            /// <summary>
            /// Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,

            /// <summary>
            /// Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,

            /// <summary>
            /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.
            /// </summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,

            /// <summary>
            /// Hides the window.
            /// </summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,

            /// <summary>
            /// Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            NoActivate = 0x0010,

            /// <summary>
            /// Discards the entire contents of the client area. If this flag is not specified, the valid contents 
            /// of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            NoCopyBits = 0x0100,

            /// <summary>
            /// Retains the current position (ignores X and Y parameters).
            /// </summary>
            /// <remarks>SWP_NOMOVE</remarks>
            NoMove = 0x0002,

            /// <summary>
            /// Does not change the owner window's position in the Z order.
            /// </summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            NoOwnerZOrder = 0x0200,

            /// <summary>
            /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            NoRedraw = 0x0008,

            /// <summary>
            /// Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            NoReposition = 0x0200,

            /// <summary>
            /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            NoSendChanging = 0x0400,

            /// <summary>
            /// Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            /// <remarks>SWP_NOSIZE</remarks>
            NoSize = 0x0001,

            /// <summary>
            /// Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            /// <remarks>SWP_NOZORDER</remarks>
            NoZOrder = 0x0004,

            /// <summary>
            /// Displays the window.
            /// </summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
    }
}
