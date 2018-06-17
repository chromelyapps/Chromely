// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameHandler.cs" company="Chromely">
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
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk
{
    using Chromely.CefGlue.Gtk.Browser.FrameHandlers;
    using Chromely.Core.Infrastructure;

    using Xilium.CefGlue;

    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static CefBrowser browser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        private static CefBrowser Browser
        {
            get
            {
                if (browser != null)
                {
                    return browser;
                }

                CefGlueFrameHandler cefGlueFrameHandler = IoC.GetInstance<CefGlueFrameHandler>(typeof(CefGlueFrameHandler).FullName);
                if (cefGlueFrameHandler != null)
                {
                    browser = cefGlueFrameHandler.Browser;
                }

                return browser;
            }
        }

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetMainFrame()
        {
            return Browser?.GetMainFrame();
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetFrame(string name)
        {
            return Browser?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="CefBrowser"/>.
        /// </returns>
        public static CefBrowser GetBrowser()
        {
            return Browser;
        }
    }
}
