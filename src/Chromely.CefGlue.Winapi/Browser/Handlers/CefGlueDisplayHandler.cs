// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDisplayHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using Chromely.CefGlue.Winapi.Browser.EventParams;
    using Chromely.Core.Host;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue display handler.
    /// </summary>
    public class CefGlueDisplayHandler : CefDisplayHandler
    {
        /// <summary>
        /// The m_browser.
        /// </summary>
        private readonly CefGlueBrowser mBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueDisplayHandler"/> class.
        /// </summary>
        public CefGlueDisplayHandler()
        {
            mBrowser = CefGlueBrowser.BrowserCore;
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnConsoleMessage(evntArgs));
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
            mBrowser.InvokeAsyncIfPossible(() => mBrowser.OnTooltip(evntArgs));
            return evntArgs.Handled;
        }
    }
}
