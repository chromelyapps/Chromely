// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameLoadStartEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.EventParams
{
    /// <summary>
    /// The load start event args.
    /// </summary>
    public class FrameLoadStartEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLoadStartEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        public FrameLoadStartEventArgs(CefFrame frame)
        {
            Frame = frame;
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
    }
}