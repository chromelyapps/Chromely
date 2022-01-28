
# Network Service

Chromely via CefGlue provides 2 different ways of Interprocess Communication (IPC) between the Renderer and the Browser.

- Generic Message Routing - more info @ [Generic Message Routing](https://github.com/chromelyapps/Chromely/blob/master/Documents/generic_message_routing.md).
- Ajax HTTP/XHR  -  more info @ [Custom Scheme Handling](https://github.com/chromelyapps/Chromely/blob/master/Documents/ajax_xhr_request_handling.md).


These allow Chromely to receieve JavaScript requests initated by the Renderer, processed by the Browser (C#) and returned Json data response to the Renderer. 

## Configuring Network Scheme/Request Endpoint

The configuration of an IPC workflow involves:

1. Create a Controller class
2. Add the attribute - [ChromelyRouteAttribute](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Network/ControllerAttributes.cs) to the route actions (methods). Ensure that the "Path" property is set.
3. Register the Controller class
4. Register a scheme (http, custom, etc)
5. Create a JavaScript request in the Renderer html

##  1.  Create a Controller class

Every IPC workflow requires a Controller class. The class must inherit [ChromelyController](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Network/ChromelyController.cs).


````csharp
public class CustomController : ChromelyController
{
}
````

##  2.  Add the attribute - [ChromelyRouteAttribute](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Network/ControllerAttributes.cs)

An action method is recognized by the ChromelyRoute attribute .

````csharp

[ChromelyRoute(Path = "/democontroller/movies/get")]
public List<MovieInfo> GetMovies()
{
}

[ChromelyRoute(Path = "/democontroller/movies/post")]
public string SaveMovies(List<MovieInfo> movies)
{
}

[ChromelyRoute(Path = "/democontroller/showdevtools")]
public void ShowDevTools()
{
}

````

##  3.  Register the Controller class

The class CustomController must be registered. Registration can be done either by registering the Assembly where the class is created or registering via the Container

- Adding as assembly or assembly file

````csharp

public class CusomChromelyApp : ChromelyBasicApp
{
        public override void ConfigureServices(ServiceCollection services)
    {
        base.ConfigureServices(services);
        RegisterControllerAssembly(services, typeof(DemoApp).Assembly);
        RegisterControllerAssembly(services, fullpath);
    }
}

````

- Register the Controller class via the Container:

````csharp

public class CusomChromelyApp : ChromelyBasicApp
{
        public override void ConfigureServices(ServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<ChromelyController, CustomController>();
    }
}

````
### 4. Register a scheme (http, custom, etc)

For details on registering a custom scheme plese see [Custom Http Registration](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_scheme_handlers.md). 

### 5. Create a JavaScript request in the Renderer html

To trigger an actual request, an event must must be set up in the HTML file. 

Developers have 2 options to do this:
- Using Message Routing:

````javascript
function getMovies() {
        var request = {
            "url": "/democontroller/movies/get",
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

- Using Ajax XMLHttpRequest (XHR):

````javascript
var http = new XMLHttpRequest();
var url = "http://chromely.com/democontroller/movies/get";
http.open("GET", url, true);

http.onreadystatechange = function () {
if (http.readyState == 4 && http.status == 200) {
    var jsonData = JSON.parse(http.responseText);
    ..... process response
    }
}
http.send();
````

#### UI - A typical XHR Post request:
````javascript
    var moviesJson = [
        { Id: 1, Title: "The Shawshank Redemption", Year: 1994, Votes: 678790, Rating: 9.2 },
        { Id: 2, Title: "The Godfather", Year: 1972, votes: 511495, Rating: 9.2 },
        { Id: 3, Title: "The Godfather: Part II", Year: 1974, Votes: 319352, Rating: 9.0 },
        { Id: 4, Title: "The Good, the Bad and the Ugly", Year: 1966, Votes: 213030, Rating: 8.9 },
        { Id: 5, Title: "My Fair Lady", Year: 1964, Votes: 533848, Rating: 8.9 },
        { Id: 6, Title: "12 Angry Men", Year: 1957, Votes: 164558, Rating: 8.9 }
    ];

    var http = new XMLHttpRequest();
    var url = "http://chromely.com/democontroller/movies/post";
    http.open("POST", url, true);
    http.setRequestHeader("Content-type", "application/json");

    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            var jsonData = JSON.parse(http.responseText);
            $('#ajaxPostResult').html(jsonData);
        }
    }

    var reqMovies = {};
    reqMovies.movies = moviesJson;
    http.send(JSON.stringify(reqMovies));
````