// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// The address changed event args.
/// </summary>
public class AddressChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddressChangedEventArgs"/> class.
    /// </summary>
    /// <param name="frame">
    /// The frame.
    /// </param>
    /// <param name="address">
    /// The address.
    /// </param>
    public AddressChangedEventArgs(CefFrame frame, string address)
    {
        Address = address;
        Frame = frame;
    }

    /// <summary>
    /// Gets the address.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Gets the frame.
    /// </summary>
    public CefFrame Frame { get; }
}