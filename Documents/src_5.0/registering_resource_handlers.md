
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

- Using config file

````javascript
  "urlSchemes": [
    {
      "name": "custom-01",
      "baseUrl": "",
      "scheme": "local",
      "host": "",
      "urlSchemeType": "resource",
      "baseUrlStrict": false
    },
  ]
````
- Using C# code

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
    Name of assembley: embeddedAssemblyName.dll
    Folder where resources are: app
````

### 2. Registration of a custom resource handler factory

A registered url scheme must be matched to custom resource handler. If no resource handler is provided, Chromely uses the provided [default handler](https://github.com/chromelyapps/Chromely/blob/master/src_5.0/Chromely.CefGlue/Browser/Handlers/CefGlueResourceSchemeHandler.cs).

Registering a custom scheme requires creating both a custom scheme handler and custom scheme handler factory. The factory is then registered with the IOC container.

Custom scheme handler:

````csharp
    public class CustomResourceSchemeHandler : CefResourceHandler
    {
    }
````

Custom scheme handler factory:

````csharp
    public class CustomResourceSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new CustomResourceSchemeHandler();
        }
    }
````

Handler factory registration:

````csharp
    public class DemoChromelyApp : ChromelyBasicApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(IChromelyResourceHandlerFactory), "custom-01", typeof(CustomResourceSchemeHandlerFactory));
        }
    }
````

Notes:
- Interface [IChromelyResourceHandlerFactory](https://github.com/chromelyapps/Chromely/blob/master/src_5.0/Chromely.Core/IChromelyResourceHandlerFactory.cs) is just a placeholder and should not be implemented.
- The name "custom-01" used in url scheme must match the key used in factory registration.
- Sample [CustomResourceSchemeHandler](https://github.com/chromelyapps/Chromely/blob/master/src_5.0/Chromely.CefGlue/Browser/Handlers/CefGlueResourceSchemeHandler.cs) and [CustomResourceSchemeHandlerFactory](https://github.com/chromelyapps/Chromely/blob/master/src_5.0/Chromely.CefGlue/Browser/Handlers/CefGlueResourceSchemeHandlerFactory.cs).

