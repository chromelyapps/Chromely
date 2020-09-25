
# Registering Resource Handlers

Resource files (html, css, javasctipt etc) can be loaded in 3 different ways:

- real website (e.g http://google.com)
- file protocol (e.g file:///{appDirectory}app/chromely.html)
- local resource (e.g local://app/chromely.html)

The preferred option is via local resources. For local resource processing a custom (or default) handler must be registered.

Registration of a resource handler requires 2 steps:

1. Registration of a url scheme
2. Registration of a custom resource scheme handler factory

### 1. Registration of a url scheme

You can register a url scheme either in config file or via C# code.

Sample 1 - usual file resources
````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("custom-01", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false),
            });
          
        }
    }
````

Sample 2 - embedded file resources.
````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                var assemblyOptions = new AssemblyOptions("embeddedAssemblyName.dll", null, "app");
                UrlSchemes.Add(new UrlScheme(DefaultSchemeName.ASSEMBLYRESOURCE, "assembly", "app", string.Empty,   UrlSchemeType.AssemblyResource, false, assemblyOptions));
            });
          
        }
    }
 where:
    Name of assembly: embeddedAssemblyName.dll
    Folder where resources are: app
````

### 2. Registration of a custom resource handler factory

A registered url scheme must be matched to custom resource handler. If no resource handler is provided, Chromely uses the provided [default handler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Browser/Handlers/DefaultResourceSchemeHandler.cs).

Registering a custom scheme requires creating both a custom scheme handler and custom scheme handler factory. The factory is then registered with the IOC container.

Custom scheme handler factory:

````csharp
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var config = DefaultConfiguration.CreateForRuntimePlatform();

            // Scheme: myscheme, Host: custom [Pre-registered]
            config.StartUrl = "myscheme://custom/index.html";

            AppBuilder
            .Create()
            .UseConfig<DefaultConfiguration>(config)
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
            services.AddSingleton(typeof(IChromelySchemeHandler), typeof(CustomResourceSchemeHandler));
        }
    }

    public class CustomResourceSchemeHandler : IChromelySchemeHandler
    {
        public CustomResourceSchemeHandler()
        {
            Name = "MyCustomResourceSchemeHamdler";
            // Scheme: myscheme
            // Host: custom - mapped to folder name containing resource files
            Scheme = new UrlScheme("mycustomresourcescheme", "myscheme", "custom", string.Empty, UrlSchemeType.Resource, false);
            HandlerFactory = new CustomResourceSchemeHandlerFactory();
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

    public class CustomResourceSchemeHandlerFactory : DefaultResourceSchemeHandlerFactory
    {
    }
````

Please see [Howto: Custom Resource Scheme Handler](https://github.com/chromelyapps/Chromely/issues/246) for more.
