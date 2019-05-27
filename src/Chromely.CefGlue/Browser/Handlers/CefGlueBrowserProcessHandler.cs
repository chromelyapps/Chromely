// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueBrowserProcessHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using Chromely.CefGlue.Subprocess;
using Chromely.Core;
using Chromely.Core.Infrastructure;
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
        /// <param name="commandLine">
        /// The command line.
        /// </param>
        protected override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
        {
            // We need to know the process Id to establish WCF communication and for monitoring of parent process exit
            commandLine.AppendSwitch(SubprocessArguments.HostProcessIdArgument, Process.GetCurrentProcess().Id.ToString());
            commandLine.AppendSwitch(SubprocessArguments.ExitIfParentProcessClosed, "1");

            var schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();
                var argument = string.Empty;
                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler handler)
                    {
                        bool isStandardScheme = UrlScheme.IsStandardScheme(handler.SchemeName);
                        if (!isStandardScheme)
                        {
                            argument += handler.SchemeName + SubprocessArguments.Separator;
                        }
                    }
                }

                argument = argument.TrimEnd(SubprocessArguments.Separator);
                commandLine.AppendSwitch(SubprocessArguments.CustomSchemeArgument, argument);
            }

            // Get all custom command line argument switches
            if (ChromelyConfiguration.Instance.CommandLineArgs != null)
            {
                var argument = string.Empty;
                foreach (var commandArg in ChromelyConfiguration.Instance.CommandLineArgs)
                {
                    if (commandArg.Item3)
                    {
                        argument += commandArg.Item1 + SubprocessArguments.ChildSeparator + commandArg.Item2 + SubprocessArguments.ChildSeparator + "T" + SubprocessArguments.Separator;
                    }
                    else
                    {
                        argument += string.Empty + SubprocessArguments.ChildSeparator + commandArg.Item2 + SubprocessArguments.ChildSeparator + "F" + SubprocessArguments.Separator;
                    }
                }

                // Disable security features
                argument += "default-encoding" + SubprocessArguments.ChildSeparator + "utf-8" + SubprocessArguments.ChildSeparator + "T" + SubprocessArguments.Separator;
                argument += string.Empty + SubprocessArguments.ChildSeparator + "allow-file-access-from-files" + SubprocessArguments.ChildSeparator + "F" + SubprocessArguments.Separator;
                argument += string.Empty + SubprocessArguments.ChildSeparator + "allow-universal-access-from-files" + SubprocessArguments.ChildSeparator + "F" + SubprocessArguments.Separator;
                argument += string.Empty + SubprocessArguments.ChildSeparator + "disable-web-security" + SubprocessArguments.ChildSeparator + "F" + SubprocessArguments.Separator;
                argument += string.Empty + SubprocessArguments.ChildSeparator + "ignore-certificate-errors" + SubprocessArguments.ChildSeparator + "F" + SubprocessArguments.Separator;
               
                argument = argument.TrimEnd(SubprocessArguments.Separator);
                commandLine.AppendSwitch(SubprocessArguments.CustomCmdlineArgument, argument);
            }

            // Disable security features
            commandLine.AppendSwitch("default-encoding", "utf-8");
            commandLine.AppendSwitch("allow-file-access-from-files");
            commandLine.AppendSwitch("allow-universal-access-from-files");
            commandLine.AppendSwitch("disable-web-security");
            commandLine.AppendSwitch("ignore-certificate-errors");

            if (ChromelyConfiguration.Instance.DebuggingMode)
            {
                Console.WriteLine("On CefGlue child process launch arguments:");
                Console.WriteLine(commandLine.ToString());
            }
        }
    }
}
