// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadErrorEventArgs.cs" company="Chromely Projects">
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
    /// The load error event args.
    /// </summary>
    public class LoadErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadErrorEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="errorText">
        /// The error text.
        /// </param>
        /// <param name="failedUrl">
        /// The failed url.
        /// </param>
        public LoadErrorEventArgs(CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            this.Frame = frame;
            this.ErrorCode = errorCode;
            this.ErrorText = errorText;
            this.FailedUrl = failedUrl;
        }

        /// <summary>
        /// Gets the failed url.
        /// </summary>
        public string FailedUrl { get; }

        /// <summary>
        /// Gets the error text.
        /// </summary>
        public string ErrorText { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public CefErrorCode ErrorCode { get; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
    }
}
