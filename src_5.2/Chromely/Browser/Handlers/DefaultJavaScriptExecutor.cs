// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Implements <see cref="IChromelyJavaScriptExecutor"/>
/// </summary>
public class DefaultJavaScriptExecutor : IChromelyJavaScriptExecutor
{
    /// <summary>
    /// The browser.
    /// </summary>
    private readonly CefBrowser? _browser;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultJavaScriptExecutor"/>.
    /// </summary>
    /// <param name="browser">Instance of <see cref="CefBrowser"/>.</param>
    public DefaultJavaScriptExecutor(CefBrowser browser)
    {
        _browser = browser;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public object? GetBrowser()
    {
        return _browser;
    }

    /// <inheritdoc/>
    public object? GetMainFrame()
    {
        return _browser?.GetMainFrame();
    }

    /// <inheritdoc/>
    public object? GetFrame(string name)
    {
        return _browser?.GetFrame(name);
    }

    /// <inheritdoc/>
    public List<long> GetFrameIdentifiers
    {
        get
        {
            var list = _browser?.GetFrameIdentifiers()?.ToList();
            return list ?? new List<long>();
        }
    }

    /// <inheritdoc/>
    public List<string> GetFrameNames
    {
        get
        {
            var list = _browser?.GetFrameNames()?.ToList();
            return list ?? new List<string>();
        }
    }
}