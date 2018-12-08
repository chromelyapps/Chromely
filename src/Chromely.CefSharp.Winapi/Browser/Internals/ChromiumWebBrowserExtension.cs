// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromiumWebBrowserExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    using System;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using global::CefSharp;

    /// <summary>
    /// The chromium web browser extension.
    /// </summary>
    public static class ChromiumWebBrowserExtension
    {
        /// <summary>
        /// The set handlers.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public static void SetHandlers(this ChromiumWebBrowser browser)
        {
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
                            if (instance is ILifeSpanHandler lifeSpanHandler)
                            {
                                browser.LifeSpanHandler = lifeSpanHandler;
                            }
                            else
                            {
                                browser.LifeSpanHandler = new CefSharpLifeSpanHandler();
                            }

                            break;

                        case CefHandlerKey.LoadHandler:
                            if (instance is ILoadHandler loadHandler)
                            {
                                browser.LoadHandler = loadHandler;
                            }

                            break;

                        case CefHandlerKey.RequestHandler:
                            if (instance is IRequestHandler requestHandler)
                            {
                                browser.RequestHandler = requestHandler;
                            }
                            else
                            {
                                browser.RequestHandler = new CefSharpRequestHandler();
                            }

                            break;

                        case CefHandlerKey.DisplayHandler:
                            if (instance is IDisplayHandler displayHandler)
                            {
                                browser.DisplayHandler = displayHandler;
                            }

                            break;

                        case CefHandlerKey.ContextMenuHandler:
                            if (instance is IContextMenuHandler contextMenuHandler)
                            {
                                browser.MenuHandler = contextMenuHandler;
                            }
                            else
                            {
                                browser.MenuHandler = new CefSharpContextMenuHandler();
                            }

                            break;

                        case CefHandlerKey.FocusHandler:
                            if (instance is IFocusHandler focusHandler)
                            {
                                browser.FocusHandler = focusHandler;
                            }

                            break;

                        case CefHandlerKey.KeyboardHandler:
                            if (instance is IKeyboardHandler keyboardHandler)
                            {
                                browser.KeyboardHandler = keyboardHandler;
                            }

                            break;

                        case CefHandlerKey.JsDialogHandler:
                            if (instance is IJsDialogHandler jsDialogHandler)
                            {
                                browser.JsDialogHandler = jsDialogHandler;
                            }

                            break;

                        case CefHandlerKey.DialogHandler:
                            if (instance is IDialogHandler dialogHandler)
                            {
                                browser.DialogHandler = dialogHandler;
                            }

                            break;

                        case CefHandlerKey.DragHandler:
                            if (instance is IDragHandler dragHandler)
                            {
                                browser.DragHandler = dragHandler;
                            }

                            break;

                        case CefHandlerKey.DownloadHandler:
                            if (instance is IDownloadHandler downloadHandler)
                            {
                                browser.DownloadHandler = downloadHandler;
                            }

                            break;

                        case CefHandlerKey.FindHandler:
                            if (instance is IFindHandler findHandler)
                            {
                                browser.FindHandler = findHandler;
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
