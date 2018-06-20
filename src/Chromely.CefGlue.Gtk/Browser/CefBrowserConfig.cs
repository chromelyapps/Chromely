// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefBrowserConfig.cs" company="Chromely">
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

namespace Chromely.CefGlue.Gtk.Browser
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The cef browser config.
    /// </summary>
    public class CefBrowserConfig
    {
        /// <summary>
        /// Gets or sets the parent handle.
        /// </summary>
        public IntPtr ParentHandle { get; set; }

        /// <summary>
        /// Gets or sets the cef rectangle.
        /// </summary>
        public CefRectangle CefRectangle { get; set; }

        /// <summary>
        /// Gets or sets the app args.
        /// </summary>
        public string[] AppArgs { get; set; }

        /// <summary>
        /// Gets or sets the start url.
        /// </summary>
        public string StartUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether start web socket.
        /// </summary>
        public bool StartWebSocket { get; set; }

        /// <summary>
        /// Gets or sets the websocket address.
        /// </summary>
        public string WebsocketAddress { get; set; }

        /// <summary>
        /// Gets or sets the websocket port.
        /// </summary>
        public int WebsocketPort { get; set; }
    }
}
