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

namespace Chromely.CefSharp.Winapi
{
    using global::CefSharp;
    using Chromely.CefSharp.Winapi.Browser.FrameHandlers;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static IBrowser browser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        private static IBrowser Browser
        {
            get
            {
                if (browser != null)
                {
                    return browser;
                }

                CefSharpFrameHandler cefSharpFrameHandler = IoC.GetInstance<CefSharpFrameHandler>(typeof(CefSharpFrameHandler).FullName);
                if (cefSharpFrameHandler != null)
                {
                    browser = cefSharpFrameHandler.Browser;
                }

                return browser;
            }
        }

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetMainFrame()
        {
            return Browser?.MainFrame;
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetFrame(string name)
        {
            return Browser?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="IBrowser"/>.
        /// </returns>
        public static IBrowser GetBrowser()
        {
            return Browser;
        }
    }
}
