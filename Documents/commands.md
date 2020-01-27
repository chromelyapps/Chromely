
# Commands

There are two ways to communicate between Renderer and Browser.
- Action route
- Command route

An action route, is the normal way to do IPC. There is a request and a response. For more info on normal "action route" please see [Registering Custom Scheme Handlers](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_scheme_handlers.md). The second option is to use commands. Commands are fire-and-forget. There is a request but no response returned to the originating method.

A command must be registered and requires a URL scheme registarion and a related action method. 

### Register URl Scheme
 A command scheme can be registered in config file or configuration object.

- Using config file

````javascript
  "urlSchemes": [
      {
        "name": "default-command-http",
        "baseUrl": "",
        "scheme": "http",
        "host": "command.com",
        "urlSchemeType": "command",
        "baseUrlStrict": false
        }
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
                new UrlScheme("default-command-http", "http", "command.com", string.Empty UrlSchemeType.Command, false),
            });
          
        }
    }
````

### Add Command Methods To Controller 

Command methods can be added to Controller class in 2 ways.

- Constructor type
````csharp
public DemoController()
{
	RegisterCommand("/democontroller/showdevtools", ShowDevTools);
}

public void ShowDevTools(IDictionary<string, string> queryParameters)
{
}

````  

- Attribute decorated type
````csharp
 
[Command(Route = "/democontroller/showdevtools")]
public void ShowDevTools(IDictionary<string, string> queryParameters)
{
}
````

### Create a JavaScript request in the Renderer html

To trigger an actual request, an event must must be set up in the HTML file.

A sample:

````html
   <a href="http://command.com/democontroller/showdevtools">Show DevTools</a>
````