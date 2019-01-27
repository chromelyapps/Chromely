# CefSharp App Demo Templates

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

      using (var window = new CefSharpBrowserWindow(config))
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

                var startUrl = "local://app/chromely.html";
                var config = ChromelyConfiguration
                                .Create()
                                .WithHostMode(WindowState.Normal)
                                .WithHostTitle("chromely")
                                .WithHostIconFile("chromely.ico")
                                .WithAppArgs(args)
                                .WithHostSize(1200, 700)
                                .WithStartUrl(startUrl)
                                .WithLogSeverity(Core.Infrastructure.LogSeverity.Info)
                                .UseDefaultLogger()
                                .UseDefaultResourceSchemeHandler("local", string.Empty)
                                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                                .UseDefautJsHandler("boundControllerAsync", true)
                                .RegisterEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadStart, OnWebBrowserFrameLoadStart)
                                .RegisterEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnWebBrowserFrameLoadEnd)
                                .RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage)
                                .RegisterEventHandler<StatusMessageEventArgs>(CefEventKey.StatusMessage, OnWebBrowserStatusMessage);
 
                using (var window = new CefSharpBrowserWindow(config))
                {
                    // Register external url schemes
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/chromelyapps/Chromely", true));

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

        private static void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
        }

        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadStart(object sender, FrameLoadStartEventArgs e)
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

                var startUrl = "local://app/chromely.html";
                var config = ChromelyConfiguration
                                .Create()
                                .WithHostMode(WindowState.Normal)
                                .WithHostTitle("chromely")
                                .WithHostIconFile("chromely.ico")
                                .WithAppArgs(args)
                                .WithHostSize(1200, 700)
                                .WithStartUrl(startUrl)
                                .WithLogSeverity(Core.Infrastructure.LogSeverity.Info)
                                .UseDefaultLogger()
                                .UseDefaultResourceSchemeHandler("local", string.Empty)
                                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                                .UseDefautJsHandler("boundControllerAsync", true)

                using (var window = new CefSharpBrowserWindow(config))
                {
                    window.RegisterEventHandler<FrameLoadStartEventArgs>(CefEventKey.FrameLoadStart, OnWebBrowserFrameLoadStart);
                    window.RegisterEventHandler<FrameLoadEndEventArgs>(CefEventKey.FrameLoadEnd, OnWebBrowserFrameLoadEnd);
                    window.RegisterEventHandler<ConsoleMessageEventArgs>(CefEventKey.ConsoleMessage, OnWebBrowserConsoleMessage);
                    window.RegisterEventHandler<StatusMessageEventArgs>(CefEventKey.StatusMessage, OnWebBrowserStatusMessage);

                    // Register external url schemes
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/chromelyapps/Chromely", true));

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

        private static void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
        }

        private static void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }

        private static void OnWebBrowserFrameLoadStart(object sender, FrameLoadStartEventArgs e)
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
                                .WithLogSeverity(Core.Infrastructure.LogSeverity.Info)
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

                                .RegisterJsHandler("boundControllerAsync", new CustomJsHandler1(), null, true)
                                .RegisterJsHandler("boundControllerAsync2", new CustomJsHandler2(), null, true);

                using (var window = new CefSharpBrowserWindow(config))
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

    internal class CustomContainer : IChromelyContainer
    {
    }

    internal class CustomLogger : IChromelyLogger
    {
    }
    
    internal class CustomJsHandler2
    {
    }

    internal class CustomJsHandler1
    {
    }

    internal class CustomChromelySchemeHandler1 : ChromelySchemeHandler
    {
    }

    internal class CustomChromelySchemeHandler2 : ChromelySchemeHandler
    {
    }

    internal class CustomContextMenuHandler
    {
    }

    internal class CustomDisplayHandler
    {
    }

    internal class CustomRequestHandler
    {
    }

    internal class CustomLoadHandler
    {
    }

    internal class CustomLifeSpanHandler
    {
    }

    internal class CustomAssembly : Assembly
    {
    }
    
````
