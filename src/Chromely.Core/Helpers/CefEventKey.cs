// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefEventKey.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
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
        /// </summary>
        TooltipChanged,

        /// <summary>
        /// The before close.
        /// </summary>
        BeforeClose,

        /// <summary>
        /// The before popup.
        /// </summary>
        BeforePopup,

        /// <summary>
        /// The plugin crashed.
        /// </summary>
        PluginCrashed,

        /// <summary>
        /// The render process terminated.
        /// </summary>
        RenderProcessTerminated
   }
}
