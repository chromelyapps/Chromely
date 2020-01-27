# Chromely Configuration

Chromely configuration is equivalent to Desktop App.config or Web project Web.config/appsettings.config. Most required CEF/Chromely configurations are done via [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs) implementation. Configuration provides default values that can be used as-is or with new/custom properties set. 

To create Chromely application, the Configuration class is not required. A custom configuration class must implement - [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs). If a custom configuration class is not provided, the options used are

- It checks the application folder for [chromelyconfig.config](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/chromelyconfig.json). If it finds one it is parsed using [ConfigurationHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/ConfigurationHandler.cs) and used.
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

- List - ControllerAssemblies      
    - Used to register current and external assemblies that contain [Controller](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Network/ChromelyController.cs) implementations.
    - Can be set in both config file or C# code
    - Configuration:
      -   ````javascript
          "controllerAssemblies": [
            "Chromely.External.Controllers.dll"
          ]
          ````
      -  ````csharp
            var config = DefaultConfiguration.CreateForRuntimePlatform();
              config.ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");
          ````

- List - EventHandlers       
    - Used to register [ChromelyEventHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/ChromelyEventHandler.cs) implementations.
    - Cannot be set in config file. Only in code.
    - [ChromelyEventedApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/ChromelyEventedApp.cs) sets default event handlers if used, must methods must be overriden.
    - Samples can be found [here](https://github.com/chromelyapps/Chromely/blob/master/src/Tests/Chromely.Integration.TestApp/Program.cs)

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

### Basic Usage

Using [chromelyconfig.json](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/chromelyconfig.json) :

````javascript
{
  "appName": "chromely_demo",
  "startUrl": {
    "url": "local://app/chromely.html",
    "loadType": "localResource"
  },
  },
  "urlSchemes": [
  ],
  "controllerAssemblies": [
  ],
  "customSettings": [
  ],
  "commandLineArgs": [
  ],
  "commandLineOptions": [
  ]
}
````
- Using C# code

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      var config = DefaultConfiguration.CreateForRuntimePlatform();

      AppBuilder
      .Create()
      .UseConfiguration<DefaultConfiguration>()
      .UseApp<CustomChromelyApp>(config)
      .Build()
      .Run(args);
   }
}

````

Or:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseConfiguration<CustomConfiguraton>()  
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}

public class Customfiguraton: ICCustomConfiguraton
{
}
````

Or:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseConfiguration<CustomConfiguraton>(new Customfiguraton())
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}

public class Customfiguraton: ICCustomConfiguraton
{
}
````

### Demo coniguration file options:
- [Default for Windows](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/config/chromelyconfig_win.json).          
- [Default for Linux](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/config/chromelyconfig_linux.json).         
- [Default for MaOS](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/config/chromelyconfig_mac.json).   
- [Config options](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/CrossPlatDemo/config/config_option.json).        


