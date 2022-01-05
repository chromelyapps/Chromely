// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public interface IKeyboadHookHandler
{
    void SetNativeHost(IChromelyNativeHost nativeHost);
    bool HandleKey(IntPtr handle, object param);
}