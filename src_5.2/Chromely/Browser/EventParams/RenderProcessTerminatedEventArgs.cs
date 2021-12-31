// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using Xilium.CefGlue;

namespace Chromely.Browser
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
