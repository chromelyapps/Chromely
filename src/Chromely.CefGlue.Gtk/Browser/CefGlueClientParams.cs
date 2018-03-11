/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.CefGlue.Gtk.Browser
{
    using System;
    using Chromely.CefGlue.Gtk.Browser.Handlers;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    public class CefGlueClientParams
    {
        public CefGlueBrowser Browser { get; set; }
        public CefLifeSpanHandler LifeSpanHandler { get; set; }
        public CefLoadHandler LoadHandler { get; set; }
        public CefRequestHandler RequestHandler { get; set; }
        public CefDisplayHandler DisplayHandler { get; set; }
        public CefContextMenuHandler ContextMenuHandler { get; set; }
        public CefFocusHandler FocusHandler { get; set; }
        public CefKeyboardHandler KeyboardHandler { get; set; }
        public CefJSDialogHandler JSDialogHandler { get; set; }
        public CefDialogHandler DialogHandler { get; set; }
        public CefDragHandler DragHandler { get; set; }
        public CefGeolocationHandler GeolocationHandler { get; set; }
        public CefDownloadHandler DownloadHandler { get; set; }
        public CefFindHandler FindHandler { get; set; }

        public static CefGlueClientParams Create(CefGlueBrowser browser)
        {
            CefGlueClientParams clientParams = new CefGlueClientParams();
            clientParams.Browser = browser;

            try
            {
                foreach (var enumKey in CefHandlerDummyTypes.GetAllHandlerKeys())
                {
                    object instance = null;

                    Type service = CefHandlerDummyTypes.GetHandlerType(enumKey);
                    string keyStr = enumKey.EnumToString();
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
                            if ((instance != null) && (instance is CefLifeSpanHandler))
                            {
                                clientParams.LifeSpanHandler = (CefLifeSpanHandler)instance;
                            }
                            else
                            {
                                clientParams.LifeSpanHandler = new CefGlueLifeSpanHandler();
                            }
                            break;

                        case CefHandlerKey.LoadHandler:
                            if ((instance != null) && (instance is CefLoadHandler))
                            {
                                clientParams.LoadHandler = (CefLoadHandler)instance;
                            }
                            else
                            {
                                clientParams.LoadHandler = new CefGlueLoadHandler();
                            }
                            break;

                        case CefHandlerKey.RequestHandler:
                            if ((instance != null) && (instance is CefRequestHandler))
                            {
                                clientParams.RequestHandler = (CefRequestHandler)instance;
                            }
                            else
                            {
                                clientParams.RequestHandler = new CefGlueRequestHandler();
                            }
                            break;

                        case CefHandlerKey.DisplayHandler:
                            if ((instance != null) && (instance is CefDisplayHandler))
                            {
                                clientParams.DisplayHandler = (CefDisplayHandler)instance;
                            }
                            else
                            {
                                clientParams.DisplayHandler = new CefGlueDisplayHandler();
                            }
                            break;

                        case CefHandlerKey.ContextMenuHandler:
                            if ((instance != null) && (instance is CefContextMenuHandler))
                            {
                                clientParams.ContextMenuHandler = (CefContextMenuHandler)instance;
                            }
                            else
                            {
                                clientParams.ContextMenuHandler = new CefGlueContextMenuHandler();
                            }
                            break;

                        case CefHandlerKey.FocusHandler:
                            if ((instance != null) && (instance is CefFocusHandler))
                            {
                                clientParams.FocusHandler = (CefFocusHandler)instance;
                            }
                            break;

                        case CefHandlerKey.KeyboardHandler:
                            if ((instance != null) && (instance is CefKeyboardHandler))
                            {
                                clientParams.KeyboardHandler = (CefKeyboardHandler)instance;
                            }
                            break;

                        case CefHandlerKey.JSDialogHandler:
                            if ((instance != null) && (instance is CefJSDialogHandler))
                            {
                                clientParams.JSDialogHandler = (CefJSDialogHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DialogHandler:
                            if ((instance != null) && (instance is CefDialogHandler))
                            {
                                clientParams.DialogHandler = (CefDialogHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DragHandler:
                            if ((instance != null) && (instance is CefDragHandler))
                            {
                                clientParams.DragHandler = (CefDragHandler)instance;
                            }
                            break;

                        case CefHandlerKey.GeolocationHandler:
                            if ((instance != null) && (instance is CefGeolocationHandler))
                            {
                                clientParams.GeolocationHandler = (CefGeolocationHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DownloadHandler:
                            if ((instance != null) && (instance is CefDownloadHandler))
                            {
                                clientParams.DownloadHandler = (CefDownloadHandler)instance;
                            }
                            break;

                        case CefHandlerKey.FindHandler:
                            if ((instance != null) && (instance is CefFindHandler))
                            {
                                clientParams.FindHandler = (CefFindHandler)instance;
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
