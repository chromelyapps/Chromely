// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserProcessHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The cef glue browser process handler.
    /// </summary>
    public class CefGlueBrowserProcessHandler : CefBrowserProcessHandler
    {
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

            if (ChromelyConfiguration.Instance.DebuggingMode)
            {
                Console.WriteLine("On CefGlue child process launch arguments:");
                Console.WriteLine(browser_cmd.ToString());
            }
        }
    }
}
