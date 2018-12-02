// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueRenderProcessHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    /// <summary>
    /// The CefGlue render process handler.
    /// </summary>
    public class CefGlueRenderProcessHandler : CefRenderProcessHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueRenderProcessHandler"/> class.
        /// </summary>
        public CefGlueRenderProcessHandler()
        {
            this.MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
        }

        /// <summary>
        /// Gets the message router.
        /// </summary>
        internal CefMessageRouterRendererSide MessageRouter { get; }

        /// <summary>
        /// The on context created.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            this.MessageRouter.OnContextCreated(browser, frame, context);

            // MessageRouter.OnContextCreated doesn't capture CefV8Context immediately,
            // so we able to release it immediately in this call.
            context.Dispose();
        }

        /// <summary>
        /// The on context released.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            // MessageRouter.OnContextReleased releases captured CefV8Context (if have).
            this.MessageRouter.OnContextReleased(browser, frame, context);

            // Release CefV8Context.
            context.Dispose();
        }

        /// <summary>
        /// The on process message received.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="sourceProcess">
        /// The source process.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            var handled = this.MessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
            if (handled)
            {
                return true;
            }

            return false;
        }
    }
}
