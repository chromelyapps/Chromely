// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforePopupEventArgs.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The before popup event args.
    /// </summary>
    public class BeforePopupEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforePopupEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="targetUrl">
        /// The target url.
        /// </param>
        /// <param name="targetFrameName">
        /// The target frame name.
        /// </param>
        /// <param name="popupFeatures">
        /// The popup features.
        /// </param>
        /// <param name="windowInfo">
        /// The window info.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="noJavascriptAccess">
        /// The no javascript access.
        /// </param>
        public BeforePopupEventArgs(
            CefFrame frame,
            string targetUrl,
            string targetFrameName,
            CefPopupFeatures popupFeatures,
            CefWindowInfo windowInfo,
            CefClient client,
            CefBrowserSettings settings,
            bool noJavascriptAccess)
        {
                this.Frame = frame;
                this.TargetUrl = targetUrl;
                this.TargetFrameName = targetFrameName;
                this.PopupFeatures = popupFeatures;
                this.WindowInfo = windowInfo;
                this.Client = client;
                this.Settings = settings;
                this.NoJavascriptAccess = noJavascriptAccess;
        }

        /// <summary>
        /// Gets or sets a value indicating whether no javascript access.
        /// </summary>
        public bool NoJavascriptAccess { get; set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public CefBrowserSettings Settings { get; private set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public CefClient Client { get; set; }

        /// <summary>
        /// Gets the window info.
        /// </summary>
        public CefWindowInfo WindowInfo { get; private set; }

        /// <summary>
        /// Gets the popup features.
        /// </summary>
        public CefPopupFeatures PopupFeatures { get; private set; }

        /// <summary>
        /// Gets the target frame name.
        /// </summary>
        public string TargetFrameName { get; private set; }

        /// <summary>
        /// Gets the target url.
        /// </summary>
        public string TargetUrl { get; private set; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
