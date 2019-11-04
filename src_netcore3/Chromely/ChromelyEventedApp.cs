using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Helpers;

namespace Chromely
{
    public abstract class ChromelyEventedApp : BasicChromelyApp
    {
        public override void RegisterEvents(IChromelyContainer container)
        {
            EnsureContainerValid(container);

            RegisterEventHandler(container, CefEventKey.FrameLoadStart, new ChromelyEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadEnd, OnFrameLoadStart));
            RegisterEventHandler(container, CefEventKey.FrameLoadEnd, new ChromelyEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnFrameLoadEnd));
            RegisterEventHandler(container, CefEventKey.ConsoleMessage, new ChromelyEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnConsoleMessage));
            RegisterEventHandler(container, CefEventKey.StatusMessage, new ChromelyEventHandler<StatusMessageEventArgs>(CefEventKey.FrameLoadEnd, OnStatusMessage));
            RegisterEventHandler(container, CefEventKey.BeforeClose, new ChromelyEventHandler<BeforeCloseEventArgs>(CefEventKey.FrameLoadEnd, OnBeforeClose));
        }

        public virtual void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
        }

        public virtual void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }

        public virtual void OnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        public virtual void OnStatusMessage(object sender, StatusMessageEventArgs e)
        {
        }

        public virtual void OnBeforeClose(object sender, BeforeCloseEventArgs e)
        {
        }

        private void RegisterEventHandler<T>(IChromelyContainer container, CefEventKey key, ChromelyEventHandler<T> handler)
        {
            var service = CefEventHandlerTypes.GetHandlerType(key);
            container.RegisterInstance(service, handler.Key, handler);
        }
    }
}
