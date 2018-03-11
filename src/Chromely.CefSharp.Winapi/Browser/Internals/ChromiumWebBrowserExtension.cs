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

 namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    using System;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;

    using global::CefSharp;


    public static class ChromiumWebBrowserExtension
    {
        public static void SetHandlers(this ChromiumWebBrowser browser)
        {
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
                            if ((instance != null) && (instance is ILifeSpanHandler))
                            {
                                browser.LifeSpanHandler = (ILifeSpanHandler)instance;
                            }
                            break;

                        case CefHandlerKey.LoadHandler:
                            if ((instance != null) && (instance is ILoadHandler))
                            {
                                browser.LoadHandler = (ILoadHandler)instance;
                            }
                            break;

                        case CefHandlerKey.RequestHandler:
                            if ((instance != null) && (instance is IRequestHandler))
                            {
                                browser.RequestHandler = (IRequestHandler)instance;
                            }
                            else
                            {
                                browser.RequestHandler = new CefSharpRequestHandler();
                            }
                            break;

                        case CefHandlerKey.DisplayHandler:
                            if ((instance != null) && (instance is IDisplayHandler))
                            {
                                browser.DisplayHandler = (IDisplayHandler)instance;
                            }
                            break;

                        case CefHandlerKey.ContextMenuHandler:
                            if ((instance != null) && (instance is IContextMenuHandler))
                            {
                                browser.MenuHandler = (IContextMenuHandler)instance;
                            }
                            else
                            {
                                browser.MenuHandler = new CefSharpContextMenuHandler();
                            }
                            break;

                        case CefHandlerKey.FocusHandler:
                            if ((instance != null) && (instance is IFocusHandler))
                            {
                                browser.FocusHandler = (IFocusHandler)instance;
                            }
                            break;

                        case CefHandlerKey.KeyboardHandler:
                            if ((instance != null) && (instance is IKeyboardHandler))
                            {
                                browser.KeyboardHandler = (IKeyboardHandler)instance;
                            }
                            break;

                        case CefHandlerKey.JSDialogHandler:
                            if ((instance != null) && (instance is IJsDialogHandler))
                            {
                                browser.JsDialogHandler = (IJsDialogHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DialogHandler:
                            if ((instance != null) && (instance is IDialogHandler))
                            {
                                browser.DialogHandler = (IDialogHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DragHandler:
                            if ((instance != null) && (instance is IDragHandler))
                            {
                                browser.DragHandler = (IDragHandler)instance;
                            }
                            break;

                        case CefHandlerKey.GeolocationHandler:
                            if ((instance != null) && (instance is IGeolocationHandler))
                            {
                                browser.GeolocationHandler = (IGeolocationHandler)instance;
                            }
                            break;

                        case CefHandlerKey.DownloadHandler:
                            if ((instance != null) && (instance is IDownloadHandler))
                            {
                                browser.DownloadHandler = (IDownloadHandler)instance;
                            }
                            break;

                        case CefHandlerKey.FindHandler:
                            if ((instance != null) && (instance is IFindHandler))
                            {
                                browser.FindHandler = (IFindHandler)instance;
                            }
                            break;

                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

        }
    }
}
