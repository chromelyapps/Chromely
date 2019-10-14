using Chromely.Core.Infrastructure;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chromely.Native
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

    [Flags]
    public enum DialogFlags
    {
        Modal = 1 << 0,
        DestroyWithParent = 1 << 1,
    }

    public enum MessageType
    {
        Info,
        Warning,
        Question,
        Error,
        Other,
    }

    public enum ButtonsType
    {
        None,
        Ok,
        Close,
        Cancel,
        YesNo,
        OkCancel,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        /// The left.
        /// // x position of upper-left corner
        /// </summary>
        public int Left;

        /// <summary>
        /// The top.
        /// // y position of upper-left corner
        /// </summary>
        public int Top;

        /// <summary>
        /// The right.
        /// // x position of lower-right corner
        /// </summary>
        public int Right;

        /// <summary>
        /// The bottom.
        /// y position of lower-right corner
        /// </summary>
        public int Bottom;
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

    public static class Utils
    {
        public static void AssertNotNull(string methodName, IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Handle not valid in Method:{handle}");
            }
        }
    }

    public static class IconHandler
    {
        public static IntPtr IconFileToPtr(string iconFile)
        {
            try
            {
                if (string.IsNullOrEmpty(iconFile))
                {
                    return IntPtr.Zero;
                }

                if (!File.Exists(iconFile))
                {
                    // If local file
                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    iconFile = Path.Combine(appDirectory, iconFile);
                    if (!File.Exists(iconFile))
                    {
                        return IntPtr.Zero;
                    }
                }

                var iconBytes = File.ReadAllBytes(iconFile);
                return Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * iconBytes.Length);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return IntPtr.Zero;
        }
    }
}
