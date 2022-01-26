// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

/// <summary>
/// Represents Windows OS keyboard handler.
/// </summary>
public interface IKeyboadHookHandler
{
    /// <summary>
    /// Set the Windows OS native host.
    /// </summary>
    /// <param name="nativeHost">The Windows native host of type <see cref="IChromelyNativeHost"/>.</param>
    void SetNativeHost(IChromelyNativeHost nativeHost);

    /// <summary>
    /// Handler keyboard keys.
    /// </summary>
    /// <param name="handle">Windows OS handle.</param>
    /// <param name="param">The key parameter.</param>
    /// <returns>true is handled, otherwise false.</returns>
    bool HandleKey(IntPtr handle, object param);
}