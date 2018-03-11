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

namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using Chromely.CefGlue.Gtk.Browser;
    using Chromely.Core;
    using System;
    using Xilium.CefGlue;

    public sealed class Window : NativeWindow
    {
        private CefGlueBrowser m_core;
        private IntPtr m_browserWindowHandle;
        private ChromelyConfiguration m_hostConfig;
        private readonly HostBase m_application;

        public Window(HostBase application, ChromelyConfiguration hostConfig)
            : base(hostConfig.HostTitle, hostConfig.HostWidth, hostConfig.HostHeight, hostConfig.HostIconFile)
        {
            m_hostConfig = hostConfig;
            m_core = new CefGlueBrowser(this, new CefBrowserSettings(), hostConfig.StartUrl);
            m_core.Created += new EventHandler(BrowserCreated);
            m_application = application;

            ShowWindow();
        }

        public void Close()
        {
            Dispose();
        }

        public CefGlueBrowser WebBrowser
        {
            get
            {
                return m_core;
            }
        }

        protected override void OnRealized(object sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows:
                    var parentHandle = HostXid;
                    windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, m_hostConfig.HostWidth, m_hostConfig.HostHeight)); 
                    break;

                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(HostXid, new CefRectangle(0, 0, m_hostConfig.HostWidth, m_hostConfig.HostHeight));
                    break;

                case CefRuntimePlatform.MacOSX:
                default:
                    throw new NotSupportedException();
            }

            m_core.Create(windowInfo);
        }

        protected override void OnResize(object sender, EventArgs e)
        {
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
            {
                if (m_browserWindowHandle != IntPtr.Zero)
                {
                    int width;
                    int height;
                    GetSize(out width, out height);

                    NativeMethods.SetWindowPos(m_browserWindowHandle, IntPtr.Zero,
                        0, 0, width, height);
                }
            }
            else
            {
                base.OnResize(sender, e);
            }
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            m_application.Quit();
        }

        private void BrowserCreated(object sender, EventArgs e)
        {
            m_browserWindowHandle = m_core.CefBrowser.GetHost().GetWindowHandle();
        }

        public void Dispose()
        {
            if (m_core != null)
            {
                var browser = m_core.CefBrowser;
                var host = browser.GetHost();
                host.CloseBrowser();
                host.Dispose();
                browser.Dispose();
                browser = null;
                m_browserWindowHandle = IntPtr.Zero;
            }
        }
    }
}
