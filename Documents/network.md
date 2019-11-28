
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
2. Register the Controller class
3. Register a scheme (http, custom, etc)
4. Create a JavaScript request in the Renderer html

##  Create a Controller class

Every IPC workflow requires a Controller class. The class must inherit [ChromelyController](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/RestfulService/ChromelyController.cs).


````csharp
public class CustomController : ChromelyController
{
}
````

##  Register an Action method in the Controller class




##  Register the Controller class

The class CustomController must be registered. Registration can be done either by registering the Assembly where the class is created or registering via the Container

Registering the assembly class can be done in 2 ways:

- Adding the fullpath of the assembly in the configuration config file 

````javascript
  "controllerAssemblies": [
    "Chromely.External.Controllers.dll"
  ],
````

- Adding it in the configuration object.

````csharp

    var config = new DefaultConfiguration();
    config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
    config.ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");

````

To register the Controller class via the Container:

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