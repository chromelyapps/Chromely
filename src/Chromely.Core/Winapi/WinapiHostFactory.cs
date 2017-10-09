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

namespace Chromely.Core.Winapi
{
    using System;
    using WinApi.User32;
    using WinApi.Windows;

    public class WinapiHostFactory : WindowFactory
    {
        public WinapiHostFactory(string name, WindowClassStyles styles, IntPtr hInstance, IntPtr hIcon, IntPtr hCursor, IntPtr hBgBrush, WindowProc wndProc)
            : base(name, styles, hInstance, hIcon, hCursor, hBgBrush, wndProc)
        {
        }

        public static WindowFactory Init(string iconFullPath = null)
        {
            IntPtr? hIcon = NativeMethods.LoadIconFromFile(iconFullPath);
            return Create(null, WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_HREDRAW, null, hIcon, null, null, null);
        }

    }
}
