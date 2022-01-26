// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

public partial class Window
{
    /// <summary>
    /// Register non scheme related handlers like the message route handlers.
    /// </summary>
    public void RegisterHandlers()
    {
        MessageRouterHandlers();
    }

    /// <summary>
    /// Create and add message router handlers.
    /// </summary>
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
        if (messageRouterHandlers is not null && messageRouterHandlers.Any())
        {
            foreach (var handler in messageRouterHandlers)
            {
                var router = handler as CefMessageRouterBrowserSide.Handler;
                if (router is not null)
                {
                    _browserMessageRouter.AddHandler(router);
                }
            }
        }
    }

    internal void OnBrowserCreated(object sender, EventArgs e)
    {
        if (Browser is not null)
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