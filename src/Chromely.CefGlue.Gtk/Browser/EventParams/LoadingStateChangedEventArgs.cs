// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadingStateChangedEventArgs.cs" company="Chromely">
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

    /// <summary>
    /// The loading state changed event args.
    /// </summary>
    public sealed class LoadingStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="isLoading">
        /// The is loading.
        /// </param>
        /// <param name="canGoBack">
        /// The can go back.
        /// </param>
        /// <param name="canGoForward">
        /// The can go forward.
        /// </param>
        public LoadingStateChangedEventArgs(bool isLoading, bool canGoBack, bool canGoForward)
        {
            this.Loading = isLoading;
            this.CanGoBack = canGoBack;
            this.CanGoForward = canGoForward;
        }

        /// <summary>
        /// Gets a value indicating whether loading.
        /// </summary>
        public bool Loading { get; }

        /// <summary>
        /// Gets a value indicating whether can go back.
        /// </summary>
        public bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether can go forward.
        /// </summary>
        public bool CanGoForward { get; }
    }
}
