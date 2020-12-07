// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Linq;
using Chromely.Core;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely
{
    public partial class Window
    {
        public void RegisterHandlers()
        {
            MessageRouterHandlers();
        }

        protected virtual void MessageRouterHandlers()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                ActionTask.PostTask(CefThreadId.UI, MessageRouterHandlers);
                return;
            }

            _browserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());

            // Register message router handlers
            var messageRouterHandlers = _handlersResolver?.Invoke(typeof(IChromelyMessageRouter))?.ToList();
            if (messageRouterHandlers != null && messageRouterHandlers.Any())
            {
                foreach (var handler in messageRouterHandlers)
                {
                    var router = handler as CefMessageRouterBrowserSide.Handler;
                    if (router != null)
                    {
                        _browserMessageRouter.AddHandler(router);
                    }
                }
            }
        }

        internal void OnBrowserCreated(object sender, EventArgs e)
        {
            _browserWindowHandle = Browser.GetHost().GetWindowHandle();
            if (_browserWindowHandle != IntPtr.Zero)
            {
                var size = NativeHost.GetWindowClientSize();
                ResizeBrowser(size.Width, size.Height);

                NativeHost.SetupMessageInterceptor(_browserWindowHandle);
            }
        }

        internal void ResizeBrowser()
        {
            if (Browser != null)
            {
                _browserWindowHandle = Browser.GetHost().GetWindowHandle();
                if (_browserWindowHandle != IntPtr.Zero)
                {
                    var size = NativeHost.GetWindowClientSize();
                    ResizeBrowser(size.Width, size.Height);
                }
            }
        }

        internal void ResizeBrowser(int width, int height)
        {
            NativeHost?.ResizeBrowser(_browserWindowHandle, width, height);
        }
    }
}
