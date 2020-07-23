// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.Browser
{
    /// <summary>
    /// Default CEF render process handler.
    /// </summary>
    public class DefaultRenderProcessHandler : CefRenderProcessHandler
    {
        protected readonly IChromelyConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRenderProcessHandler"/> class.
        /// </summary>
        public DefaultRenderProcessHandler(IChromelyConfiguration config)
        {
            _config = config;
            MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
        }

        /// <summary>
        /// Gets the message router.
        /// </summary>
        public CefMessageRouterRendererSide MessageRouter { get; }

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
            MessageRouter.OnContextCreated(browser, frame, context);

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
            MessageRouter.OnContextReleased(browser, frame, context);

            // Release CefV8Context.
            context.Dispose();
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
        {
            var handled = MessageRouter.OnProcessMessageReceived(browser, frame, sourceProcess, message);
            if (handled)
            {
                return true;
            }

            return false;
        }
    }
}
