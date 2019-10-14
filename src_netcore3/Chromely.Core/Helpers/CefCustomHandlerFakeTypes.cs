// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefCustomHandlerFakeTypes.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The cef handler dummy types.
    /// The class holds a set of dummy handler interfaces as keys for registering handlers.
    /// This allow of the keys to be used for both CefSharp and CefGlue implementations.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1502:ElementMustNotBeOnSingleLine", Justification = "Reviewed. Suppression is OK here.")]
    public static class CefCustomHandlerFakeTypes
    {
        /// <summary>
        /// The LifeSpanHandler interface.
        /// </summary>
        public interface ILifeSpanHandler {}

        /// <summary>
        /// The LoadHandler interface.
        /// </summary>
        public interface ILoadHandler {}

        /// <summary>
        /// The RequestHandler interface.
        /// </summary>
        public interface IRequestHandler {}

        /// <summary>
        /// The DisplayHandler interface.
        /// </summary>
        public interface IDisplayHandler {}

        /// <summary>
        /// The ContextMenuHandler interface.
        /// </summary>
        public interface IContextMenuHandler {}

        /// <summary>
        /// The FocusHandler interface.
        /// </summary>
        public interface IFocusHandler {}

        /// <summary>
        /// The KeyboardHandler interface.
        /// </summary>
        public interface IKeyboardHandler {}

        /// <summary>
        /// The JsDialogHandler interface.
        /// </summary>
        public interface IJsDialogHandler {}

        /// <summary>
        /// The DialogHandler interface.
        /// </summary>
        public interface IDialogHandler {}

        /// <summary>
        /// The DragHandler interface.
        /// </summary>
        public interface IDragHandler {}

        /// <summary>
        /// The DownloadHandler interface.
        /// </summary>
        public interface IDownloadHandler {}

        /// <summary>
        /// The FindHandler interface.
        /// </summary>
        public interface IFindHandler {}

        /// <summary>
        /// The get custom handler type.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetHandlerType(CefHandlerKey key)
        {
            switch (key)
            {
                case CefHandlerKey.LifeSpanHandler:
                    return typeof(ILifeSpanHandler);

                case CefHandlerKey.LoadHandler:
                    return typeof(ILoadHandler);

                case CefHandlerKey.RequestHandler:
                    return typeof(IRequestHandler);

                case CefHandlerKey.DisplayHandler:
                    return typeof(IDisplayHandler);

                case CefHandlerKey.ContextMenuHandler:
                    return typeof(IContextMenuHandler);

                case CefHandlerKey.FocusHandler:
                    return typeof(IFocusHandler);

                case CefHandlerKey.KeyboardHandler:
                    return typeof(IKeyboardHandler);

                case CefHandlerKey.JsDialogHandler:
                    return typeof(IJsDialogHandler);

                case CefHandlerKey.DialogHandler:
                    return typeof(IDialogHandler);

                case CefHandlerKey.DragHandler:
                    return typeof(IDragHandler);

                case CefHandlerKey.DownloadHandler:
                    return typeof(IDownloadHandler);

                case CefHandlerKey.FindHandler:
                    return typeof(IFindHandler);
            }

            return null;
        }


        /// <summary>
        /// The get all handler keys.
        /// </summary>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public static List<CefHandlerKey> GetAllCustomHandlerKeys()
        {
            var types = new List<CefHandlerKey>
                            {
                                CefHandlerKey.LifeSpanHandler,
                                CefHandlerKey.LoadHandler,
                                CefHandlerKey.RequestHandler,
                                CefHandlerKey.DisplayHandler,
                                CefHandlerKey.ContextMenuHandler,
                                CefHandlerKey.FocusHandler,
                                CefHandlerKey.KeyboardHandler,
                                CefHandlerKey.JsDialogHandler,
                                CefHandlerKey.DialogHandler,
                                CefHandlerKey.DragHandler,
                                CefHandlerKey.DownloadHandler,
                                CefHandlerKey.FindHandler
                            };

            return types;
        }
    }
}
