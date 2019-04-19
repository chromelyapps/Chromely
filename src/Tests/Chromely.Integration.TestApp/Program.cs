using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Chromely.CefGlue;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Helpers;

namespace Chromely.Integration.TestApp
{
    /// <summary>
    /// This is a minimal chromely application
    /// to be used during integration tests.
    /// It will emit console outputs starting
    /// with "CI-TRACE:" which are checked
    /// in the test run - so don't remove them.
    /// </summary>
    internal static class Program
    {
        private const string TraceSignature = "CI-TRACE:";

        private static void CiTrace(string key, string value)
        {
            Console.WriteLine($"{TraceSignature} {key}={value}");
        }

        private static Stopwatch startupTimer;

        private static int Main(string[] args)
        {
            CiTrace("Application", "Started");
            // measure startup time (maybe including CEF download)
            startupTimer = new Stopwatch();
            startupTimer.Start();

            var core = typeof(ChromelyConfiguration).Assembly;
            CiTrace("Chromely.Core", core.GetName().Version.ToString());
            CiTrace("Platform", ChromelyRuntime.Platform.ToString());

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            CiTrace("AppDirectory", appDirectory);
            var startUrl = $"file:///{appDirectory}/index.html";

            var config = ChromelyConfiguration
                .Create()
                .WithLoadingCefBinariesIfNotFound(true)
                .RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage)
                .WithAppArgs(args)
                .WithHostSize(1000, 600)
                .WithStartUrl(startUrl);
            CiTrace("Configuration", "Created");

            try
            {
                using (var window = ChromelyWindow.Create(config))
                {
                    CiTrace("Window", "Created");
                    var result = window.Run(args);
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

        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            startupTimer.Stop();
            CiTrace("Content", "Loaded");
            CiTrace("StartupMs", startupTimer.ElapsedMilliseconds.ToString());

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Environment.Exit(0);
            });
        }
    }

}

