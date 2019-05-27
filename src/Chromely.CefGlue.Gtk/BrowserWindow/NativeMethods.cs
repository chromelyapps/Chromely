// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Gtk.BrowserWindow
{
    /// <summary>
    /// The native methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The event handler.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void EventHandler(StructEventArgs eventArgs);

        #region Enums
        /// <summary>
        /// The gtk window type.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        internal enum GtkWindowType
        {
            GtkWindowToplevel,
            GtkWindowPopup
        }

        /// <summary>
        /// The gtk event.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum GtkEvent
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

        /// <summary>
        /// The gtk window position.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        internal enum GtkWindowPosition
        {
            GtkWinPosNone,
            GtkWinPosCenter,
            GtkWinPosMouse,
            GtkWinPosCenterAlways,
            GtkWinPosCenterOnParent
        }

        /// <summary>
        /// The g connect flags.
        /// see: https://code.woboq.org/gtk/include/glib-2.0/gobject/gsignal.h.html#GConnectFlags
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        internal enum GConnectFlags
        {
            /// <summary>
            /// whether the handler should be called before or after the default handler of the signal.
            /// </summary>
            GConnectAfter = 1 << 0,

            /// <summary>
            /// whether the instance and data should be swapped when calling the handler; see g_signal_connect_swapped() for an example.
            /// </summary>
            GConnectSwapped = 1 << 1
        }

        /// <summary>
        /// The set window pos flags.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
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

        #endregion Enums

        /// <summary>
        /// The init window.
        /// </summary>
        /// <param name="argc">
        /// The argc.
        /// </param>
        /// <param name="argv">
        /// The argv.
        /// </param>
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

        /// <summary>
        /// The get window xid.
        /// </summary>
        /// <param name="handle">
        /// The handle.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
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

        /// <summary>
        /// The show all.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
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

        /// <summary>
        /// The run.
        /// </summary>
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

        /// <summary>
        /// The quit.
        /// </summary>
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

        /// <summary>
        /// The new window.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
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

        /// <summary>
        /// The create cef host.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
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

        /// <summary>
        /// The set title.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
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

        /// <summary>
        /// The set window size.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
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

        /// <summary>
        /// The set size request.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        internal static void SetSizeRequest(IntPtr window, int width, int height)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_widget_set_size_request(window, width, height);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_widget_set_size_request(window, width, height);
            }
        }

        /// <summary>
        /// The set window resizable.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="resizable">
        /// The resizable.
        /// </param>
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

        /// <summary>
        /// The set window pos.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="hWndInsertAfter">
        /// The h wnd insert after.
        /// </param>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="cx">
        /// The cx.
        /// </param>
        /// <param name="cy">
        /// The cy.
        /// </param>
        internal static void SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, SetWindowPosFlags.NoZOrder);
            }
        }

        /// <summary>
        /// The set window position.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
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

        /// <summary>
        /// The set window maximize.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        internal static void SetWindowMaximize(IntPtr window)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_maximize(window);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_maximize(window);
            }
        }

        /// <summary>
        /// The set fullscreen.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        internal static void SetFullscreen(IntPtr window)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_window_fullscreen(window);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_window_fullscreen(window);
            }
        }

        /// <summary>
        /// The add configure event.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        internal static void AddConfigureEvent(IntPtr window)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                Win.gtk_widget_add_events(window, GtkEvent.GdkConfigure);
            }

            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                Linux.gtk_widget_add_events(window, GtkEvent.GdkConfigure);
            }
        }

        /// <summary>
        /// The get window size.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
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

        /// <summary>
        /// The connect signal.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="flags">
        /// The flags.
        /// </param>
        internal static uint ConnectSignal(IntPtr window, string name, Delegate callback, int key, IntPtr data, int flags)
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    return Win.g_signal_connect_data(window, name, callback, key, data, flags);
                case CefRuntimePlatform.Linux:
                    return Linux.g_signal_connect_data(window, name, callback, key, data, flags);
                case CefRuntimePlatform.MacOSX:
                default:
                    return 0;
            }
        }

        internal static uint DisconnectSignal(IntPtr window, uint handler)
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    return Win.g_signal_handler_disconnect(window, handler);
                case CefRuntimePlatform.Linux:
                    return Linux.g_signal_handler_disconnect(window, handler);
                case CefRuntimePlatform.MacOSX:
                default:
                    return 0;
            }
        }
        
        /// <summary>
        /// The set icon from file.
        /// </summary>
        /// <param name="handle">
        /// The handle.
        /// </param>
        /// <param name="filename">
        /// The filename.
        /// </param>
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
                    return;
                }

                var ptrToAnsi = Marshal.StringToHGlobalAnsi(filename);
                IntPtr zero;
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
                    Log.Error("Icon handle not successfully freed.");
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        /// <summary>
        /// The struct event args.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        internal struct StructEventArgs
        {
            /// <summary>
            /// The handle.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed. Suppression is OK here.")]
            private readonly IntPtr Handle;
        }

        /// <summary>
        /// The win.
        /// </summary>
        private static class Win
        {
            /// <summary>
            /// The gtk lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GtkLib = "libgtk-win32-2.0-0.dll";

            /// <summary>
            /// The g obj lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GObjLib = "libgobject-2.0-0.dll";

            /// <summary>
            /// The gdk lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GdkLib = "libgdk-win32-2.0-0.dll";

            /// <summary>
            /// The glib lib.
            /// </summary>
            // ReSharper disable once UnusedMember.Local
            internal const string GlibLib = "libglib-2.0-0.dll";

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_init(int argc, string[] argv);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern IntPtr gtk_window_new(GtkWindowType type);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
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

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_maximize(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_fullscreen(IntPtr window);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_resize(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_set_size_request(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_set_resizable(IntPtr window, bool resizable);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern void gtk_widget_show(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main_quit();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern void gtk_widget_destroy(IntPtr window);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_connect_data(IntPtr window, string name, Delegate cb, int key, IntPtr p, int flags);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern uint g_signal_handler_disconnect(IntPtr instance, uint handlerId);

            [DllImport("gtksharpglue-2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern IntPtr gtksharp_gtk_widget_get_window(IntPtr widget);
        }

        /// <summary>
        /// The linux.
        /// </summary>
        private static class Linux
        {
            /// <summary>
            /// The gtk lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GtkLib = "libgtk-x11-2.0.so.0";

            /// <summary>
            /// The g obj lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GObjLib = "libgobject-2.0.so.0";

            /// <summary>
            /// The gdk lib.
            /// </summary>
            // ReSharper disable once MemberCanBePrivate.Local
            internal const string GdkLib = "libgdk-x11-2.0.so.0";

            /// <summary>
            /// The glib lib.
            /// </summary>
            // ReSharper disable once UnusedMember.Local
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
            internal static extern bool gtk_window_maximize(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern bool gtk_window_fullscreen(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_add_events(IntPtr window, GtkEvent eventType);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_resize(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_set_size_request(IntPtr window, int width, int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_set_resizable(IntPtr window, bool resizable);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern void gtk_widget_show(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_widget_show_all(IntPtr window);

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern void gtk_main_quit();

            [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern void gtk_widget_destroy(IntPtr window);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            internal static extern uint g_signal_connect_data(IntPtr window, string name, Delegate cb, int key, IntPtr p, int flags);

            [DllImport(GObjLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
            // ReSharper disable once UnusedMember.Local
            internal static extern uint g_signal_handler_disconnect(IntPtr instance, uint handlerId);
        }
    }
}
