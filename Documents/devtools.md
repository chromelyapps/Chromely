# DevTool

- [DevTool](#devtool)
  - [DevTools On Windows](#devtools-on-windows)
  - [DevTools on All Platforms](#devtools-on-all-platforms)


<div id='devtoolswin'/>

## DevTools On Windows

On Windows, if debug mode is enabled, Chromely provides 2 command context menu items:
- Show DevTools
    - Launches Chromium in-built DevTool window.
- Close DevTools
    - Closes the DevTool window if opened.

This context menu option is only availble in Windows and available if debug mode is selected. 

Debug mode in config file:

````javascript
    "debuggingMode": true
````
Debug mode in config code:

````csharp
    var config = DefaultConfiguration.CreateForRuntimePlatform();
    config.DebuggingMode = true;
````

The context menu is provided via CEF/CefGlue's [ContextMenuHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefContextMenuHandler.cs). Chromely provides a [default implementation of ContextMenuHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/Browser/Handlers/CefGlueContextMenuHandler.cs) but it is configurable.

To conigure the DevTools menu, register a new custom ContextMenuHandler:

````csharp
    public class CusomChromelyApp : ChromelyBasicApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(IChromelyCustomHandler), Guid.NewGuid().ToString(), typeof(CustomContextMenuHandler));
        }
    }

    public class CustomContextMenuHandler : CefContextMenuHandler
    {
    }
````

The default handler screenshots:

![image](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/devtool/devtool_win.png)

![image](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/devtool/devtool_win2.png)


<div id='devtoolsall'/>

## DevTools on All Platforms

The DevTools context menu is not available on Linux and MacOS. **Developers who are interested can look into developing one for Linux or MacOS but will not be supported**. 

For all platforms the [Demo](https://github.com/chromelyapps/demo-projects/blob/5d075a31d335ca7b64750555e4765eb1b854b203/regular-chromely/CrossPlatDemo/app/chromely.html#L83), [Demo-Angular](https://github.com/chromelyapps/demo-projects/blob/5d075a31d335ca7b64750555e4765eb1b854b203/angular-react-vue/ChromelyAngular/angularapp/src/components/app.component.html#L33), [Demo-React](https://github.com/chromelyapps/demo-projects/blob/5d075a31d335ca7b64750555e4765eb1b854b203/angular-react-vue/ChromelyReact/reactapp/src/App.jsx#L56) and [Demo-Vue](https://github.com/chromelyapps/demo-projects/blob/5d075a31d335ca7b64750555e4765eb1b854b203/angular-react-vue/ChromelyVue/vueapp/src/App.vue#L35) provide ways to launch the DevTools window in the OS default browser.

This requires a [Command implementation](https://github.com/chromelyapps/Chromely/blob/master/Documents/commands.md):

````html
      <a class="dropdown-item" onclick="showDevTools()">Show DevTools</a>
````

````javascript
    function showDevTools() {
        var url = "http://command.com/democontroller/showdevtools";
        var link = document.createElement('a');
        link.href = url;
        document.body.appendChild(link);
        link.click(); 
    }
````
.. and [Controller code](https://github.com/chromelyapps/demo-projects/blob/5d075a31d335ca7b64750555e4765eb1b854b203/regular-chromely/CrossPlatDemo/Controllers/DemoController.cs#L57):

````csharp
    [Command(Route = "/democontroller/showdevtools")]
    public void ShowDevTools(IDictionary<string, string> queryParameters)
    {
        if (_config != null && !string.IsNullOrWhiteSpace(_config.DevToolsUrl))
        {
            BrowserLauncher.Open(_config.Platform, _config.DevToolsUrl);
        }
    }
````

Alternatively the developer can manually launch in the browser using the DevTool url.

Usually it is: http://127.0.0.1:20480 but the actual value can be found by:

````csharp
    var config = DefaultConfiguration.CreateForRuntimePlatform();
    .....
    var devtoolsUrl = DevToolsUrl;
````

The DevTools screenshots:

![image](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/devtool/devtool_all1.png)

![image](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/devtool/devtool_all2.png)

![image](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/devtool/devtool_all3.png)