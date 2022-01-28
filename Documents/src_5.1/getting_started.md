
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

To run a Chromely app, 3 primary objects can be configured. Other services/objects can be registered using [.NET Extensions Dependency Injection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) in the [application class](https://github.com/chromelyapps/demo-projects/blob/53ccbdd22eac818ebf96df594f6fc81369965772/regular-chromely/CrossPlatDemo/Program.cs#L40).

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

To create a Chromely application, a custom application class (or/and instance) is required. The class can inherit the base class implementation:
- [ChromelyBasicApp](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/ChromelyBasicApp.cs) 

#### Window Class - optional

To create Chromely application, the Window class is not required. A custom window class must implement - [IChromelyWindow](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Host/IChromelyWindow.cs) or inherit from [Window](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Window.cs). A custom Window type/instance can also be registered using [.NET Extensions Dependency Injection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/).

#### Configuration Class - optional

The Chromely configuration is equivalent to Desktop App.config or Web project Web.config/appsettings.config. To create Chromely application, the Configuration class is not required. A custom configuration class must implement - [IChromelyConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/IChromelyConfiguration.cs).  A custom configuration type/instance can also be registered using [.NET Extensions Dependency Injection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/).

- If no configuraton file is found, it uses the default - [DefaultConfiguration](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/DefaultConfiguration.cs).
