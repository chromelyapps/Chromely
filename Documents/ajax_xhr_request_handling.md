
# Ajax XHR Request Handling

This is info about registering handlers for [CEF Request Handling](https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage#markdown-header-scheme-handler).

Chromely supports CEF3 network request handling via the scheme handling approach. This allows for Ajax XMLHttpRequest (XHR) request handling. 

A custom scheme handler would require:
1. The scheme name - HTTP, HTTPS and custom schemes
2. The domain name - chromely.com, anydomain, google.com
3. The handler factory (the actual scheme handler object must be defined in the factory).

For more info on using a default handler or to register a custom scheme handler please check - [Registering Custom Scheme Handlers](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_scheme_handlers.md).


####  UI - XHR Url
A complete sample url will be
http://chromely.com/democontroller/movies
- where: 
  - scheme name - http
  -  domain name - chromely.com
  -  controller  - democontroller
  - action  - movies

The route path "democontroller/movies" must be registered and map to a route resource. For more info please check - [Registering Url Schemes](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_url_schemes.md)

#### UI - A typical XHR Get request:
````javascript
var http = new XMLHttpRequest();
var url = "http://chromely.com/democontroller/movies";
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
