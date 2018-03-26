// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueClient.cs" company="Chromely">
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

namespace Chromely.CefGlue.Gtk.Browser
{
    using Chromely.CefGlue.Gtk.ChromeHost;
    using Xilium.CefGlue;

    /// <summary>
    /// The cef glue client.
    /// </summary>
    public class CefGlueClient : CefClient
    {
        /// <summary>
        /// The m life span handler.
        /// </summary>
        private readonly CefLifeSpanHandler mLifeSpanHandler;

        /// <summary>
        /// The m load handler.
        /// </summary>
        private readonly CefLoadHandler mLoadHandler;

        /// <summary>
        /// The m request handler.
        /// </summary>
        private readonly CefRequestHandler mRequestHandler;

        /// <summary>
        /// The m display handler.
        /// </summary>
        private readonly CefDisplayHandler mDisplayHandler;

        /// <summary>
        /// The m context menu handler.
        /// </summary>
        private readonly CefContextMenuHandler mContextMenuHandler;

        /// <summary>
        /// The m focus handler.
        /// </summary>
        private readonly CefFocusHandler mFocusHandler;

        /// <summary>
        /// The m keyboard handler.
        /// </summary>
        private readonly CefKeyboardHandler mKeyboardHandler;

        /// <summary>
        /// The m js dialog handler.
        /// </summary>
        private readonly CefJSDialogHandler mJsDialogHandler;

        /// <summary>
        /// The m dialog handler.
        /// </summary>
        private readonly CefDialogHandler mDialogHandler;

        /// <summary>
        /// The m drag handler.
        /// </summary>
        private readonly CefDragHandler mDragHandler;

        /// <summary>
        /// The m geolocation handler.
        /// </summary>
        private readonly CefGeolocationHandler mGeolocationHandler;

        /// <summary>
        /// The m download handler.
        /// </summary>
        private readonly CefDownloadHandler mDownloadHandler;

        /// <summary>
        /// The m find handler.
        /// </summary>
        private readonly CefFindHandler mFindHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueClient"/> class.
        /// </summary>
        /// <param name="clientParams">
        /// The client params.
        /// </param>
        public CefGlueClient(CefGlueClientParams clientParams)
        {
            this.CoreBrowser = clientParams.Browser;
            this.mLifeSpanHandler = clientParams.LifeSpanHandler;
            this.mLoadHandler = clientParams.LoadHandler;
            this.mRequestHandler = clientParams.RequestHandler;
            this.mDisplayHandler = clientParams.DisplayHandler;
            this.mContextMenuHandler = clientParams.ContextMenuHandler;
            this.mFocusHandler = clientParams.FocusHandler;
            this.mKeyboardHandler = clientParams.KeyboardHandler;
            this.mJsDialogHandler = clientParams.JsDialogHandler;
            this.mDialogHandler = clientParams.DialogHandler;
            this.mDragHandler = clientParams.DragHandler;
            this.mGeolocationHandler = clientParams.GeolocationHandler;
            this.mDownloadHandler = clientParams.DownloadHandler;
            this.mFindHandler = clientParams.FindHandler;
        }

        /// <summary>
        /// Gets the core browser.
        /// </summary>
        public CefGlueBrowser CoreBrowser { get; }

        /// <summary>
        /// The get life span handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefLifeSpanHandler"/>.
        /// </returns>
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return this.mLifeSpanHandler;
        }

        /// <summary>
        /// The get load handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefLoadHandler"/>.
        /// </returns>
        protected override CefLoadHandler GetLoadHandler()
        {
            return this.mLoadHandler;
        }

        /// <summary>
        /// The get request handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRequestHandler"/>.
        /// </returns>
        protected override CefRequestHandler GetRequestHandler()
        {
            return this.mRequestHandler;
        }

        /// <summary>
        /// The get display handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDisplayHandler"/>.
        /// </returns>
        protected override CefDisplayHandler GetDisplayHandler()
        {
            return this.mDisplayHandler;
        }

        /// <summary>
        /// The get context menu handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefContextMenuHandler"/>.
        /// </returns>
        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return this.mContextMenuHandler;
        }

        /// <summary>
        /// The get focus handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFocusHandler"/>.
        /// </returns>
        protected override CefFocusHandler GetFocusHandler()
        {
            return this.mFocusHandler;
        }

        /// <summary>
        /// The get keyboard handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefKeyboardHandler"/>.
        /// </returns>
        protected override CefKeyboardHandler GetKeyboardHandler()
        {
            return this.mKeyboardHandler;
        }

        /// <summary>
        /// The get js dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefJSDialogHandler"/>.
        /// </returns>
        protected override CefJSDialogHandler GetJSDialogHandler()
        {
            return this.mJsDialogHandler;
        }

        /// <summary>
        /// The get dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDialogHandler"/>.
        /// </returns>
        protected override CefDialogHandler GetDialogHandler()
        {
            return this.mDialogHandler;
        }

        /// <summary>
        /// The get drag handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDragHandler"/>.
        /// </returns>
        protected override CefDragHandler GetDragHandler()
        {
            return this.mDragHandler;
        }

        /// <summary>
        /// The get geolocation handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefGeolocationHandler"/>.
        /// </returns>
        protected override CefGeolocationHandler GetGeolocationHandler()
        {
            return this.mGeolocationHandler;
        }

        /// <summary>
        /// The get download handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDownloadHandler"/>.
        /// </returns>
        protected override CefDownloadHandler GetDownloadHandler()
        {
            return this.mDownloadHandler;
        }

        /// <summary>
        /// The get find handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFindHandler"/>.
        /// </returns>
        protected override CefFindHandler GetFindHandler()
        {
            return this.mFindHandler;
        }

        /// <summary>
        /// The on process message received.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="sourceProcess">
        /// The source process.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            return HostBase.BrowserMessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        }
    }
}
