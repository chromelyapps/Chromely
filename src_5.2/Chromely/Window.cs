// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Browser;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using static Chromely.Interop.User32;

namespace Chromely
{
    public partial class Window : ChromiumBrowser, IChromelyWindow
    {
        public Window(IChromelyNativeHost nativeHost,
                      IChromelyConfiguration config,
                      ChromelyHandlersResolver handlersResolver)
            : base(nativeHost, config, handlersResolver)
        {
            Created += OnBrowserCreated;
        }

        public IntPtr Handle 
        {
            get
            {
                if (NativeHost is not null)
                {
                    return NativeHost.Handle;
                }

                return IntPtr.Zero;
            }
        }

        public virtual void Init(object settings)
        {
        }
        public virtual void Create(IntPtr hostHandle, IntPtr winXID)
        {
            CreateBrowser(hostHandle, winXID);
        }

        public virtual void SetTitle(string title)
        {
            NativeHost?.SetWindowTitle(title);
        }

        public virtual void NotifyOnMove()
        {
            NotifyMoveOrResize();
        }

        public virtual void Resize(int width, int height)
        {
            ResizeBrowser(width,  height);
        }

        public virtual void Minimize()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_config.WindowOptions.WindowFrameless)
                {
                    _config.WindowOptions.WindowState = WindowState.Minimize;
                }

                ShowWindow(Handle, SW.SHOWMINIMIZED);
            }
        }
        public virtual void Maximize()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_config.WindowOptions.WindowFrameless)
                {
                    _config.WindowOptions.WindowState = WindowState.Maximize;
                }
               
                ShowWindow(Handle, SW.SHOWMAXIMIZED);
            }
        }
        public virtual void Restore()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_config.WindowOptions.WindowFrameless)
                {
                    _config.WindowOptions.WindowState = WindowState.Normal;
                }
                
                ShowWindow(Handle, SW.RESTORE);
            }
        }

        public virtual void Close()
        {
            if (Handle != IntPtr.Zero)
            {
                SendMessageW(Handle, WM.CLOSE);
            }
        }
    }
}
