// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadingStateChangedEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;

    /// <summary>
    /// The loading state changed event args.
    /// </summary>
    public sealed class LoadingStateChangedEventArgs : EventArgs
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
            Loading = isLoading;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
        }

        /// <summary>
        /// Gets a value indicating whether loading.
        /// </summary>
        public bool Loading { get; }

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
