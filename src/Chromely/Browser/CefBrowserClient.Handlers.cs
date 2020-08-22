// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    internal partial class CefBrowserClient 
    {
        private void SetHandlers(ChromelyHandlersResolver handlersResolver)
        {
            if (handlersResolver == null)
            {
                return;
            }

            // LifeSpanHandler
            var handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefLifeSpanHandler));
            if (handler is CefLifeSpanHandler lifeSpanHandler)
            {
                _lifeSpanHandler = lifeSpanHandler;
            }

            // LoadHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefLoadHandler));
            if (handler is CefLoadHandler loadHandler)
            {
                _loadHandler = loadHandler;
            }

            // RequestHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefRequestHandler));
            if (handler is CefRequestHandler requestHandler)
            {
                _requestHandler = requestHandler;
            }

            // DisplayHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefDisplayHandler));
            if (handler is CefDisplayHandler displayHandler)
            {
                _displayHandler = displayHandler;
            }

            // ContextMenuHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefContextMenuHandler));
            if (handler is CefContextMenuHandler contextMenuHandler)
            {
                _contextMenuHandler = contextMenuHandler;
            }

            // FocusHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefFocusHandler));
            if (handler is CefFocusHandler focusHandler)
            {
                _focusHandler = focusHandler;
            }

            // KeyboardHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefKeyboardHandler));
            if (handler is CefKeyboardHandler keyboardHandler)
            {
                _keyboardHandler = keyboardHandler;
            }

            // JsDialogHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefJSDialogHandler));
            if (handler is CefJSDialogHandler jsDialogHandler)
            {
                _jsDialogHandler = jsDialogHandler;
            }

            // DialogHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefDialogHandler));
            if (handler is CefDialogHandler dialogHandler)
            {
                _dialogHandler = dialogHandler;
            }

            // DragHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefDragHandler));
            if (handler is CefDragHandler dragHandler)
            {
                _dragHandler = dragHandler;
            }

            // DownloadHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefDownloadHandler));
            if (handler is CefDownloadHandler downloadHandler)
            {
                _downloadHandler = downloadHandler;
            }

            // FindHandler
            handler = handlersResolver.GetCustomOrDefaultHandler(typeof(CefFindHandler));
            if (handler is CefFindHandler findHandler)
            {
                _findHandler = findHandler;
            }
        }
    }
}
