#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    public class CefGlueDisplayHandler : CefDisplayHandler
    {
        private readonly CefGlueBrowser m_browser;

        public CefGlueDisplayHandler()
        {
            m_browser = CefGlueBrowser.BrowserCore;
        }

        protected override void  OnTitleChange(CefBrowser browser, string title)
        {
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnTitleChanged(new TitleChangedEventArgs(title)));
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain)
            {
                m_browser.InvokeAsyncIfPossible(() => m_browser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
            }
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnStatusMessage(new StatusMessageEventArgs(value)));
        }

        protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
		{
			var e = new ConsoleMessageEventArgs(message, source, line);
            m_browser.InvokeAsyncIfPossible(() => m_browser.OnConsoleMessage(e));

			return e.Handled;
		}

		protected override bool OnTooltip(CefBrowser browser, string text)
		{
			var e = new TooltipEventArgs(text);
            m_browser.InvokeAsyncIfPossible(()=> m_browser.OnTooltip(e));
			return e.Handled;
		}
    }
}
