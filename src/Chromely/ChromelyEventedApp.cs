using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Helpers;

namespace Chromely
{
    /// <summary>
    /// Chromely app implementation handler for some events
    /// to be overwritten by.
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class ChromelyEventedApp : ChromelyBasicApp
    {
        /// <summary>
        /// Registers the events
        /// FrameLoadStart, FrameLoadEnd, ConsoleMessage, StatusMessage and BeforeClose
        /// to call overridable methods.
        /// </summary>
        /// <param name="container"></param>
        public override void RegisterEvents(IChromelyContainer container)
        {
            EnsureContainerValid(container);

            RegisterEventHandler(container, CefEventKey.FrameLoadStart, new ChromelyEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadStart, OnFrameLoadStart));
            RegisterEventHandler(container, CefEventKey.FrameLoadEnd, new ChromelyEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnFrameLoadEnd));
            RegisterEventHandler(container, CefEventKey.ConsoleMessage, new ChromelyEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnConsoleMessage));
            RegisterEventHandler(container, CefEventKey.StatusMessage, new ChromelyEventHandler<StatusMessageEventArgs>(CefEventKey.StatusMessage, OnStatusMessage));
            RegisterEventHandler(container, CefEventKey.BeforeClose, new ChromelyEventHandler<BeforeCloseEventArgs>(CefEventKey.BeforeClose, OnBeforeClose));
        }

        /// <summary>
        /// Override to handle FrameLoadStart event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void OnFrameLoadStart(object sender, FrameLoadStartEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Override to handle FrameLoadEnd event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Override to handle ConsoleMessage event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void OnConsoleMessage(object sender, ConsoleMessageEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Override to handle StatusMessage event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void OnStatusMessage(object sender, StatusMessageEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Override to handle BeforeClose event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void OnBeforeClose(object sender, BeforeCloseEventArgs eventArgs)
        {
        }

        private static void RegisterEventHandler<T>(IChromelyContainer container, CefEventKey key, ChromelyEventHandler<T> handler)
        {
            var service = CefEventHandlerTypes.GetHandlerType(key);
            container.RegisterInstance(service, handler.Key, handler);
        }
    }
}
