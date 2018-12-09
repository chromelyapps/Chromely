// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadEndEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

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
        public int HttpStatusCode { get; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
    }
}
