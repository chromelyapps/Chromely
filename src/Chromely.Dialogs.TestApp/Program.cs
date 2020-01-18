using System;
using System.IO;
using System.Runtime.InteropServices;
using Chromely.CefGlue.Browser.EventParams;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Chromely.Windows;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;

namespace Chromely.Dialogs.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sample showing Chromely.Dialogs");
            Console.WriteLine();

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

            var builder = AppBuilder.Create();
            builder = builder.UseApp<ChromelyDialogsTestApp>();
            builder = builder.UseConfiguration<DefaultConfiguration>(config);
            builder = builder.Build();
            builder.Run(args);
            
            Console.WriteLine("Sample done.");
        }
        
        public class ChromelyDialogsTestApp : BasicChromelyApp
        {
            public override void Initialize(IChromelyContainer container, IChromelyAppSettings appSettings, IChromelyConfiguration config,
                IChromelyLogger chromelyLogger)
            {
                base.Initialize(container, appSettings, config, chromelyLogger);
            }
        }
        
        
    }
}
