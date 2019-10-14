using System;
using System.Reflection;
using Chromely.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;

namespace CrossPlatDemo
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                /*
                * Start url (load html) options:
                */

                // Options 1 - real standard urls 
                // string startUrl = "https://google.com";

                // Options 2 - using local resource file handling with default/custom local scheme handler 
                // Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
                //            or register new resource scheme handler - RegisterSchemeHandler("local", string.Empty,  new CustomResourceHandler())
                var startUrl = "local://app/chromely.html";

                // Options 3 - using file protocol - using default/custom scheme handler for Ajax/Http requests
                // Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
                //            or register new resource handler - RegisterSchemeHandler("local", string.Empty,  new CustomResourceHandler())
                // Requires - (sample) UseDefaultHttpSchemeHandler("http", "chromely.com")
                //            or register new http scheme handler - RegisterSchemeHandler("http", "test.com",  new CustomHttpHandler())
                // var startUrl = $"file:///{appDirectory}app/chromely.html";

                var config = ChromelyConfiguration
                                             .Create()
                                             .WithHostMode(WindowState.Normal)
                                             .WithHostTitle("chromely")
                                             //.WithHostIconFile("chromely.ico")
                                             .WithHostIconFile("chromely_gray.png")
                                             .WithAppArgs(args)
                                             .WithHostBounds(1200, 700)
                                             .WithLogFile("logs\\chromely.cef_new.log")
                                             .WithStartUrl(startUrl)
                                             .WithLogSeverity(LogSeverity.Info)
                                             .UseDefaultLogger("logs\\chromely_new.log")
                                             .UseDefaultResourceSchemeHandler("local", string.Empty)
                                             .UseDefaultHttpSchemeHandler("http", "chromely.com");

                using (var window = new CefGlueWindow(config))
                {
                    // Register external url schemes
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/chromelyapps/Chromely", UrlSchemeType.External));

                    /*
                     * Register service assemblies
                     * Uncomment relevant part to register assemblies
                     */

                    // 1. Register current/local assembly:
                    window.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

                    // 2. Register external assembly with file name:
                    var externalAssemblyFile = System.IO.Path.Combine(appDirectory, "Chromely.Service.Demo.dll");
                    window.RegisterServiceAssembly(externalAssemblyFile);

                    // 3. Register external assemblies with list of filenames:
                    // string serviceAssemblyFile1 = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    // List<string> filenames = new List<string>();
                    // filenames.Add(serviceAssemblyFile1);
                    // app.RegisterServiceAssemblies(filenames);

                    // 4. Register external assemblies directory:
                    // var serviceAssembliesFolder = @"C:\ChromelyDlls";
                    // window.RegisterServiceAssemblies(serviceAssembliesFolder);

                    // Scan assemblies for Controller routes 
                    window.ScanAssemblies();

                    return window.Run(args);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
            finally
            {
                System.Environment.Exit(0);
            }

            return 0;
        }
    }
}