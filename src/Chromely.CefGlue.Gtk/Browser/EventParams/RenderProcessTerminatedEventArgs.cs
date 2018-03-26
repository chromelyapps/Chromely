// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderProcessTerminatedEventArgs.cs" company="Chromely">
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

namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The render process terminated event args.
    /// </summary>
    public class RenderProcessTerminatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderProcessTerminatedEventArgs"/> class.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        public RenderProcessTerminatedEventArgs(CefTerminationStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public CefTerminationStatus Status { get; }
    }
}
