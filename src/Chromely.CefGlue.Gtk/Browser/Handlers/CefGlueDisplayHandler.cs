// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueDisplayHandler.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using Chromely.CefGlue.Gtk.Browser.EventParams;
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
            this.mBrowser = CefGlueBrowser.BrowserCore;
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
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnTitleChanged(new TitleChangedEventArgs(title)));
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
                this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
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
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnStatusMessage(new StatusMessageEventArgs(value)));
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
            var e = new ConsoleMessageEventArgs(message, source, line);
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnConsoleMessage(e));

            return e.Handled;
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
            var e = new TooltipEventArgs(text);
            this.mBrowser.InvokeAsyncIfPossible(() => this.mBrowser.OnTooltip(e));
            return e.Handled;
        }
    }
}
