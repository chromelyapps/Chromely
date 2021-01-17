
# Registering Custom Scheme Handlers

Interprocess Communication (IPC) in Chromely uses Restful model. It has a method (GET, POST..) and a resource has an identifier, which is a URI that uniquely identifies that resource. 

A fully qualified qualified resource will be in format:
scheme://host/resource_relative_path

A sample url:
http://chromely.com/democontroller/movies

where:
<pre>
scheme: http 
host: chromely.com
resource_relative_path : /democontroller/movies
</pre>

Registration of a custom scheme handler requires 2 steps:

1. Registration of a url scheme
2. Registration of a custom scheme handler factory

### 1. Registration of a url scheme

````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("custom-01", "local", string.Empty, string.Empty, UrlSchemeType.Custom, false),
            });
          
        }
    }
````

### 2. Registration of a custom scheme handler factory

A registered url scheme must be matched to custom scheme handler. If no custom handler is provided, Chromely uses the provided [default handler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Browser/Handlers/DefaultRequestSchemeHandler.cs).

Registering a custom scheme requires creating both a custom scheme handler and custom scheme handler factory. The factory and custom scheme handler are then registered with the IOC container.

Custom scheme handler factory:

````csharp
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppBuilder
            .Create()
            .UseApp<DemoApp>()
            .Build()
            .Run(args);
        }
    }

    public class DemoApp : ChromelyBasicApp
    {
        public override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton(typeof(CustomRequestSchemeHandlerFactory), typeof(CustomRequestSchemeHandlerFactory));
            services.AddSingleton(typeof(IChromelySchemeHandler), typeof(CustomSchemeHandler));

            RegisterControllerAssembly(services, typeof(DemoApp).Assembly);
        }
    }

    public class CustomSchemeHandler : IChromelySchemeHandler
    {
        public CustomSchemeHandler(CustomRequestSchemeHandlerFactory schemeHandlerFactory)
        {
            Name = "MyCustomRequestSchemeHamdler";
            // Scheme: myscheme
            // Host: custom - mapped to folder name containing resource files
            //http://custom.com/customcontroller/hello";
            Scheme = new UrlScheme("mycustomrequestcheme", "http", "custom.com", string.Empty, UrlSchemeType.LocalRquest);
            HandlerFactory = schemeHandlerFactory;
            IsCorsEnabled = true;
            IsSecure = false;
        }

        public string Name { get; set; }
        public UrlScheme Scheme { get; set; }

        // Needed for CefSharp
        public object Handler { get; set; }
        public object HandlerFactory { get; set; }
        public bool IsCorsEnabled { get; set; }
        public bool IsSecure { get; set; }
    }

    public class CustomRequestSchemeHandlerFactory : DefaultRequestSchemeHandlerFactory
    {
    }
````

Please see [Howto: Custom Request Scheme Handler](https://github.com/chromelyapps/Chromely/issues/247) for more.
