// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetUrlChangedEventArgs.cs" company="Chromely Projects">
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
    /// The target url changed event args.
    /// </summary>
    public sealed class TargetUrlChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetUrlChangedEventArgs"/> class.
        /// </summary>
        /// <param name="targetUrl">
        /// The target url.
        /// </param>
        public TargetUrlChangedEventArgs(string targetUrl)
        {
            TargetUrl = targetUrl;
        }

        /// <summary>
        /// Gets the target url.
        /// </summary>
        public string TargetUrl { get; }
    }
}
