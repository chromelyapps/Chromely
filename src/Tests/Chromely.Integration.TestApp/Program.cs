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
using Chromely.Core.Configuration;
using Chromely.Core.Helpers;

namespace Chromely.Integration.TestApp
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

            var core = typeof(IChromelyConfiguration).Assembly;
            CiTrace("Chromely.Core", core.GetName().Version.ToString());
            CiTrace("Platform", ChromelyRuntime.Platform.ToString());

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            CiTrace("AppDirectory", appDirectory);
            var startUrl = $"file:///{appDirectory}/index.html";

            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.CefDownloadOptions = new CefDownloadOptions(true, false);
            config.WindowOptions.Position = new WindowPosition(1, 2);
            config.WindowOptions.Size = new WindowSize(1000, 600);
            config.StartUrl = startUrl;
            config.DebuggingMode = true;
            config.WindowOptions.RelativePathToIconFile = "chromely.ico";

            CiTrace("Configuration", "Created");

            try
            {
                var builder = AppBuilder.Create();
                builder = builder.UseApp<TestApp>();
                builder = builder.UseConfiguration<DefaultConfiguration>(config);
                builder = builder.Build();
                builder.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            CiTrace("Application", "Done");
            return 0;
        }

        internal static void OnBeforeClose(object sender, BeforeCloseEventArgs e)
        {
            CiTrace("OnBeforeClose", "called");
        }

        internal static void OnFrameLoaded(object sender, FrameLoadEndEventArgs e)
        {
            var hWnd = Process.GetCurrentProcess().MainWindowHandle;
            CiTrace("NativeMainWindowHandle", hWnd.ToString());
        }


        internal static void OnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            _startupTimer.Stop();
            CiTrace("Content", "Loaded");
            CiTrace("StartupMs", _startupTimer.ElapsedMilliseconds.ToString());

            if (Debugger.IsAttached) return;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                //Environment.Exit(0);
            });
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class TestApp : ChromelyEventedApp
    {
        protected override void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs eventArgs)
        {
            Program.OnFrameLoaded(sender, eventArgs);
        }

        protected override void OnConsoleMessage(object sender, ConsoleMessageEventArgs eventArgs)
        {
            Program.OnConsoleMessage(sender, eventArgs);
        }

        protected override void OnBeforeClose(object sender, BeforeCloseEventArgs eventArgs)
        {
            Program.OnBeforeClose(sender, eventArgs);
        }
    }
    
}

