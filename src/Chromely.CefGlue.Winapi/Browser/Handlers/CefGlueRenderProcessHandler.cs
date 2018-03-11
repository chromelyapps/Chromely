#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    public class CefGlueRenderProcessHandler : CefRenderProcessHandler
    {
        public CefGlueRenderProcessHandler()
        {
            MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
        }

        internal CefMessageRouterRendererSide MessageRouter { get; private set; }

        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            MessageRouter.OnContextCreated(browser, frame, context);

            // MessageRouter.OnContextCreated doesn't capture CefV8Context immediately,
            // so we able to release it immediately in this call.
            context.Dispose();
        }

        protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            // MessageRouter.OnContextReleased releases captured CefV8Context (if have).
            MessageRouter.OnContextReleased(browser, frame, context);

            // Release CefV8Context.
            context.Dispose();
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            var handled = MessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
            if (handled) return true;

            return false;
        }
    }
}
