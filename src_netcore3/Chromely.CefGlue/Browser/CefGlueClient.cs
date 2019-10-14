// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueClient.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.Browser
{
    /// <summary>
    /// The CefGlue client.
    /// </summary>
    public class CefGlueClient : CefClient
    {
        /// <summary>
        /// The life span handler.
        /// </summary>
        private readonly CefLifeSpanHandler _lifeSpanHandler;

        /// <summary>
        /// The load handler.
        /// </summary>
        private readonly CefLoadHandler _loadHandler;

        /// <summary>
        /// The request handler.
        /// </summary>
        private readonly CefRequestHandler _requestHandler;

        /// <summary>
        /// The display handler.
        /// </summary>
        private readonly CefDisplayHandler _displayHandler;

        /// <summary>
        /// The context menu handler.
        /// </summary>
        private readonly CefContextMenuHandler _contextMenuHandler;

        /// <summary>
        /// The focus handler.
        /// </summary>
        private readonly CefFocusHandler _focusHandler;

        /// <summary>
        /// The keyboard handler.
        /// </summary>
        private readonly CefKeyboardHandler _keyboardHandler;

        /// <summary>
        /// The Javascript dialog handler.
        /// </summary>
        private readonly CefJSDialogHandler _jsDialogHandler;

        /// <summary>
        /// The dialog handler.
        /// </summary>
        private readonly CefDialogHandler _dialogHandler;

        /// <summary>
        /// The drag handler.
        /// </summary>
        private readonly CefDragHandler _dragHandler;

        /// <summary>
        /// The download handler.
        /// </summary>
        private readonly CefDownloadHandler _downloadHandler;

        /// <summary>
        /// The find handler.
        /// </summary>
        private readonly CefFindHandler _findHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueClient"/> class.
        /// </summary>
        /// <param name="clientParams">
        /// The client params.
        /// </param>
        public CefGlueClient(CefGlueClientParams clientParams)
        {
            CoreBrowser = clientParams.Browser;
            _lifeSpanHandler = clientParams.LifeSpanHandler;
            _loadHandler = clientParams.LoadHandler;
            _requestHandler = clientParams.RequestHandler;
            _displayHandler = clientParams.DisplayHandler;
            _contextMenuHandler = clientParams.ContextMenuHandler;
            _focusHandler = clientParams.FocusHandler;
            _keyboardHandler = clientParams.KeyboardHandler;
            _jsDialogHandler = clientParams.JsDialogHandler;
            _dialogHandler = clientParams.DialogHandler;
            _dragHandler = clientParams.DragHandler;
            _downloadHandler = clientParams.DownloadHandler;
            _findHandler = clientParams.FindHandler;
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
            return _lifeSpanHandler;
        }

        /// <summary>
        /// The get load handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefLoadHandler"/>.
        /// </returns>
        protected override CefLoadHandler GetLoadHandler()
        {
            return _loadHandler;
        }

        /// <summary>
        /// The get request handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRequestHandler"/>.
        /// </returns>
        protected override CefRequestHandler GetRequestHandler()
        {
            return _requestHandler;
        }

        /// <summary>
        /// The get display handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDisplayHandler"/>.
        /// </returns>
        protected override CefDisplayHandler GetDisplayHandler()
        {
            return _displayHandler;
        }

        /// <summary>
        /// The get context menu handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefContextMenuHandler"/>.
        /// </returns>
        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return _contextMenuHandler;
        }

        /// <summary>
        /// The get focus handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFocusHandler"/>.
        /// </returns>
        protected override CefFocusHandler GetFocusHandler()
        {
            return _focusHandler;
        }

        /// <summary>
        /// The get keyboard handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefKeyboardHandler"/>.
        /// </returns>
        protected override CefKeyboardHandler GetKeyboardHandler()
        {
            return _keyboardHandler;
        }

        /// <summary>
        /// The get js dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefJSDialogHandler"/>.
        /// </returns>
        protected override CefJSDialogHandler GetJSDialogHandler()
        {
            return _jsDialogHandler;
        }

        /// <summary>
        /// The get dialog handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDialogHandler"/>.
        /// </returns>
        protected override CefDialogHandler GetDialogHandler()
        {
            return _dialogHandler;
        }

        /// <summary>
        /// The get drag handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDragHandler"/>.
        /// </returns>
        protected override CefDragHandler GetDragHandler()
        {
            return _dragHandler;
        }

        /// <summary>
        /// The get download handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefDownloadHandler"/>.
        /// </returns>
        protected override CefDownloadHandler GetDownloadHandler()
        {
            return _downloadHandler;
        }

        /// <summary>
        /// The get find handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFindHandler"/>.
        /// </returns>
        protected override CefFindHandler GetFindHandler()
        {
            return _findHandler;
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
            var browserMessageRouter = IoC.GetInstance<CefMessageRouterBrowserSide>(typeof(CefMessageRouterBrowserSide).FullName);
            return browserMessageRouter?.OnProcessMessageReceived(browser, sourceProcess, message) ?? false;
        }
    }
}
