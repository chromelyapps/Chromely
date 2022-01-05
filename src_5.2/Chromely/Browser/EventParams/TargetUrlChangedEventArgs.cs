// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

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