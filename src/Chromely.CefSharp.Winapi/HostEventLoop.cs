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

namespace Chromely.CefSharp.Winapi
{
    using System;
    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Host;
    using WinApi.User32;
    using WinApi.Windows;

    using global::CefSharp;

    public class HostEventLoop : EventLoopCore
    {
        public HostEventLoop() : base(null)
        {
            m_hostConfig = new ChromelyConfiguration();
        }

        private ChromelyConfiguration m_hostConfig;

        public override int RunCore()
        {
            Message msg;
            var quitMsg = (uint)WM.QUIT;

            bool isMultiThreadedLoopSet = m_hostConfig.GetBooleanValue(CefSettingKeys.MultiThreadedMessageLoop, true);
            bool isExternalPumpSet = m_hostConfig.GetBooleanValue(CefSettingKeys.ExternalMessagePump, false);

            if (!isMultiThreadedLoopSet && !isExternalPumpSet)
            {
                Cef.RunMessageLoop();
            }
            else
            {
                while (true)
                {
                    if (User32Helpers.PeekMessage(out msg, IntPtr.Zero, 0, 0, PeekMessageFlags.PM_REMOVE))
                    {
                        if (msg.Value == quitMsg) break;

                        User32Methods.TranslateMessage(ref msg);
                        User32Methods.DispatchMessage(ref msg);
                    }

                    // Do your idle processing
                    if (isExternalPumpSet)
                    {
                        Cef.DoMessageLoopWork();
                    }
                }
            }

            return 0;
        }

        public new int Run(WindowCore mainWindow = null)
        {
            if ((mainWindow != null) && (mainWindow is IChromelyHost))
            {
                IChromelyHost chromelyHost = (IChromelyHost)mainWindow;
                m_hostConfig = chromelyHost.HostConfig;
            }

            return base.Run(mainWindow);
        }
    }
}
