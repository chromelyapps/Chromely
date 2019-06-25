// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyCefWrapper.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core
{
    /// <summary>
    /// List of all supported CEF wrappers.
    /// </summary>
    public enum ChromelyCefWrapper
    {
        /// <summary>
        /// See: https://gitlab.com/xiliumhq/chromiumembedded/cefglue
        /// </summary>
        CefGlue,

        /// <summary>
        /// See: https://github.com/cefsharp/CefSharp
        /// </summary>
        CefSharp
    }
}
