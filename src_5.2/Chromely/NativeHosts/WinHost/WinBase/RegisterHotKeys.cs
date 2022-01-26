// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public abstract partial class NativeHostBase
{
    /// <summary>
    /// Place holder method to register hot keys.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    protected virtual void RegisterHotKeys(IntPtr hwnd)
    {
    }
}