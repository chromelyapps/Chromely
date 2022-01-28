// Copyright © 2014 The CefSharp Authors. All rights reserved.
// https://github.com/cefsharp/CefSharp/blob/master/CefSharp/ResourceHandler.cs
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// A wrapper for <see cref="CefSafeBuffer"/>.
/// </summary>
public class CefSafeBuffer : SafeBuffer
{
    /// <summary>
    /// Initializes a new instance of <see cref="CefSafeBuffer"/>.
    /// </summary>
    /// <param name="dataHandle">The data handle.</param>
    /// <param name="noOfBytes">Number of bytes in data.</param>
    public CefSafeBuffer(IntPtr dataHandle, ulong noOfBytes) : base(false)
    {
        SetHandle(dataHandle);
        Initialize(noOfBytes);
    }

    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        return true;
    }
}