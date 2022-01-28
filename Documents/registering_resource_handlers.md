
# Registering Resource Handlers

Resource files (html, css, javasctipt etc) can be loaded in 3 different ways:

- real website (e.g http://google.com)
- file protocol (e.g file:///{appDirectory}app/chromely.html)
- local resource (e.g local://app/chromely.html)

The preferred option is via local resources. 

Registration of a custom resource handler requires 2 steps:

1. Registration of a url scheme
2. Registration of a custom resource scheme handler factory

- A default resource handler will be pre-registered for the developer and the developer can use the pre-register scheme handler - or register a new one using **"1. Registration of a url scheme"** below. 

    A pre-registered url scheme -  
    ````
    StartUrl = "local://app/index.html";
    ````
    https://github.com/chromelyapps/Chromely/blob/960f8ac1c97c9fd73591bd2f0828d4cee5bf5900/src/Chromely.Core/Configuration/DefaultConfiguration.cs#L71
    https://github.com/chromelyapps/Chromely/blob/960f8ac1c97c9fd73591bd2f0828d4cee5bf5900/src/Chromely.Core/Configuration/DefaultConfiguration.cs#L83
- If you are using a custom resource handler, it must be registred using **"1. Registration of a url scheme"** and **"2. Registration of a custom resource handler factory"** below.

### 1. Registration of a url scheme

You can register a url scheme either in config file or via C# code.

Sample 1 - using local file resources
````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("custom-01", "local", string.Empty, string.Empty, UrlSchemeType.LocalResource, false),
            });
          
        }
    }
````

Sample 2 - using host to folder mapping ("http://img" to "E:\img" map)
````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("custom-02", "http", "img", "E:\img", string.Empty, UrlSchemeType.FolderResource, false),
            });
          
        }
    }
````

Sample 3 - embedded file resources.
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

Registering a custom scheme requires creating both a custom scheme handler and custom scheme handler factory. The factory and custom scheme handler are then registered with the IOC container.


Custom scheme handler factory:

````csharp
var config = DefaultConfiguration.CreateForRuntimePlatform();

// Scheme: myscheme, Host: custom [Pre-registered]
config.StartUrl = "myscheme://custom/index.html";

ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<DefaultConfiguration>(config)
    .UseApp<DemoApp>()
    .Build()
    .Run();

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
