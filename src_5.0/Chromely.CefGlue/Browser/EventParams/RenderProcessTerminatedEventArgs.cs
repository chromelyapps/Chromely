// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderProcessTerminatedEventArgs.cs" company="Chromely Projects">
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
    /// The render process terminated event args.
    /// </summary>
    public class RenderProcessTerminatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderProcessTerminatedEventArgs"/> class.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        public RenderProcessTerminatedEventArgs(CefTerminationStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public CefTerminationStatus Status { get; private set; }
    }
}
