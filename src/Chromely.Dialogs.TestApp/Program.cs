using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Chromely.Native;
using Chromely.Windows;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;

namespace Chromely.Dialogs.TestApp
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Sample showing Chromely.Dialogs");
            Console.WriteLine();
            
            ChromelyDialogs.Init(null);
            //ChromelyDialogs.MessageBox("Test");

            Console.WriteLine($"Running on {RuntimeEnvironment.GetRuntimeDirectory()}, CLR {RuntimeEnvironment.GetSystemVersion()}");
            Console.WriteLine();

            // Starting stonehenge backend
            Console.WriteLine("Starting stonehenge backend");
            var provider = StonehengeResourceLoader
                .CreateDefaultLoader(new VueResourceProvider());
            var options = new StonehengeHostOptions
            {
                Title = "Chromely.Dialogs",
                StartPage = "messages",
                ServerPushMode = ServerPushModes.LongPolling,
                PollIntervalMs = 5000
            };
            var host = new KestrelHost(provider, options);
            if (!host.Start("localhost", 32000))
            {
                Console.WriteLine("Failed to start stonehenge server");
            }

            // Starting chromely frontend
            Console.WriteLine("Starting chromely frontend");
            var startUrl = host.BaseUrl;

            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.CefDownloadOptions = new CefDownloadOptions(true, false);
            config.WindowOptions.Position = new WindowPosition(1, 2);
            config.WindowOptions.Size = new WindowSize(1000, 600);
            config.StartUrl = startUrl;
            config.DebuggingMode = true;

            Console.WriteLine($"Main PID={Process.GetCurrentProcess().Id}, THREAD={Thread.CurrentThread.ManagedThreadId}");
                
            var builder = AppBuilder.Create();
            builder = builder.UseApp<ChromelyDialogsTestApp>();
            builder = builder.UseConfiguration<DefaultConfiguration>(config);
            builder = builder.Build();

            builder.Run(args);
            
            Console.WriteLine("Sample done.");
        }
        
        private static void OnFrameStartLoading(object? sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine($"OnFrameStartLoading PID={Process.GetCurrentProcess().Id}, THREAD={Thread.CurrentThread.ManagedThreadId}");
            //ChromelyDialogs.MessageBox("Test");
        }

        private static void OnFrameLoaded(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine($"OnFrameLoaded PID={Process.GetCurrentProcess().Id}, THREAD={Thread.CurrentThread.ManagedThreadId}");
            
            //ChromelyDialogs.Init(null);
            //ChromelyDialogs.MessageBox("Test");
        }

        public class ChromelyDialogsTestApp : BasicChromelyApp
        {
            public override void RegisterEvents(IChromelyContainer container)
            {
                EnsureContainerValid(container);

                RegisterEventHandler(container, CefEventKey.FrameLoadStart, new ChromelyEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadStart, Program.OnFrameStartLoading));
                RegisterEventHandler(container, CefEventKey.FrameLoadEnd, new ChromelyEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, Program.OnFrameLoaded));
                
                //ChromelyDialogs.Init(null);
            }

            private void RegisterEventHandler<T>(IChromelyContainer container, CefEventKey key, ChromelyEventHandler<T> handler)
            {
                var service = CefEventHandlerTypes.GetHandlerType(key);
                container.RegisterInstance(service, handler.Key, handler);
            }

            public override void Initialize(IChromelyContainer container, IChromelyAppSettings appSettings, IChromelyConfiguration config,
                IChromelyLogger chromelyLogger)
            {
                Console.WriteLine($"Initialize PID={Process.GetCurrentProcess().Id}, THREAD={Thread.CurrentThread.ManagedThreadId}");
                base.Initialize(container, appSettings, config, chromelyLogger);
                //ChromelyDialogs.Init(null);
            }

            public override IChromelyWindow CreateWindow()
            {
                var wnd = base.CreateWindow();
                //ChromelyDialogs.Init(wnd);
                return wnd;
            }
            
        }

    }
}
