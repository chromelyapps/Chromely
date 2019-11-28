
# Getting Started

When creating a new project, you want to create a .NET Core 3 Console Project (for all platforms - Windows, Linux, MacOS) or a .NET Framework Console Project for Windows. 

Visual Studio 2019 or Visual Studio Code is preferred but any Editor can be used.

Chromely ONLY supports x64 application. Developers can try x86 too but will not be supported.

A simple Chromely project requires:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
       AppBuilder
       .Create()
       .UseApp<DemoChromelyApp>()
       .Build()
       .Run(args);
    }
}
````

Chromely is configurable and extensible. 

To run a Chromely app, 5 primary objects can be configured. 

Full application builder options:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseConfiguration<CustomContainer>()
      .UseAppSettings<CustomAppSetting>()
      .UseLogger<CustomLogger>()
      .UseConfiguration<CustomConfiguraton>()
      .UseApp<CustomChromelyApp>()
      .Build()
      .Run(args);
   }
}
````

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseConfiguration<CustomContainer>(new CustomContainer())
      .UseAppSettings<CustomAppSetting>(new CustomAppSetting())
      .UseLogger<CustomLogger>(new CustomLogger())
      .UseConfiguration<CustomConfiguraton>(new CustomConfiguraton())
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}
````

#### Custom Application Class - required

To create a Chromely application, a custom application class (and instance) is required. The class can inherit from any of the abstract class implementations:
- [BasicChromelyAp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/BasicChromelyApp.cs) 
- [ChromelyEventedApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/ChromelyEventedApp.cs) 
- [ChromelyApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/ChromelyApp.cs) 


#### IOC Conatainer Class - optional

Chromely provides a default IOC container - [SimpleContainer](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Infrastructure/SimpleContainer.cs). This is a copy of [Caliburn.Micro SimpleContainer](https://caliburnmicro.com/documentation/simple-container) with slight modification. Developers can provide a different container, but must implement [IChromelyContainer](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyContainer.cs).


#### App/User Settings Class - optional

Chromely provides storing and retrieval of application/user preferences settings.
A default AppSetting class - []() is provided. Developers can provide a custom settings class, but the class must implement [IChromelyAppSettings](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyAppSettings.cs). If none is provided it uses the default - [DefaultAppSettings](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Defaults/DefaultAppSettings.cs).


#### Logger Class - optional

Application wide logging is provided via a logger class that implements [IChromelyLogger](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyLogger.cs). Chromely provides a default, simple logging class - [SimpleLogger](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Infrastructure/SimpleLogger.cs). This can be replaced by a different logger, but the logger must implement - [IChromelyLogger](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyLogger.cs).



#### Configuration Class - optional

The Chromely configuration is equivalent to Desktop App.config or Web project Web.config/appsettings.config. To create Chromely application, the Configuration class is not required. A custom configuration class must implement - [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyConfiguration.cs). If a custom configuration class is not provided, the options used are

- It checks the application folder for [chromelyconfig.config](https://github.com/chromelyapps/Chromely/blob/master/src/Demos/CrossPlatDemo/chromelyconfig.json). If it finds one it is parsed using [ConfigurationHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/ConfigurationHandler.cs) and used.
- If no configuraton file is found, it uses the default - [DefaultConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Defaults/DefaultConfiguration.cs).