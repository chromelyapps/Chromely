#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Core.CefGlueBrowser
{
    using Xilium.CefGlue;

    internal sealed class CefWebDisplayHandler : CefDisplayHandler
    {
        private readonly CefWebBrowser m_core;

        public CefWebDisplayHandler(CefWebBrowser core)
        {
            m_core = core;
        }

        protected override void  OnTitleChange(CefBrowser browser, string title)
        {
            m_core.InvokeAsyncIfPossible(() => m_core.OnTitleChanged(new TitleChangedEventArgs(title)));
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain)
            {
               m_core.InvokeAsyncIfPossible(() => m_core.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
            }
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            m_core.InvokeAsyncIfPossible(() => m_core.OnStatusMessage(new StatusMessageEventArgs(value)));
        }

		protected override bool OnConsoleMessage(CefBrowser browser, string message, string source, int line)
		{
			var e = new ConsoleMessageEventArgs(message, source, line);
			m_core.InvokeAsyncIfPossible(() => m_core.OnConsoleMessage(e));

			return e.Handled;
		}

		protected override bool OnTooltip(CefBrowser browser, string text)
		{
			var e = new TooltipEventArgs(text);
			m_core.InvokeAsyncIfPossible(()=> m_core.OnTooltip(e));
			return e.Handled;
		}
    }
}
