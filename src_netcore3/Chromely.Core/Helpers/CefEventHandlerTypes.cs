// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefEventHandlerFakeTypes.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chromely.Core.Helpers
{
    /// <summary>
    /// The cef handler dummy types.
    /// The class holds a set of dummy handler interfaces as keys for registering handlers.
    /// This allow of the keys to be used for both CefSharp and CefGlue implementations.
    /// </summary>
    public static class CefEventHandlerTypes
    {
        /// <summary>
        /// The None interface.
        /// </summary>
        public interface INoneHandler { }

        /// <summary>
        /// The FrameLoadStart interface.
        /// </summary>
        public interface IFrameLoadStartHandler { }

        /// <summary>
        /// The AddressChanged interface.
        /// </summary>
        public interface IAddressChangedHandler { }

        /// <summary>
        /// The TitleChanged interface.
        /// </summary>
        public interface ITitleChangedHandler { }

        /// <summary>
        /// The FrameLoadEnd interface.
        /// </summary>
        public interface IFrameLoadEndHandler { }

        /// <summary>
        /// The LoadingStateChanged interface.
        /// </summary>
        public interface ILoadingStateChangedHandler { }

        /// <summary>
        /// The ConsoleMessage interface.
        /// </summary>
        public interface IConsoleMessageHandler { }

        /// <summary>
        /// The StatusMessageHandler interface.
        /// </summary>
        public interface IStatusMessageHandler { }

        /// <summary>
        /// The LoadError interface.
        /// </summary>
        public interface ILoadErrorHandler { }

        /// <summary>
        /// The TooltipChangedHandler interface.
        /// </summary>
        public interface ITooltipChangedHandler { }

        /// <summary>
        /// The BeforeCloseHandler interface.
        /// </summary>
        public interface IBeforeCloseHandler { }

        /// <summary>
        /// The BeforePopupHandler interface.
        /// </summary>
        public interface IBeforePopupHandler { }

        /// <summary>
        /// The PluginCrashedHandler interface.
        /// </summary>
        public interface IPluginCrashedHandler { }

        /// <summary>
        /// The RenderProcessTerminatedHandler interface.
        /// </summary>
        public interface IRenderProcessTerminatedHandler { }

        /// <summary>
        /// The get custom handler type.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetHandlerType(CefEventKey key)
        {
            switch (key)
            {
                case CefEventKey.None:
                    return typeof(INoneHandler);

                case CefEventKey.FrameLoadStart:
                    return typeof(IFrameLoadStartHandler);

                case CefEventKey.AddressChanged:
                    return typeof(IAddressChangedHandler);

                case CefEventKey.TitleChanged:
                    return typeof(ITitleChangedHandler);

                case CefEventKey.FrameLoadEnd:
                    return typeof(IFrameLoadEndHandler);

                case CefEventKey.LoadingStateChanged:
                    return typeof(ILoadingStateChangedHandler);

                case CefEventKey.ConsoleMessage:
                    return typeof(IConsoleMessageHandler);

                case CefEventKey.StatusMessage:
                    return typeof(IStatusMessageHandler);

                case CefEventKey.LoadError:
                    return typeof(ILoadErrorHandler);

                case CefEventKey.TooltipChanged:
                    return typeof(ITooltipChangedHandler);

                case CefEventKey.BeforeClose:
                    return typeof(IBeforeCloseHandler);

                case CefEventKey.BeforePopup:
                    return typeof(IBeforePopupHandler);

                case CefEventKey.PluginCrashed:
                    return typeof(IPluginCrashedHandler);

                case CefEventKey.RenderProcessTerminated:
                    return typeof(IRenderProcessTerminatedHandler);
            }

            return null;
        }

        /// <summary>
        /// The get all event handler keys.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<CefEventKey> GetAllEventHandlerKeys()
        {
            return Enum.GetValues(typeof(CefEventKey)).Cast<CefEventKey>();
        }
    }
}
