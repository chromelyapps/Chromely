// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Browser
{
    /// <summary>
    /// The loading state change event args.
    /// </summary>
    public class LoadingStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="isLoading">
        /// The is loading.
        /// </param>
        /// <param name="canGoBack">
        /// The can go back.
        /// </param>
        /// <param name="canGoForward">
        /// The can go forward.
        /// </param>
        public LoadingStateChangedEventArgs(bool isLoading, bool canGoBack, bool canGoForward)
        {
            IsLoading = isLoading;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
        }

        /// <summary>
        /// Gets a value indicating whether is loading.
        /// </summary>
        public bool IsLoading { get; }

        /// <summary>
        /// Gets a value indicating whether can go back.
        /// </summary>
        public bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether can go forward.
        /// </summary>
        public bool CanGoForward { get; }
    }
}
