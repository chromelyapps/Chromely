// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Subprocess.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Subprocess
{
    /// <summary>
    /// The Subprocess.
    /// </summary>
    public class Subprocess
    {
        private readonly SubprocessCefGlueApp _cefglueApp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subprocess"/> class.
        /// </summary>
        public Subprocess(string[] cmdlineArgs)
        {
            var subprocessParams = SubprocessParams.Parse(cmdlineArgs);
            _cefglueApp = new SubprocessCefGlueApp(subprocessParams);
        }

        /// <summary>
        /// Executes the <see cref="Subprocess"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Execute(string[] args)
        {
            int result;
            var type = args.GetArgumentValue(SubprocessArguments.ArgumentType);

            // The Crashpad Handler doesn't have any HostProcessIdArgument, so we must not try to
            // parse it lest we want an ArgumentNullException.
            if (type != "crashpad-handler")
            {
                var parentProcessId = int.Parse(args.GetArgumentValue(SubprocessArguments.HostProcessIdArgument));
                if (args.HasArgument(SubprocessArguments.ExitIfParentProcessClosed))
                {
                    Task.Factory.StartNew(() => AwaitParentProcessExit(parentProcessId), TaskCreationOptions.LongRunning);
                }
            }

            // For Windows 7 and above, best to include relevant app.manifest entries as well
            CefRuntime.EnableHighDpiSupport();

            if (type == "renderer")
            {
                var subProcess = new Subprocess(args);
                result = subProcess.Run(args);
            }
            else
            {
                result = ExecuteProcess(args);
            }

            if (ChromelyConfiguration.Instance.DebuggingMode)
            {
                Console.WriteLine("CefGlue Subprocess shutting down.");
            }

            return result;
        }

        internal int Run(string[] args)
        {
            CefMainArgs cefMainArgs = new CefMainArgs(args);
            return CefRuntime.ExecuteProcess(cefMainArgs, _cefglueApp, IntPtr.Zero);
        }

        internal static int ExecuteProcess(string[] args)
        {
            CefMainArgs cefMainArgs = new CefMainArgs(args);
            return CefRuntime.ExecuteProcess(cefMainArgs, null, IntPtr.Zero);
        }

        private static async void AwaitParentProcessExit(int parentProcessId)
        {
            try
            {
                var parentProcess = Process.GetProcessById(parentProcessId);
                parentProcess.WaitForExit();
            }
            catch (Exception e)
            {
                //main process probably died already
                Debug.WriteLine(e);
            }

            await Task.Delay(1000); //wait a bit before exiting

            Debug.WriteLine("CefGlue Subprocess shutting down forcibly.");

            Environment.Exit(0);
        }
    }
}
