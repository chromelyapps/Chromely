using System;
using System.Runtime.InteropServices;

namespace Chromely.Native
{
    internal class MacNativeMethods
    {
        internal const string ChromelyMacLib = "libchromely.dylib";

        [StructLayout(LayoutKind.Sequential)]
        internal struct ChromelyParam
        {
            public int x;
            public int y;
            public int width;
            public int height;
            public int centerscreen;
            public int frameless;
            public int fullscreen;
            public int noresize;
            public int nominbutton;
            public int nomaxbutton;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string title;
            public IntPtr runMessageLoopCallback;
            public IntPtr cefShutdownCallback;
            public IntPtr initCallback;
            public IntPtr createCallback;
            public IntPtr movingCallback;
            public IntPtr resizeCallback;
            public IntPtr exitCallback;
        }

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void createwindow(ref ChromelyParam chromelyParam);

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void run(IntPtr application);

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void quit(IntPtr application, IntPtr pool);
    }
}
