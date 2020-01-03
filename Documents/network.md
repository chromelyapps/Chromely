
# Network Service

Chromely via CefGlue provides 2 different ways of Interprocess Communication (IPC) between the Renderer and the Browser.

- Generic Message Routing - more info @ [Generic Message Routing](https://github.com/chromelyapps/Chromely/wiki/Generic-Message-Routing).
- Ajax HTTP/XHR  -  more info @ [Custom Scheme Handling](https://github.com/chromelyapps/Chromely/wiki/Custom-Scheme-Handling).


These allow Chromely to receieve JavaScript requests initated by the Renderer, processed by the Browser (C#) and returned Json data response to the Renderer. 

Additionally via [Commands](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/RestfulService/CommandRoute.cs), Chromely provides a **fire-and-forget** scenarias - JavaScript requests are sent from the Renderer and processed by the Browser but no response returned to the Renderer.

## Configuring Network Scheme/Request Endpoint

The configuration of an IPC workflow involves:

1. Create a Controller class
2. Register an Action method in the Controller class
3. Register the Controller class
4. Register a scheme (http, custom, etc)
5. Create a JavaScript request in the Renderer html

##  1.  Create a Controller class

Every IPC workflow requires a Controller class. The class must inherit [ChromelyController](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/RestfulService/ChromelyController.cs).


````csharp
public class CustomController : ChromelyController
{
}
````

##  2.  Register an Action method in the Controller class

An action method can be registered in the custom controller constructor or as an attribute decorated method.

- Constructor type
````csharp
public DemoController()
{
	RegisterGetRequest("/democontroller/movies", GetMovies);
	RegisterPostRequest("/democontroller/movies", SaveMovies);
	RegisterCommand("/democontroller/showdevtools", ShowDevTools);
}

private ChromelyResponse GetMovies(ChromelyRequest request)
{
}


private ChromelyResponse SaveMovies(ChromelyRequest request)
{
}



public void ShowDevTools(IDictionary<string, string> queryParameters)
{
}

````

- Attribute decorated type
````csharp
[HttpGet(Route = "/democontroller/movies")]
public ChromelyResponse GetMovies(ChromelyRequest request)
{
}

[HttpPost(Route = "/democontroller/movies")]
public ChromelyResponse SaveMovies(ChromelyRequest request)
{
}

	  
[Command(Route = "/democontroller/showdevtools")]
public void ShowDevTools(IDictionary<string, string> queryParameters)
{
}
````



##  3.  Register the Controller class

The class CustomController must be registered. Registration can be done either by registering the Assembly where the class is created or registering via the Container

- Registering the assembly class can be done in 2 ways:

1.  Adding the fullpath of the assembly in the configuration config file 

````javascript
  "controllerAssemblies": [
    "Chromely.External.Controllers.dll"
  ],
````

2. Adding it in the configuration object.

````csharp

    var config = new DefaultConfiguration();
    config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
    config.ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");

````

- Register the Controller class via the Container:

````csharp

public class DemoChromelyApp : BasicChromelyApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(CustomController));

        }
    }

````
### 4. Register a scheme (http, custom, etc)

For details on registering a custom scheme plese see [Custom Http Registration](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_scheme_handlers.md). 

### 5. Create a JavaScript request in the Renderer html

To trigger an actual request, an event must must be set up in the HTML file.

A sample:

````javascript
function getMovies() {
        var request = {
            "method": "GET",
            "url": "/democontroller/movies",
            "parameters": null,
            "postData": null
        };
        window.cefQuery({
            request: JSON.stringify(request),
            onSuccess: function (response) {
        -               -- process response
            }, onFailure: function (err, msg) {
                console.log(err, msg);
            }
        });
}
````