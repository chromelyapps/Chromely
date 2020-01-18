
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

You can register a url scheme either in config file or via C# code.

- Using config file

````javascript
  "urlSchemes": [
    {
      "name": "custom-01",
      "baseUrl": "",
      "scheme": "http",
      "host": "chromely.com",
      "urlSchemeType": "custom",
      "baseUrlStrict": false
    },
  ]
````
- Using C# code

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

A registered url scheme must be matched to custom scheme handler. If no custom handler is provided, Chromely uses the provided [default handler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/Browser/Handlers/CefGlueHttpSchemeHandler.cs).

Registering a custom scheme requires creating both a custom scheme handler and custom scheme handler factory. The factory is then registered with the IOC container.

Custom scheme handler:

````csharp
    public class CustomHttpSchemeHandler : CefResourceHandler
    {
    }
````

Custom scheme handler factory:

````csharp
    public class CustomHttpSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new CustomHttpSchemeHandler();
        }
    }
````

Handler factory registration:

````csharp
    public class DemoChromelyApp : BasicChromelyApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(IChromelySchemeHandlerFactory), "custom-01", typeof(CustomHttpSchemeHandlerFactory));
        }
    }
````

Notes:
- Interface [IChromelySchemeHandlerFactory](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelySchemeHandlerFactory.cs) is just a placeholder and should not be implemented.
- The name "custom-01" used in url scheme must match the key used in factory registration.
- Sample [CustomHttpSchemeHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/Browser/Handlers/CefGlueHttpSchemeHandler.cs) and [CustomHttpSchemeHandlerFactory](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/Browser/Handlers/CefGlueHttpSchemeHandlerFactory.cs).
