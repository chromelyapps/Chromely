
# Registering Url Schemes

Chromely centralizes and control all url requests via a url scheme. A registered url determines what Chromely does with it. An unregistered scheme will follow default behavior or a behavior pre-programmed by the developer. 

There are [4 types](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Infrastructure/UrlSchemeType.cs) of schemes
- Resource
- Custom (usually http used for IPC)
- Command
- External

 Every scheme is registered via a [UrlScheme](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Infrastructure/UrlScheme.cs) object.

| __Property__ | __Description__ | __Comment__ |
|-------------|------------|------------|
| Name         | The name identifies the scheme. | This is required for Custom schemes. The name matches the key used to register a custom handler in IOC container. |       
| Scheme         | The scheme. | Sample schemes are http, https, cmd etc.    |
| Host         | The host. | Sample hosts are chromely.com, me.com, command.com.    |
| BaseUrl         | The base url. | With base url, the scheme and host can be deduced. Base ul is also required if a BaseUrlStrict is set to true. Sample base urls are https://github, http://chromely.com  |
| UrlSchemeType         | The scheme type. | Types are Resource, Custom, Command, External    |
| BaseUrlStrict         | The BaseUrlStrict determines the strictness of the BaseUrl. This is mostly used for External url schemes. | If the BaseUrl is https://github.com/chromelyapps/Chromely and BaseUrlStrict is "true" only url from the base and beyond will be processed - https://github.com/chromelyapps/Chromely and https://github.com/chromelyapps/Chromely/external will be valid. https://github.com will be ignored.    |

### Registration of a url scheme

You can register a url scheme either in config file or via C# code.

````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
            UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false),
                new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false),
                new UrlScheme("default-custom-http", "http", "chromely.com", string.Empty, UrlSchemeType.Custom, false),
                new UrlScheme("default-command-http", "http", "command.com", string.Empty, UrlSchemeType.Command, false),
                new UrlScheme("chromely-site", string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.External, true)
            });
          
        }
    }
````

### Resource Url Scheme
A resource url scheme is needed to register a custom resource handler. For details please see [Registering Resource Handlers](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_resource_handlers.md).

### Custom Url Scheme
A custom url scheme is needed to register a custom http scheme handler. For details please see [Registering Custom Scheme Handlers](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_scheme_handlers.md).

### Command Url Scheme
A command url scheme is needed to register a command. A command is a fire-and-forget event trigger. It is different from an action route because no response/data is returned. For details please see [Registering Resource Handlers](https://github.com/chromelyapps/Chromely/blob/master/Documents/commands.md).

### External Url Scheme
An external url scheme is needed to register an external url. When an external url is triggered, it is opened in the default OS browser.

For example the external url registered "https://github.com/chromelyapps/Chromely" above will be opened in the OS default browser: Chrome, FireFox, Edge etc.