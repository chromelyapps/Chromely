namespace ChromelyAngularCefSharp
{
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.CefSharp.Winapi.ChromeHost;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.Winapi;
    using System;
    using System.Reflection;
    using WinApi.Windows;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string startUrl = string.Format("file:///{0}dist/index.html", appDirectory);

                ChromelyConfiguration config = ChromelyConfiguration
                                              .Create()
                                              .WithCefAppArgs(args)
                                              .WithCefHostSize(1200, 900)
                                              .WithCefLogFile("logs\\chromely.cef_new.log")
                                              .WithCefStartUrl(startUrl)
                                              .WithCefLogSeverity(LogSeverity.Info)
                                              .UseDefaultLogger("logs\\chromely_new.log", true)
                                              .RegisterSchemeHandler("http", "chromely.com", new CefSharpSchemeHandlerFactory())
                                              .RegisterJsHandler("boundControllerAsync", new CefSharpBoundObject(), null, true);

                var factory = WinapiHostFactory.Init("chromely.ico");
                using (var window = factory.CreateWindow(() => new CefSharpBrowserHost(config),
                    "chromely", constructionParams: new FrameWindowConstructionParams()))
                {
                    // Register external url schems
                    window.RegisterExternalUrlScheme(new UrlScheme("https://github.com/mattkol/Chromely", true));

                    // Register service assemblies
                    window.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

                    // Note ensure external is valid folder.
                    // Uncomment to refgister external restful service dlls
                    string serviceAssemblyFile = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    window.RegisterServiceAssembly(serviceAssemblyFile);

                    // Scan assemblies for Controller routes 
                    window.ScanAssemblies();

                    window.SetSize(config.CefHostWidth, config.CefHostHeight);
                    window.CenterToScreen();
                    window.Show();
                    return new EventLoop().Run(window);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return 0;
        }
    }
}
