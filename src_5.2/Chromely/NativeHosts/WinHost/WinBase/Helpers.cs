// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable CA1822
#pragma warning disable CA2211

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
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

    public static class Utils
    {
        public static void AssertNotNull(string methodName, IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Handle not valid in Method:{methodName}");
            }
        }
    }

    public class WindowStylePlacement
    {
        public static WS NormalStyles = WS.OVERLAPPEDWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
        public static WS_EX NormalExStyles = WS_EX.APPWINDOW | WS_EX.WINDOWEDGE;

        public WindowStylePlacement(IWindowOptions options)
        {
            RECT = new Rectangle(options.Position.X, options.Position.Y, options.Size.Width, options.Size.Height);
            WindowPlacement = new WINDOWPLACEMENT();
            if (options.CustomStyle is not null && 
                options.CustomStyle.WindowStyles != 0 &&
                options.CustomStyle.WindowExStyles != 0)
            {
                Styles = (WS)options.CustomStyle.WindowStyles;
                ExStyles = (WS_EX)options.CustomStyle.WindowExStyles;
            }
            else
            {
                Styles = NormalStyles;
                ExStyles = NormalExStyles;
            }

            State = options.WindowState;
        }

        public WindowState State { get; set; }
        public WS Styles { get; set; }
        public WS_EX ExStyles { get; set; }
        public RECT RECT { get; set; }
        public SW ShowCommand { get; set; }
        public WINDOWPLACEMENT WindowPlacement { get; set; }
        public WS FullscreenStyles 
        { 
            get
            {
                var styles = WS.OVERLAPPEDWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
                styles &= ~(WS.CAPTION);
                return styles;
            }
        }
        public WS_EX FullscreenExStyles
        {
            get
            {
                var exStyles = WS_EX.APPWINDOW;
                exStyles &= ~(WS_EX.DLGMODALFRAME | WS_EX.WINDOWEDGE | WS_EX.CLIENTEDGE | WS_EX.STATICEDGE);
                return exStyles;
            }
        }
    }

    internal static class WindowHelper
    {
        public static class HwndZOrder
        {
            public static IntPtr
            NoTopMost = new(-2),
            TopMost = new(-1),
            Top = new(0),
            Bottom = new(1);
        }

        public static void CenterWindowToScreen(IntPtr hwnd, bool useWorkArea = true)
        {
            try
            {
                IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR.DEFAULTTONEAREST);

                MONITORINFOEXW monInfo = new(null);
                monInfo.cbSize = (uint)Marshal.SizeOf(monInfo);

                GetMonitorInfoW(handle, ref monInfo);
                var screenRect = useWorkArea ? monInfo.rcWork : monInfo.rcMonitor;
                var midX = (monInfo.rcMonitor.right - monInfo.rcMonitor.left) / 2;
                var midY = (monInfo.rcMonitor.bottom - monInfo.rcMonitor.top) / 2;
                var size = GetWindowSize(hwnd);
                var left = midX - (size.Width / 2);
                var top = midY - (size.Height / 2);

                SetWindowPos(
                    hwnd,
                    IntPtr.Zero,
                    left,
                    top,
                    -1,
                    -1,
                    SWP.NOACTIVATE | SWP.NOSIZE | SWP.NOZORDER);
            }
            catch { }
            {
            }
        }

        public static Rectangle FullScreenBounds(Rectangle configuredBounds)
        {
            try
            {
                IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR.DEFAULTTOPRIMARY);

                MONITORINFOEXW monInfo = new(null);

                GetMonitorInfoW(handle, ref monInfo);
                RECT rect = monInfo.rcMonitor;

                if (rect.Width <= 0 || rect.Height <= 0) return configuredBounds;

                return rect;
            }
            catch { }
            {
            }

            return configuredBounds;
        }

        public static Size GetWindowSize(IntPtr hwnd)
        {
            var size = new Size();
            if (hwnd != IntPtr.Zero)
            {
                RECT rect = new();
                GetWindowRect(hwnd, ref rect);
                size.Width = rect.Width;
                size.Height = rect.Height;
            }

            return size;
        }
    }

    internal static class IconHandler
    {
        public static IntPtr? LoadIconFromFile(string iconFullPath)
        {
            if (string.IsNullOrEmpty(iconFullPath))
            {
                return null;
            }

            if (!File.Exists(iconFullPath))
            {
                // If local file
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                iconFullPath = Path.Combine(appDirectory, iconFullPath);
                if (!File.Exists(iconFullPath))
                {
                    return null;
                }
            }

            return LoadImage(                       // returns a HANDLE so we have to cast to HICON
                  IntPtr.Zero,                      // hInstance must be NULL when loading from a file
                  iconFullPath,                     // the icon file name
                  (uint)LR.IMAGE_ICON,              // specifies that the file is an icon
                  0,                                // width of the image (we'll specify default later on)
                  0,                                // height of the image
                  (uint)LR.LOADFROMFILE |           // we want to load a file (as opposed to a resource)
                  (uint)LR.DEFAULTSIZE |            // default metrics based on the type (IMAGE_ICON, 32x32)
                  (uint)LR.SHARED                   // let the system release the handle when it's no longer used
            );
        }

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
                Logger.Instance.Log.LogError(exception, "IconHandler:IconFullPath");
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
                Logger.Instance.Log.LogError(exception, "IconHandler:IconFileToPtr");
            }

            return IntPtr.Zero;
        }
    }
}
