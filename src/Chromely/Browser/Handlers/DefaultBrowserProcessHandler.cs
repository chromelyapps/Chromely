// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="CefBrowserProcessHandler"/>.
/// </summary>
public class DefaultBrowserProcessHandler : CefBrowserProcessHandler
{
    protected readonly IChromelyConfiguration _config;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultBrowserProcessHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    public DefaultBrowserProcessHandler(IChromelyConfiguration config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    protected override void OnBeforeChildProcessLaunch(CefCommandLine browser_cmd)
    {
        // Disable security features
        browser_cmd.AppendSwitch("default-encoding", "utf-8");
        browser_cmd.AppendSwitch("allow-file-access-from-files");
        browser_cmd.AppendSwitch("allow-universal-access-from-files");
        browser_cmd.AppendSwitch("disable-web-security");
        browser_cmd.AppendSwitch("ignore-certificate-errors");

        if (_config.DebuggingMode)
        {
            Console.WriteLine("On CefGlue child process launch arguments:");
            Console.WriteLine(browser_cmd.ToString());
        }
    }
}