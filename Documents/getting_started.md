
# Getting Started

When creating a new project, you want to create a .NET Core 3/.NET 5 Console Project (for all platforms - Windows, Linux, MacOS) or a .NET Framework Console Project for Windows. 

Visual Studio 2019, JetBrains Rider or Visual Studio Code is preferred but any Editor can be used.

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

To run a Chromely app, 3 primary objects can be configured. 

Full application builder options:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseConfig<CustomConfiguraton>()
      .UseWindow<CustomWindow>()
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
      .UseConfig<CustomConfiguraton>(new CustomConfiguraton())
      .UseWindow<CustomWindow>(new CustomWindow())
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}
````

#### Custom Application Class - required

To create a Chromely application, a custom application class (and instance) is required. The class can inherit from any of the base class implementations:
- [ChromelyBasicApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/ChromelyBasicApp.cs) 
- [ChromelyFramelessApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/ChromelyFramelessApp.cs) 

#### Window Class - optional

To create Chromely application, the Window class is not required. A custom window class must implement - [IChromelyWindow](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Host/IChromelyWindow.cs) or inherit from [Window](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Window.cs)

#### Configuration Class - optional

The Chromely configuration is equivalent to Desktop App.config or Web project Web.config/appsettings.config. To create Chromely application, the Configuration class is not required. A custom configuration class must implement - [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs). 

- If no configuraton file is found, it uses the default - [DefaultConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/DefaultConfiguration.cs).
