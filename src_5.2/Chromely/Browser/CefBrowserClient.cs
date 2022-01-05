// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable

namespace Chromely.Browser;

/// <summary>
/// The CEF brwoser client.
/// </summary>
public partial class CefBrowserClient : CefClient
{
    private readonly CefMessageRouterBrowserSide _browserMessageRouter;

    /// <summary>
    /// The life span handler.
    /// </summary>
    private CefLifeSpanHandler _lifeSpanHandler;

    /// <summary>
    /// The load handler.
    /// </summary>
    private CefLoadHandler _loadHandler;

    /// <summary>
    /// The request handler.
    /// </summary>
    private CefRequestHandler _requestHandler;

    /// <summary>
    /// The display handler.
    /// </summary>
    private CefDisplayHandler _displayHandler;

    /// <summary>
    /// The context menu handler.
    /// </summary>
    private CefContextMenuHandler _contextMenuHandler;

    /// <summary>
    /// The focus handler.
    /// </summary>
    private CefFocusHandler _focusHandler;

    /// <summary>
    /// The keyboard handler.
    /// </summary>
    private CefKeyboardHandler _keyboardHandler;

    /// <summary>
    /// The Javascript dialog handler.
    /// </summary>
    private CefJSDialogHandler _jsDialogHandler;

    /// <summary>
    /// The dialog handler.
    /// </summary>
    private CefDialogHandler _dialogHandler;

    /// <summary>
    /// The drag handler.
    /// </summary>
    private CefDragHandler _dragHandler;

    /// <summary>
    /// The download handler.
    /// </summary>
    private CefDownloadHandler _downloadHandler;

    /// <summary>
    /// The find handler.
    /// </summary>
    private CefFindHandler _findHandler;

    /// <summary>Initializes a new instance of the <see cref="CefBrowserClient" /> class.</summary>
    public CefBrowserClient(CefMessageRouterBrowserSide browserMessageRouter, ChromelyHandlersResolver handlersResolver)
    {
        _browserMessageRouter = browserMessageRouter;
        SetHandlers(handlersResolver);
    }

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

    /// <summary>The on process message received.</summary>
    /// <param name="browser">The browser.</param>
    /// <param name="frame"></param>
    /// <param name="sourceProcess">The source process.</param>
    /// <param name="message">The message.</param>
    /// <returns>The <see cref="bool" />.</returns>
    protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        return _browserMessageRouter?.OnProcessMessageReceived(browser, frame, sourceProcess, message) ?? false;
    }
}