// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowCreationStyle.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using WinApi.User32;

namespace Chromely.Common
{
    /// <summary>
    /// Styles to be used in Windows environment.
    /// </summary>
    public class WindowCreationStyle
    {
        /// <summary>
        /// The window styles WS_*
        /// </summary>
        public WindowStyles WindowStyles { get; set; }
        /// <summary>
        /// The window extended styles WS_EX_*
        /// </summary>
        public WindowExStyles WindowExStyles { get; set; }
    }
}
