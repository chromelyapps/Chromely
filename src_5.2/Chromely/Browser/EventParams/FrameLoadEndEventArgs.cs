// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// The load end event args.
/// </summary>
public class FrameLoadEndEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FrameLoadEndEventArgs"/> class.
    /// </summary>
    /// <param name="frame">
    /// The frame.
    /// </param>
    /// <param name="httpStatusCode">
    /// The http status code.
    /// </param>
    public FrameLoadEndEventArgs(CefFrame frame, int httpStatusCode)
    {
        Frame = frame;
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Gets the http status code.
    /// </summary>
    public int HttpStatusCode { get; private set; }

    /// <summary>
    /// Gets the frame.
    /// </summary>
    public CefFrame Frame { get; private set; }
}