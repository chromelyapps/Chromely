// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefHandlerFakeTypes.cs" company="Chromely">
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
    public static class CefHandlerFakeTypes
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
        /// The get handler type.
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
        public static List<CefHandlerKey> GetAllHandlerKeys()
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
