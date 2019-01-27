# CefGlue App Demo Templates

##### Table of Contents  
[Simple Usage](#simple)  
[With Events](#withevents)  
[With Custom Handlers](#customhandlers)  

<a name="simple"/>

## Simple Usage


````csharp
class Program
{
   static int Main(string[] args)
   {
      var startUrl = "https://google.com";

      var config = ChromelyConfiguration
                      .Create()
                      .WithHostMode(WindowState.Normal, true)
                      .WithHostTitle("chromely")
                      .WithHostIconFile("chromely.ico")
                      .WitAppArgs(args)
                      .WithHostSize(1000, 600)
                      .WithStartUrl(startUrl);

      using (var window = new CefGlueBrowserWindow(config))
      {
         return window.Run(args);
      }
  }
}
- Creates Chromely window of size 1000 x 600 pixels.
- Sets the window title to "chromely"
- Sets start url to "https://google.com"
- Centers the window 
````

<a name="withevents"/>

## With Events

#### Option 1
````csharp
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();
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
                                .WithHostMode(WindowState.Fullscreen)
                                .WithHostTitle("chromely")
                                .WithHostIconFile("chromely.ico")
                                .WithAppArgs(args)
                                .WithHostSize(1200, 700)
                                .WithLogFile("logs\\chromely.cef_new.log")
                                .WithStartUrl(startUrl)
                                .WithLogSeverity(LogSeverity.Info)
                                .UseDefaultLogger("logs\\chromely_new.log")
                                .UseDefaultResourceSchemeHandler("local", string.Empty)
                                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                                .RegisterEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadStart, OnWebBrowserFrameLoadStart)
                                .RegisterEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnWebBrowserFrameLoadEnd)
                                .RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage)
                                .RegisterEventHandler<StatusMessageEventArgs>(CefEventKey.StatusMessage, OnWebBrowserStatusMessage)
                                .UseDefaultWebsocketHandler(string.Empty, 8181, true);

                using (var window = new CefGlueBrowserWindow(config))
                {
                    // Register external url schems
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/mattkol/Chromely", true));

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

            return 0;
        }

        private static void OnWebBrowserFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }

        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        private static void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
        }
    }
````

#### Option 2

````csharp
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();
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
                                .WithHostMode(WindowState.Fullscreen)
                                .WithHostTitle("chromely")
                                .WithHostIconFile("chromely.ico")
                                .WithAppArgs(args)
                                .WithHostSize(1200, 700)
                                .WithLogFile("logs\\chromely.cef_new.log")
                                .WithStartUrl(startUrl)
                                .WithLogSeverity(LogSeverity.Info)
                                .UseDefaultLogger("logs\\chromely_new.log")
                                .UseDefaultResourceSchemeHandler("local", string.Empty)
                                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                                .UseDefaultWebsocketHandler(string.Empty, 8181, true);
  
                using (var window = new CefGlueBrowserWindow(config))
                {
                    window.RegisterEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadStart, OnWebBrowserFrameLoadStart);
                    window.RegisterEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnWebBrowserFrameLoadEnd);
                    window.RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage);
                    window.RegisterEventHandler<StatusMessageEventArgs>(CefEventKey.StatusMessage, OnWebBrowserStatusMessage);

                    // Register external url schems
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/mattkol/Chromely", true));

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

            return 0;
        }

        private static void OnWebBrowserFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }

        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        private static void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
        }
    }
````

<a name="customhandlers"/>

## With Custom Handlers

````csharp
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                var startUrl = "local://app/chromely.html";

                var config = ChromelyConfiguration
                                .Create(new CustomContainer())
                                .WithHostMode(WindowState.Maximize)
                                .WithHostTitle("my app name")
                                .WithHostIconFile("myappicon.ico")
                                .WithAppArgs(args)
                                .WithHostSize(1200, 700)
                                .WithAppArgs(args)
                                .WithCommandLineArg("no-sandbox", "1")
                                .WithCommandLineArg("device-scale-factor", "1")
                                .WithLogFile("logs\\myappcef.log")
                                .WithLogger(new CustomLogger())
                                .WithStartUrl(startUrl)
                                .WithLogSeverity(LogSeverity.Info)
                                .WithDebuggingMode(false)
                                .WithLocale("fr-FR")
                                .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, "full_path_to_subprocess")
                                .WithCustomSetting(CefSettingKeys.UserAgent, "custom-user-agent")

                                .RegisterServiceAssembly("full path to dll")
                                .RegisterServiceAssembly(new CustomAssembly())
                                .RegisterServiceAssembly(Assembly.GetExecutingAssembly())
                                .RegisterCustomrUrlScheme(new UrlScheme("https://github.com/chromelyapps/Chromely", true))
                                .RegisterCustomHandler(CefHandlerKey.LifeSpanHandler, typeof(CustomLifeSpanHandler))
                                .RegisterCustomHandler(CefHandlerKey.LoadHandler, typeof(CustomLoadHandler))
                                .RegisterCustomHandler(CefHandlerKey.RequestHandler, typeof(CustomRequestHandler))
                                .RegisterCustomHandler(CefHandlerKey.DisplayHandler, typeof(CustomDisplayHandler))
                                .RegisterCustomHandler(CefHandlerKey.ContextMenuHandler, typeof(CustomContextMenuHandler))

                                .RegisterCustomrUrlScheme("http", "chromely.com")
                                .RegisterCustomrUrlScheme("test", "test.com")
                                .RegisterExternalUrlScheme("https", "google.com")
                                .RegisterExternalUrlScheme("https", "https://github.com/chromelyapps/Chromely")
                                .RegisterSchemeHandler("local", string.Empty, new CustomChromelySchemeHandler1())
                                .RegisterSchemeHandler("http", "chromely.com", new CustomChromelySchemeHandler2())

                               .RegisterMessageRouterHandler(new CustomMessageRouter1())
                               .RegisterMessageRouterHandler(new CustomMessageRouter2())

                              .RegisterWebsocketHandler(address: string.Empty, port: 8181, onloadstartserver: true, sockeHandler: new CustomWebsocketHandler());

                using (var window = new CefGlueBrowserWindow(config))
                {
                    // Scan assemblies for Controller routes 
                    window.ScanAssemblies();
                    return window.Run(args);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return 0;
        }
    }

    class CustomContainer : IChromelyContainer
    {
    }

    class CustomLogger : IChromelyLogger
    {
    }
    
    class CustomMessageRouter2
    {
    }

    class CustomMessageRouter1
    {
    }

    class CustomChromelySchemeHandler1 : ChromelySchemeHandler
    {
    }

    class CustomChromelySchemeHandler2 : ChromelySchemeHandler
    {
    }

    class CustomContextMenuHandler
    {
    }

    class CustomDisplayHandler
    {
    }

    class CustomRequestHandler
    {
    }

    class CustomLoadHandler
    {
    }

    class CustomLifeSpanHandler
    {
    }

    class CustomAssembly : Assembly
    {
    }
    
    class CustomWebsocketHandler : IChromelyWebsocketHandler
    {
    }
    
````
