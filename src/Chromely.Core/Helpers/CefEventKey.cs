// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefEventKey.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Helpers
{
    /// <summary>
    /// The cef event key.
    /// </summary>
    public enum CefEventKey
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,

        /// <summary>
        /// The frame load start.
        /// </summary>
        FrameLoadStart,

        /// <summary>
        /// The address changed.
        /// </summary>
        AddressChanged,

        /// <summary>
        /// The title changed.
        /// </summary>
        TitleChanged,

        /// <summary>
        /// The frame load end.
        /// </summary>
        FrameLoadEnd,

        /// <summary>
        /// The loading state changed.
        /// </summary>
        LoadingStateChanged,

        /// <summary>
        /// The console message.
        /// </summary>
        ConsoleMessage,

        /// <summary>
        /// The status message.
        /// </summary>
        StatusMessage,

        /// <summary>
        /// The load error.
        /// </summary>
        LoadError,

        /// <summary>
        /// The tooltip changed.
        /// For CefGlue apps only.
        /// </summary>
        TooltipChanged,

        /// <summary>
        /// The before close.
        /// For CefGlue apps only.
        /// </summary>
        BeforeClose,

        /// <summary>
        /// The before popup.
        /// For CefGlue apps only.
        /// </summary>
        BeforePopup,

        /// <summary>
        /// The plugin crashed.
        /// For CefGlue apps only.
        /// </summary>
        PluginCrashed,

        /// <summary>
        /// The render process terminated.
        /// For CefGlue apps only.
        /// </summary>
        RenderProcessTerminated
    }
}
