// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace Chromely.Core
{
    public interface IChromelyJavaScriptExecutor
    {
        object ExecuteScript(string frameName, string script);
        object ExecuteScript(string script);
        object GetBrowser();
        object GetMainFrame();
        object GetFrame(string frameName);
        List<long> GetFrameIdentifiers { get; }
        List<string> GetFrameNames { get; }
    }
}
