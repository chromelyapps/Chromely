using System.Collections.Generic;
using System.Linq;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public class JavaScriptExecutor : IChromelyJavaScriptExecutor
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly CefBrowser _browser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public JavaScriptExecutor(CefBrowser browser)
        {
            _browser = browser;
        }

        public void ExecuteScript(string frameName, string script)
        {
            CefFrame frame = _browser?.GetFrame(frameName);
            if (frame == null)
            {
                Logger.Instance.Log.Error($"Frame {frameName} does not exist.");
                return;
            }

            frame.ExecuteJavaScript(script, null, 0);
        }

        public void ExecuteScript(string script)
        {
            CefFrame frame = _browser?.GetMainFrame();
            if (frame == null)
            {
                Logger.Instance.Log.Error("Mainframe is nullt.");
                return;
            }

            frame.ExecuteJavaScript(script, null, 0);
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
