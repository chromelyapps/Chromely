// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TitleChangedEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// -----------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.CefGlue.Browser.EventParams
{
    /// <summary>
    /// The title changed event args.
    /// </summary>
    public class TitleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TitleChangedEventArgs"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public TitleChangedEventArgs(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; }
    }
}
