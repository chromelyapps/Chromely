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
        public static string IconFullPath(string iconFile)
        {
            try
            {
                if (string.IsNullOrEmpty(iconFile))
                {
                    return iconFile;
                }

                if (!File.Exists(iconFile) || !Path.IsPathRooted(iconFile))
                {
                    // If local file
                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    iconFile = Path.Combine(appDirectory, iconFile);
                    if (!File.Exists(iconFile))
                    {
                        return string.Empty;
                    }

                    return iconFile;
                }

                return iconFile;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return iconFile;
        }

        public static IntPtr IconFileToPtr(string iconFile)
        {
            try
            {
                iconFile = IconFullPath(iconFile);
                if (string.IsNullOrEmpty(iconFile))
                {
                    return IntPtr.Zero;
                }

                var iconBytes = File.ReadAllBytes(iconFile);
                return Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * iconBytes.Length);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return IntPtr.Zero;
        }
    }
}
