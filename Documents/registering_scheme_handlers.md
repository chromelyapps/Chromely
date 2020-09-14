
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

A registered url scheme must be matched to custom scheme handler. If no custom handler is provided, Chromely uses the provided [default handler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/Browser/Handlers/CefGlueHttpSchemeHandler.cs).

Handler factory registration:

````csharp
    public class CusomChromelyApp : ChromelyBasicApp
    {
          public override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IChromelySchemeHandler, CustomRequestSchemeHandler>();
        }
    }

    public class CustomRequestSchemeHandler : IChromelySchemeHandler
    {
    }
````

Note the detail:
- Interface [IChromelySchemeHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelySchemeHandler.cs)


