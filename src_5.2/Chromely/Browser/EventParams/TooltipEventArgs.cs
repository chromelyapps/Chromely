// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Browser
{
    /// <summary>
    /// The tooltip event args.
    /// </summary>
    public class TooltipEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipEventArgs"/> class.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public TooltipEventArgs(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
