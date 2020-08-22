// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Browser
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
