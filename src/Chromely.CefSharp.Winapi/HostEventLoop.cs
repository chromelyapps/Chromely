// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostEventLoop.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi
{
    using System;

    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Host;
    using WinApi.User32;
    using WinApi.Windows;

    using global::CefSharp;

    /// <summary>
    /// The host event loop.
    /// </summary>
    public class HostEventLoop : EventLoopCore
    {
        /// <summary>
        /// The m_host config.
        /// </summary>
        private ChromelyConfiguration mHostConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostEventLoop"/> class.
        /// </summary>
        public HostEventLoop() : base(null)
        {
            this.mHostConfig = new ChromelyConfiguration();
        }

        /// <summary>
        /// The run core.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int RunCore()
        {
            Message msg;
            var quitMsg = (uint)WM.QUIT;

            bool isMultiThreadedLoopSet = this.mHostConfig.GetBooleanValue(CefSettingKeys.MultiThreadedMessageLoop, true);
            bool isExternalPumpSet = this.mHostConfig.GetBooleanValue(CefSettingKeys.ExternalMessagePump);

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
                        if (msg.Value == quitMsg)
                        {
                            break;
                        }

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

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="mainWindow">
        /// The main window.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public new int Run(WindowCore mainWindow = null)
        {
            if (mainWindow is IChromelyHost)
            {
                IChromelyHost chromelyHost = (IChromelyHost)mainWindow;
                this.mHostConfig = chromelyHost.HostConfig;
            }

            return base.Run(mainWindow);
        }
    }
}
