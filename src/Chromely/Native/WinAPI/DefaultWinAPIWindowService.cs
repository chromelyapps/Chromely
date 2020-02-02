using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Chromely.Core.Host;
using Xilium.CefGlue;
using static Chromely.Native.WinNativeMethods;

namespace FramelessDemo.Services.Window
{
    public class DefaultWinAPIWindowService : IFramelessWindowService
    {

        private readonly IntPtr _handle;

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public DefaultWinAPIWindowService(IntPtr handle)
        {
            _handle = handle;
        }

        public void Close()
        {
            if (_handle != IntPtr.Zero)
            {
                try
                {
                    ShowWindow(_handle, ShowWindowCommand.SW_HIDE);
                    CefRuntime.Shutdown();
                    Task.Run(() =>
                    {
                        int excelProcessId = -1;
                        GetWindowThreadProcessId(_handle, ref excelProcessId);
                        var process = Process.GetProcessById(excelProcessId);
                        if (process != null)
                        {
                            process.Kill();
                        }
                    });
                }
                catch { }
            }
        }

        public bool Maximize()
        {
            return ShowWindow(Handle, ShowWindowCommand.SW_SHOWMAXIMIZED);
        }

        public bool Minimize()
        {
            return ShowWindow(Handle, ShowWindowCommand.SW_SHOWMINIMIZED);
        }

        public bool Restore()
        {
            return ShowWindow(Handle, ShowWindowCommand.SW_RESTORE);
        }

    }
}