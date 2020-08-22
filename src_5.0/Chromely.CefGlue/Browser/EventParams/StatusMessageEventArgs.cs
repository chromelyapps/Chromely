// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusMessageEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.CefGlue.Browser.EventParams
{
    /// <summary>
    /// The status message event args.
    /// </summary>
    public class StatusMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMessageEventArgs"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public StatusMessageEventArgs(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; }
    }
}
