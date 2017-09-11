/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.ChromeHosts
{
    using Chromely.Core.ChromeHosts.Winapi;
    using System;
    using WinApi.User32;
    using WinApi.Windows;

    public class ChromeHostFactory : WindowFactory
    {
        public ChromeHostFactory(string name, WindowClassStyles styles, IntPtr hInstance, IntPtr hIcon, IntPtr hCursor, IntPtr hBgBrush, WindowProc wndProc) 
            : base(name, styles, hInstance, hIcon, hCursor, hBgBrush, wndProc)
        {
        }

        public static WindowFactory CreateWinapi(string iconFullPath)
        {
            IntPtr iconPtr = WinapiNativeMethods.LoadImage(       // returns a HANDLE so we have to cast to HICON
              IntPtr.Zero,                                        // hInstance must be NULL when loading from a file
              iconFullPath,                                       // the icon file name
              (uint)ResourceImageType.IMAGE_ICON,                 // specifies that the file is an icon
              0,                                                  // width of the image (we'll specify default later on)
              0,                                                  // height of the image
              (uint)LoadResourceFlags.LR_LOADFROMFILE |           // we want to load a file (as opposed to a resource)
              (uint)LoadResourceFlags.LR_DEFAULTSIZE |            // default metrics based on the type (IMAGE_ICON, 32x32)
              (uint)LoadResourceFlags.LR_SHARED                   // let the system release the handle when it's no longer used
            );

            return  Create(null, WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_HREDRAW, null, iconPtr, null, null, null);
        }

    }
}
