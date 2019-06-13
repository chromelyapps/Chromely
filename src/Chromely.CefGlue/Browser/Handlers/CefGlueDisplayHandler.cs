// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDisplayHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core.Host;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue display handler.
    /// </summary>
    public class CefGlueDisplayHandler : CefDisplayHandler
    {
        /// <summary>
        /// The m_browser.
        /// </summary>
        private readonly CefGlueBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueDisplayHandler"/> class.
        /// </summary>
        public CefGlueDisplayHandler()
        {
            _browser = CefGlueBrowser.BrowserCore;
        }

        /// <summary>
        /// The on title change.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        protected override void OnTitleChange(CefBrowser browser, string title)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnTitleChanged(new TitleChangedEventArgs(title)));
        }

        /// <summary>
        /// The on address change.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain)
            {
                _browser.InvokeAsyncIfPossible(() => _browser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
            }
        }

        /// <summary>
        /// The on status message.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnStatusMessage(new StatusMessageEventArgs(value)));
        }

        /// <summary>
        /// The on console message.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
        {
            var evntArgs = new ConsoleMessageEventArgs(message, source, line);
            _browser.InvokeAsyncIfPossible(() => _browser.OnConsoleMessage(evntArgs));
            return evntArgs.Handled;
        }

        /// <summary>
        /// The on tooltip.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnTooltip(CefBrowser browser, string text)
        {
            var evntArgs = new TooltipEventArgs(text);
            _browser.InvokeAsyncIfPossible(() => _browser.OnTooltip(evntArgs));
            return evntArgs.Handled;
        }
    }
}
