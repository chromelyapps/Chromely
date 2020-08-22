// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core
{
    public interface IChromelyJsBindingHandler
    {
        string Key { get; }

        string ObjectName { get; set; }

        object BoundObject { get; set; }

        object BindingOptions { get; set; }
    }
}
