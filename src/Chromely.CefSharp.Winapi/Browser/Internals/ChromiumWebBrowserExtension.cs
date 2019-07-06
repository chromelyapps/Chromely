// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromiumWebBrowserExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using global::CefSharp;
using Chromely.CefSharp.Winapi.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;

namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    /// <summary>
    /// The chromium web browser extension.
    /// </summary>
    internal static class ChromiumWebBrowserExtension
    {
        /// <summary>
        /// The set event handlers.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        internal static void SetEventHandlers(this ChromiumWebBrowser browser)
        {
            try
            {
                foreach (var enumKey in CefEventHandlerFakeTypes.GetAllEventHandlerKeys())
                {
                    object instance = null;

                    var service = CefEventHandlerFakeTypes.GetHandlerType(enumKey);
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
                        case CefEventKey.None:
                            break;

                        case CefEventKey.FrameLoadStart:
                            if (instance is ChromelyEventHandler<FrameLoadStartEventArgs> frameLoadStart)
                            {
                                browser.FrameLoadStart += frameLoadStart.Handler;
                            }

                            break;

                        case CefEventKey.AddressChanged:
                            if (instance is ChromelyEventHandler<AddressChangedEventArgs> addressChanged)
                            {
                                browser.AddressChanged += addressChanged.Handler;
                            }

                            break;

                        case CefEventKey.TitleChanged:
                            if (instance is ChromelyEventHandler<TitleChangedEventArgs> titleChanged)
                            {
                                browser.TitleChanged += titleChanged.Handler;
                            }

                            break;

                        case CefEventKey.FrameLoadEnd:
                            if (instance is ChromelyEventHandler<FrameLoadEndEventArgs> frameLoadEnd)
                            {
                                browser.FrameLoadEnd += frameLoadEnd.Handler;
                            }

                            break;

                        case CefEventKey.LoadingStateChanged:
                            if (instance is ChromelyEventHandler<LoadingStateChangedEventArgs> loadingStateChanged)
                            {
                                browser.LoadingStateChanged += loadingStateChanged.Handler;
                            }

                            break;

                        case CefEventKey.ConsoleMessage:
                            if (instance is ChromelyEventHandler<ConsoleMessageEventArgs> consoleMessage)
                            {
                                browser.ConsoleMessage += consoleMessage.Handler;
                            }

                            break;

                        case CefEventKey.StatusMessage:
                            if (instance is ChromelyEventHandler<StatusMessageEventArgs> statusMessage)
                            {
                                browser.StatusMessage += statusMessage.Handler;
                            }

                            break;

                        case CefEventKey.LoadError:
                            if (instance is ChromelyEventHandler<LoadErrorEventArgs> loadError)
                            {
                                browser.LoadError += loadError.Handler;
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

        /// <summary>
        /// The set custom handlers.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        internal static void SetCustomHandlers(this ChromiumWebBrowser browser)
        {
            try
            {
                foreach (var enumKey in CefCustomHandlerFakeTypes.GetAllCustomHandlerKeys())
                {
                    object instance = null;

                    var service = CefCustomHandlerFakeTypes.GetHandlerType(enumKey);
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
