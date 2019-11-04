using System;
using System.IO;
using System.Runtime.InteropServices;
using Chromely.CefGlue;
using Chromely.Core;
using Chromely.Core.Host;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;
using Xilium.CefGlue;

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

            // ensure CEF runtime files are present
            Console.WriteLine("Check CEF framework is installed in the correct version");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(path);

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

            var config = ChromelyConfiguration
                .Create()
                .WithLoadingCefBinariesIfNotFound(true)
                .WithSilentCefBinariesLoading(true)
                .WithHostMode(WindowState.Normal)
                .WithHostTitle(options.Title)
                .WithAppArgs(args)
                .WithHostBounds(1000, 600)
                .RegisterCustomerUrlScheme("http", "localhost")
                .WithDebuggingMode(true)
                .WithStartUrl(startUrl);

            using (var window = ChromelyWindow.Create(config))
            {
                ChromelyDialogs.Init(window);
                var exitCode = window.Run(args);
                if (exitCode != 0)
                {
                    Console.WriteLine("Failed to start chromely frontend: code " + exitCode);
                }
            }
            
            Console.WriteLine("Sample done.");
        }
    }
}
