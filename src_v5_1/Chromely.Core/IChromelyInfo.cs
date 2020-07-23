// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;

namespace Chromely.Core
{
    public interface IChromelyInfo
    {
        IChromelyResponse GetInfo(string requestId);
    }
}
