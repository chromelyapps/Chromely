// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueClientParams.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using Chromely.CefGlue.Winapi.Browser.Handlers;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue client params.
    /// </summary>
    public class CefGlueClientParams
    {
        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        public CefGlueBrowser Browser { get; set; }

        /// <summary>
        /// Gets or sets the life span handler.
        /// </summary>
        public CefLifeSpanHandler LifeSpanHandler { get; set; }

        /// <summary>
        /// Gets or sets the load handler.
        /// </summary>
        public CefLoadHandler LoadHandler { get; set; }

        /// <summary>
        /// Gets or sets the request handler.
        /// </summary>
        public CefRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Gets or sets the display handler.
        /// </summary>
        public CefDisplayHandler DisplayHandler { get; set; }

        /// <summary>
        /// Gets or sets the context menu handler.
        /// </summary>
        public CefContextMenuHandler ContextMenuHandler { get; set; }

        /// <summary>
        /// Gets or sets the focus handler.
        /// </summary>
        public CefFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Gets or sets the keyboard handler.
        /// </summary>
        public CefKeyboardHandler KeyboardHandler { get; set; }

        /// <summary>
        /// Gets or sets the js dialog handler.
        /// </summary>
        public CefJSDialogHandler JsDialogHandler { get; set; }

        /// <summary>
        /// Gets or sets the dialog handler.
        /// </summary>
        public CefDialogHandler DialogHandler { get; set; }

        /// <summary>
        /// Gets or sets the drag handler.
        /// </summary>
        public CefDragHandler DragHandler { get; set; }

        /// <summary>
        /// Gets or sets the download handler.
        /// </summary>
        public CefDownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Gets or sets the find handler.
        /// </summary>
        public CefFindHandler FindHandler { get; set; }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <returns>
        /// The <see cref="CefGlueClientParams"/>.
        /// </returns>
        public static CefGlueClientParams Create(CefGlueBrowser browser)
        {
            var clientParams = new CefGlueClientParams { Browser = browser };

            try
            {
                foreach (var enumKey in CefHandlerFakeTypes.GetAllHandlerKeys())
                {
                    object instance = null;

                    var service = CefHandlerFakeTypes.GetHandlerType(enumKey);
                    var keyStr = enumKey.EnumToString();
                    try
                    {
                        if (IoC.IsRegistered(service, keyStr))
                        {
                            instance = IoC.GetInstance(service, keyStr);
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                    }

                    switch (enumKey)
                    {
                        case CefHandlerKey.LifeSpanHandler:
                            if (instance is CefLifeSpanHandler spanHandler)
                            {
                                clientParams.LifeSpanHandler = spanHandler;
                            }
                            else
                            {
                                clientParams.LifeSpanHandler = new CefGlueLifeSpanHandler();
                            }

                            break;

                        case CefHandlerKey.LoadHandler:
                            if (instance is CefLoadHandler loadHandler)
                            {
                                clientParams.LoadHandler = loadHandler;
                            }
                            else
                            {
                                clientParams.LoadHandler = new CefGlueLoadHandler();
                            }

                            break;

                        case CefHandlerKey.RequestHandler:
                            if (instance is CefRequestHandler requestHandler)
                            {
                                clientParams.RequestHandler = requestHandler;
                            }
                            else
                            {
                                clientParams.RequestHandler = new CefGlueRequestHandler();
                            }

                            break;

                        case CefHandlerKey.DisplayHandler:
                            if (instance is CefDisplayHandler displayHandler)
                            {
                                clientParams.DisplayHandler = displayHandler;
                            }
                            else
                            {
                                clientParams.DisplayHandler = new CefGlueDisplayHandler();
                            }

                            break;

                        case CefHandlerKey.ContextMenuHandler:
                            if (instance is CefContextMenuHandler menuHandler)
                            {
                                clientParams.ContextMenuHandler = menuHandler;
                            }
                            else
                            {
                                clientParams.ContextMenuHandler = new CefGlueContextMenuHandler();
                            }

                            break;

                        case CefHandlerKey.FocusHandler:
                            if (instance is CefFocusHandler focusHandler)
                            {
                                clientParams.FocusHandler = focusHandler;
                            }

                            break;

                        case CefHandlerKey.KeyboardHandler:
                            if (instance is CefKeyboardHandler keyboardHandler)
                            {
                                clientParams.KeyboardHandler = keyboardHandler;
                            }

                            break;

                        case CefHandlerKey.JsDialogHandler:
                            if (instance is CefJSDialogHandler jsDialogHandler)
                            {
                                clientParams.JsDialogHandler = jsDialogHandler;
                            }

                            break;

                        case CefHandlerKey.DialogHandler:
                            if (instance is CefDialogHandler dialogHandler)
                            {
                                clientParams.DialogHandler = dialogHandler;
                            }

                            break;

                        case CefHandlerKey.DragHandler:
                            if (instance is CefDragHandler dragHandler)
                            {
                                clientParams.DragHandler = dragHandler;
                            }

                            break;

                        case CefHandlerKey.DownloadHandler:
                            if (instance is CefDownloadHandler downloadHandler)
                            {
                                clientParams.DownloadHandler = downloadHandler;
                            }

                            break;

                        case CefHandlerKey.FindHandler:
                            if (instance is CefFindHandler handler)
                            {
                                clientParams.FindHandler = handler;
                            }

                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return clientParams;
        }
    }
}
