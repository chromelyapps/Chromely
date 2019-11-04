// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameLoadEndEventArgs.cs" company="Chromely Projects">
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
    /// The load end event args.
    /// </summary>
    public class FrameLoadEndEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLoadEndEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="httpStatusCode">
        /// The http status code.
        /// </param>
        public FrameLoadEndEventArgs(CefFrame frame, int httpStatusCode)
        {
            Frame = frame;
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        public int HttpStatusCode { get; private set; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; private set; }
    }
}