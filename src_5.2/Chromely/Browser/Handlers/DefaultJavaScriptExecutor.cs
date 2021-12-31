// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;
using Chromely.Core;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultJavaScriptExecutor : IChromelyJavaScriptExecutor
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly CefBrowser _browser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public DefaultJavaScriptExecutor(CefBrowser browser)
        {
            _browser = browser;
        }

        public object ExecuteScript(string frameName, string script)
        {
            var frame = _browser?.GetFrame(frameName);
            if (frame == null)
            {
                Logger.Instance.Log.LogWarning($"Frame {frameName} does not exist.");
                return null;
            }

            frame.ExecuteJavaScript(script, null, 0);

            return null;
        }

        public object ExecuteScript(string script)
        {
            var frame = _browser?.GetMainFrame();
            if (frame == null)
            {
                Logger.Instance.Log.LogWarning("Cannot accces main frame.");
                return null;
            }

            frame.ExecuteJavaScript(script, null, 0);
            return null;
        }

        public object GetBrowser()
        {
            return _browser;
        }

        public object GetMainFrame()
        {
            return _browser?.GetMainFrame();
        }

        public object GetFrame(string name)
        {
            return _browser?.GetFrame(name);
        }

        public List<long> GetFrameIdentifiers => _browser?.GetFrameIdentifiers()?.ToList();

        public List<string> GetFrameNames => _browser?.GetFrameNames()?.ToList();
    }
}
