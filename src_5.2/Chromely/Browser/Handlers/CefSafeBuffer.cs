// Copyright © 2014 The CefSharp Authors. All rights reserved.
// https://github.com/cefsharp/CefSharp/blob/master/CefSharp/ResourceHandler.cs
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace Chromely.Browser;

public class CefSafeBuffer : SafeBuffer
{
    public CefSafeBuffer(IntPtr data, ulong noOfBytes) : base(false)
    {
        SetHandle(data);
        Initialize(noOfBytes);
    }

    protected override bool ReleaseHandle()
    {
        return true;
    }
}