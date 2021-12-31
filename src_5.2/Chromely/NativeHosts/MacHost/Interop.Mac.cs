// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace Chromely.NativeHost
{
    public class InteropMac
    {
        internal const string ChromelyMacLib = "libchromely.dylib";

        [StructLayout(LayoutKind.Sequential)]
        public struct ChromelyParam
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
            public IntPtr titleUtf8Ptr;
            public IntPtr runMessageLoopCallback;
            public IntPtr cefShutdownCallback;
            public IntPtr initCallback;
            public IntPtr createCallback;
            public IntPtr movingCallback;
            public IntPtr resizeCallback;
            public IntPtr exitCallback;
        }

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void createwindow(ref ChromelyParam chromelyParam);

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void run(IntPtr application);

        [DllImport(ChromelyMacLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quit(IntPtr application, IntPtr pool);
    }
}
