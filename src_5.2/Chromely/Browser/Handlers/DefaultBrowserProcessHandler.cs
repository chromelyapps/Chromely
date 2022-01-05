// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default CEF browser process handler.
/// </summary>
public class DefaultBrowserProcessHandler : CefBrowserProcessHandler
{
    protected readonly IChromelyConfiguration _config;

    public DefaultBrowserProcessHandler(IChromelyConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// The on before child process launch.
    /// </summary>
    /// <param name="browser_cmd">
    /// The command line.
    /// </param>
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