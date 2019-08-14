// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;

namespace Chromely.CefGlue.Integration.TestApp
{
    /// <summary>
    /// 
    /// This is a minimal chromely application to be used during integration tests.
    ///
    /// PLEASE NOTE:
    /// Due it is cross platform it MUST NOT reference PLATFORM SPECIFIC ASSEMBLIES.
    ///
    /// Use projects in Chromely-Demo to show platform specific samples !
    /// 
    /// It will emit console outputs starting with "CI-TRACE:" which are checked
    /// in the test run - so DON'T REMOVE them.
    /// 
    /// </summary>
    internal static class Program
    {
        private const string TraceSignature = "CI-TRACE:";
        private static IChromelyWindow _window;

        private static void CiTrace(string key, string value)
        {
            Console.WriteLine($"{TraceSignature} {key}={value}");
        }

        private static Stopwatch _startupTimer;

        private static int Main(string[] args)
        {
            CiTrace("Application", "Started");
            // measure startup time (maybe including CEF download)
            _startupTimer = new Stopwatch();
            _startupTimer.Start();

            var core = typeof(ChromelyConfiguration).Assembly;
            CiTrace("Chromely.Core", core.GetName().Version.ToString());
            CiTrace("Platform", ChromelyRuntime.Platform.ToString());

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            CiTrace("AppDirectory", appDirectory);
            var startUrl = $"file:///{appDirectory}/index.html";

            var config = ChromelyConfiguration
                .Create()
                .WithDebuggingMode(true)
                .WithLoadingCefBinariesIfNotFound(true)
                .WithSilentCefBinariesLoading(true)
                .RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage)
                .WithAppArgs(args)
                .WithHostBounds(1000, 600)
                //.WithHostFlag(HostFlagKey.CenterScreen, true | false - default true)
                //.WithHostFlag(HostFlagKey.Frameless, true | false -  default false)
                //.WithHostFlag(HostFlagKey.KioskMode, true | false - default false)
                .WithHostFlag(HostFlagKey.NoResize, true) // - default false)
                .WithHostFlag(HostFlagKey.NoMinMaxBoxes, true) // - default false)
                .WithStartUrl(startUrl);
            CiTrace("Configuration", "Created");

            try
            {
                using (_window = ChromelyWindow.Create(config))
                {
                    _window.RegisterEventHandler(CefEventKey.FrameLoadEnd, new ChromelyEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnFrameLoaded));
                    _window.RegisterEventHandler(CefEventKey.BeforeClose, new ChromelyEventHandler<BeforeCloseEventArgs>(CefEventKey.BeforeClose, OnBeforeClose));
                    CiTrace("Window", "Created");
                    var result = _window.Run(args);
                    CiTrace("RunResult", result.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            CiTrace("Application", "Done");
            return 0;
        }

        private static void OnBeforeClose(object sender, BeforeCloseEventArgs e)
        {
            CiTrace("OnBeforeClose", "called");
        }

        private static void OnFrameLoaded(object sender, FrameLoadEndEventArgs e)
        {
            var hWnd = Process.GetCurrentProcess().MainWindowHandle;
            CiTrace("NativeMainWindowHandle", hWnd.ToString());
        }


        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            _startupTimer.Stop();
            CiTrace("Content", "Loaded");
            CiTrace("StartupMs", _startupTimer.ElapsedMilliseconds.ToString());

            if (Debugger.IsAttached) return;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Environment.Exit(0);
            });
        }
    }

}

