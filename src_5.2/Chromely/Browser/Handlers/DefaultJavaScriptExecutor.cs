// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

public class DefaultJavaScriptExecutor : IChromelyJavaScriptExecutor
{
    /// <summary>
    /// The browser.
    /// </summary>
    private readonly CefBrowser? _browser;

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
        if (frame is null)
        {
            Logger.Instance.Log.LogWarning("Frame {frameName} does not exist.", frameName);
            return string.Empty;
        }

        frame.ExecuteJavaScript(script, string.Empty, 0);

        return string.Empty;
    }

    public object ExecuteScript(string script)
    {
        var frame = _browser?.GetMainFrame();
        if (frame is null)
        {
            Logger.Instance.Log.LogWarning("Cannot accces main frame.");
            return string.Empty;
        }

        frame.ExecuteJavaScript(script, string.Empty, 0);

        return string.Empty;
    }

    public object? GetBrowser()
    {
        return _browser;
    }

    public object? GetMainFrame()
    {
        return _browser?.GetMainFrame();
    }

    public object? GetFrame(string name)
    {
        return _browser?.GetFrame(name);
    }

    public List<long> GetFrameIdentifiers
    {
        get
        {
            var list = _browser?.GetFrameIdentifiers()?.ToList();
            return list ?? new List<long>();
        }
    }

    public List<string> GetFrameNames
    {
        get
        {
            var list = _browser?.GetFrameNames()?.ToList();
            return list ?? new List<string>();
        }
    }
}