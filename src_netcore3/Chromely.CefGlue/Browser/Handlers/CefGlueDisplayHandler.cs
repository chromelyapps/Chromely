using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Host;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueDisplayHandler : CefDisplayHandler
    {
        private readonly IChromelyConfiguration _config;

        private readonly CefGlueBrowser _browser;

        public CefGlueDisplayHandler(IChromelyConfiguration config, CefGlueBrowser browser)
        {
            _config = config;
            _browser = browser;
        }

        protected override void OnTitleChange(CefBrowser browser, string title)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnTitleChanged(new TitleChangedEventArgs(title)));
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
            }
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnStatusMessage(new StatusMessageEventArgs(value)));
        }

        protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
        {
            var evntArgs = new ConsoleMessageEventArgs(message, source, line);
            _browser.InvokeAsyncIfPossible(() => _browser.OnConsoleMessage(evntArgs));
            return evntArgs.Handled;
        }

        protected override bool OnTooltip(CefBrowser browser, string text)
        {
            var evntArgs = new TooltipEventArgs(text);
            _browser.InvokeAsyncIfPossible(() => _browser.OnTooltip(evntArgs));
            return evntArgs.Handled;
        }
    }
}
