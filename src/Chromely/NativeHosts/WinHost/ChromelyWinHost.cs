// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;

namespace Chromely.NativeHost
{
    public class ChromelyWinHost : NativeHostBase
    {
        public ChromelyWinHost(IKeyboadHookHandler keyboadHandler = null): base(keyboadHandler)
        {
        }
    }
}
