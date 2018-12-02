// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadStartEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The load start event args.
    /// </summary>
    public class LoadStartEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadStartEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        public LoadStartEventArgs(CefFrame frame)
        {
            this.Frame = frame;
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
    }
}
