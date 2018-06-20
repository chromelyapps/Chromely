// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyMessageRouter.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;

    /// <summary>
    /// The chromely messsage router.
    /// Registers MessageRouter handler -
    /// https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage.md#markdown-header-generic-message-router
    /// </summary>
    public class ChromelyMessageRouter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyMessageRouter"/> class.
        /// </summary>
        public ChromelyMessageRouter()
        {
            this.Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyMessageRouter"/> class.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public ChromelyMessageRouter(object handler)
        {
            this.Key = Guid.NewGuid().ToString();
            this.Handler = handler;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        public object Handler { get; set; }
    }
}
