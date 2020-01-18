using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core
{
    public interface IChromelyJavaScriptExecutor
    {
        void ExecuteScript(string frameName, string script);
        void ExecuteScript(string script);
        object GetBrowser();
        object GetMainFrame();
        object GetFrame(string frameName);
        List<long> GetFrameIdentifiers { get; }
        List<string> GetFrameNames { get; }
    }
}
