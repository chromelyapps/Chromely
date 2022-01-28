# Chromely Configuration

Chromely configuration is equivalent to Desktop App.config or Web project Web.config/appsettings.config. Most required CEF/Chromely configurations are done via [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs) implementation. Configuration provides default values that can be used as-is or with new/custom properties set. 

To create Chromely application, the Configuration class is not required. A custom configuration class must implement - [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs). 

- If no configuraton file is found, it uses the default - [DefaultConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/DefaultConfiguration.cs).

The default implementation of [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs) has the following features:


- Property - AppName
    - Current application name

 - Property - StartUrl 
    - The start url 
    - For more info, please see [Loading Html](https://github.com/chromelyapps/Chromely/blob/master/Documents/loading_html.md).

- Property - ChromelyVersion   
    - Usually ReadOnly Chromely/CEF version

- Property - Platform   
    - The platform/OS type - [ChromelyPlatform](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/ChromelyPlatform.cs).

- Property - DebuggingMode  
    - Debugging mode - true/false

- List - CommandLineArgs    
    - Usually CEF command line arguments, but can be used for anything.
    - Can be set in both config file or C# code
    - Configuration:
      -   ````javascript
          "customSettings": [
            {
              "name": "cefLogFile",
              "value": "logs\\chromely.cef.log"
            }
          ````
      -  ````csharp
            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.CustomSettings = new Dictionary<string, string>()
            {
                ["cefLogFile"] = "logs\\chromely.cef.log",
                ["logSeverity"] = "info",
                ["locale"] = "en-US"
            };
          ````
       
- List - CommandLineOptions     
    - Usually CEF command line options, but can be used for anything.
    - Can be set in both config file or C# code
    - Configuration:
      -   ````javascript
            "commandLineOptions": [
              "no-zygote",
              "disable-gpu"
              ]
          ````
      -  ````csharp
            var config = DefaultConfiguration.CreateForRuntimePlatform();
              config.CommandLineOptions = new List<string>()
              {
                  "no-zygote",
                  "disable-gpu"
              };
          ````

- Dictionary - CustomSettings      
    - Usually CEF settings options, but can be used for anything.
    - Can be set in both config file or C# code
    - Configuration:
      -   ````javascript
            "customSettings": [
                {
                  "name": "cefLogFile",
                  "value": "logs\\chromely.cef.log"
                },
                {
                  "name": "logSeverity",
                  "value": "info"
                },
                {
                  "name": "locale",
                  "value": "en-US"
                }
              ]
          ````
      -  ````csharp
            var config = DefaultConfiguration.CreateForRuntimePlatform();
              config.    CustomSettings = new Dictionary<string, string>()
              {
                  ["cefLogFile"] = "logs\\chromely.cef.log",
                  ["logSeverity"] = "info",
                  ["locale"] = "en-US"
              };
          ````

- Interface/object - IChromelyJavaScriptExecutor        
    - For more info, please see [JavaScript Execution](https://github.com/chromelyapps/Chromely/blob/master/Documents/javascript_execution.md) 

- List of objects - UrlSchemes         
    - For more info, please see [Registering Url Schemes](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_url_schemes.md) 

- Object - CefDownloadOptions         
    - For more info, please see [CefDownloadOptions](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/CefDownloadOptions.cs) 

- Object - WindowOptions          
    - For more info, please see [WindowOptions](https://github.com/chromelyapps/Chromely/blob/master/Documents/registering_url_schemes.md) 

- Object - WindowOptions          
    - For more info, please see [IWindowOptions](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IWindowOptions.cs) 

- Object - ExtensionData       
    -  System.Text.Json provides extra storage facility via [ExtensionData](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonextensiondataattribute?view=netcore-3.1)
    - This is implemented as-is provided by System.Text.Json.


````csharp
var config = DefaultConfiguration.CreateForRuntimePlatform();

ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<DefaultConfiguration>()
    .UseApp<CustomChromelyApp>(config)
    .Build()
    .Run();

````

Or:

````csharp
ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<CustomConfiguraton>()  
    .UseApp<CustomChromelyApp>(new CustomChromelyApp())
    .Build()
    .Run();

public class Customfiguraton: ICCustomConfiguraton
{
}
````

Or:

````csharp
ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<CustomConfiguraton>(new Customfiguraton())
    .UseApp<CustomChromelyApp>(new CustomChromelyApp())
    .Build()
    .Run();


public class Customfiguraton: ICCustomConfiguraton
{
}
````

