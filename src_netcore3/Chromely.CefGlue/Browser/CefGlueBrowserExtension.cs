// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210

using System;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;

namespace Chromely.CefGlue.Browser
{
    /// <summary>
    /// The chromium web browser extension.
    /// </summary>
    public static class CefGlueBrowserExtension
    {
        /// <summary>
        /// The set event handlers.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public static void SetEventHandlers(this CefGlueBrowser browser, IChromelyContainer container)
        {
            try
            {
                foreach (var enumKey in CefEventHandlerTypes.GetAllEventHandlerKeys())
                {
                    object instance = null;

                    var service = CefEventHandlerTypes.GetHandlerType(enumKey);
                    var keyStr = enumKey.EnumToString();
                    try
                    {
                        if (container.IsRegistered(service, keyStr))
                        {
                            instance = container.GetInstance(service, keyStr);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.Log.Error(exception);
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

                        case CefEventKey.TooltipChanged:
                            if (instance is ChromelyEventHandler<TooltipEventArgs> tooltipChanged)
                            {
                                browser.TooltipChanged += tooltipChanged.Handler;
                            }

                            break;

                        case CefEventKey.BeforeClose:
                            if (instance is ChromelyEventHandler<BeforeCloseEventArgs> beforeClose)
                            {
                                browser.BeforeClose += beforeClose.Handler;
                            }

                            break;

                        case CefEventKey.BeforePopup:
                            if (instance is ChromelyEventHandler<BeforePopupEventArgs> beforePopup)
                            {
                                browser.BeforePopup += beforePopup.Handler;
                            }

                            break;

                        case CefEventKey.PluginCrashed:
                            if (instance is ChromelyEventHandler<PluginCrashedEventArgs> pluginCrashed)
                            {
                                browser.PluginCrashed += pluginCrashed.Handler;
                            }

                            break;

                        case CefEventKey.RenderProcessTerminated:
                            if (instance is ChromelyEventHandler<RenderProcessTerminatedEventArgs> renderProcessTerminated)
                            {
                                browser.RenderProcessTerminated += renderProcessTerminated.Handler;
                            }

                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }
    }
}
