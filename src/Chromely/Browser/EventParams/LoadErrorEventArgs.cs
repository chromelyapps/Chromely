// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

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
        Frame = frame;
        ErrorCode = errorCode;
        ErrorText = errorText;
        FailedUrl = failedUrl;
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