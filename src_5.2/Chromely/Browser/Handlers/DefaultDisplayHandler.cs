// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultDisplayHandler : CefDisplayHandler
    {
        protected readonly IChromelyConfiguration _config;
        protected ChromiumBrowser? _browser;

        public DefaultDisplayHandler(IChromelyConfiguration config, IChromelyWindow window)
        {
            _config = config;
            _browser = window as ChromiumBrowser;
        }

        public ChromiumBrowser? Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        protected override void OnTitleChange(CefBrowser browser, string title)
        {
            if (_browser is not null)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnTitleChanged(new TitleChangedEventArgs(title)));
            }
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain && _browser is not null)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
            }
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            if (_browser is not null)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnStatusMessage(new StatusMessageEventArgs(value)));
            }
        }

        protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
        {
            var evntArgs = new ConsoleMessageEventArgs(message, source, line);
            if (_browser is not null)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnConsoleMessage(evntArgs));
            }

            return evntArgs.Handled;
        }

        protected override bool OnTooltip(CefBrowser browser, string text)
        {
            var evntArgs = new TooltipEventArgs(text);
            if (_browser is not null)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnTooltip(evntArgs));
            }

            return evntArgs.Handled;
        }
    }
}
