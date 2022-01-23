// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Represents window messsage loop interceptor.
/// </summary>
public interface IWindowMessageInterceptor
{
    /// <summary>
    /// This sets up the service.
    /// </summary>
    /// <param name="nativeHost">Chromely native host - instance of <see cref="IChromelyNativeHost"/>.</param>
    /// <param name="browserHandle">The browser window handle.</param>
    void Setup(IChromelyNativeHost nativeHost, IntPtr browserHandle);
}