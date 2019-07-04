<p align="center"><img src="https://github.com/chromelyapps/Chromely/blob/master/nugets/chromely.ico?raw=true" /></p>
<h1 align="center">Chromely</h1>

Chromely is a lightweight alternative to <a href="https://github.com/ElectronNET/Electron.NET">Electron.NET</a>, <a href="https://github.com/electron/electron">Electron</a> for .NET/.NET Core developers.

Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps based on [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home), [CefSharp](https://github.com/cefsharp/CefSharp) implementations of  embedded Chromium ([CEF](https://bitbucket.org/chromiumembedded/cef)) **without WinForms or WPF**. Chromely uses Windows and Linux native GUI API as "thin" chromium hosts. It can be extended to use WinForms or WPF. 

With Chromely you can build Single Page Application (SPA) HTML5 desktop apps with or without Node/npm. Building SPA apps using javascript frameworks like Angular, React, Vue or similar is easy. You can use Visual Studio Code or any IDE you are familiar with as long as Chromely knows the entry html file from the compiled/bundled files. For more info please see - [Chromely-Apps](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps).

Options of communicating (IPC) with rendering process are via:

1. .NET/Javascript integration ([CefSharp](https://github.com/cefsharp/CefSharp))  -  more info @ [Expose .NET class to JavaScript](https://github.com/chromelyapps/Chromely/wiki/Expose-.NET-class-to-JavaScript).
2. Generic Message Routing ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home)) - more info @ [Generic Message Routing](https://github.com/chromelyapps/Chromely/wiki/Generic-Message-Routing).
3. Ajax HTTP/XHR ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home), [CefSharp](https://github.com/cefsharp/CefSharp)) -  more info @ [Custom Scheme Handling](https://github.com/chromelyapps/Chromely/wiki/Custom-Scheme-Handling).
 4. Real-time with Websocket (ws) ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home)) -   more info @ [Real-time with Websocket](https://github.com/chromelyapps/Chromely/wiki/Real-time-with-Websocket).

[<h4>Roadmap & Help Wanted</h4>](https://github.com/chromelyapps/Chromely/wiki/Roadmap-and-Help-Wanted) 

##### If you like Chromely, please give it a star - it helps! #####

Have a quick question? Wanna chat? Connect on  [![Join the chat at https://gitter.im/chromely_/Lobby](https://badges.gitter.im/chromely_/Lobby.svg)](https://gitter.im/chromely_/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Have an app/project/tool using Chromely - [please share!](https://github.com/chromelyapps/Chromely/issues/63)

### Platforms
Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.

| Platform                     | CefGlue.Winapi         |  CefGlue.Gtk           | CefSharp.Winapi |
| ---------------------------- | :--------------------: | :--------------------: | :------: |
| Windows<sup>(1)</sup> 32-bit | net461, netstandard2.0 | net461, netstandard2.0 | net461   |
| Windows<sup>(1)</sup> 64-bit | net461, netstandard2.0 |  ---                   |  ---     |
| Linux<sup>(2)</sup> 32-bit   |  ---     | netstandard2.0         |  ---     |
| Linux<sup>(2)</sup> 64-bit   |  ---     | netstandard2.0         |  ---     |
| MacOSX<sup>(3)</sup> 64-bit  |  ---     | ---                    |  ---     |
| Linux ARM<sup>(4)</sup>      |  ---     | netstandard2.0         |  ---     |

&nbsp;<sup>(1)</sup>&nbsp; Windows 7, Service Pack 1 and newer    
&nbsp;<sup>(2)</sup>&nbsp; Ubuntu 16.04 and newer    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(Mono currently not working, window resizing not working)    
&nbsp;<sup>(3)</sup>&nbsp; Work in progress...    
&nbsp;<sup>(4)</sup>&nbsp; i.e. Raspberry Pi 3+    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(manual download of CEF builds for ARM available on http://chromely.org/cefbuilds/index.html)    


For more info/documentation, please check [Chromely wiki](https://github.com/chromelyapps/Chromely/wiki). 

[![Chromely.Core](http://img.shields.io/nuget/vpre/Chromely.Core.svg?style=flat&label=Chromely.Core)](https://www.nuget.org/packages/Chromely.Core)
[![Chromely.CefGlue.Winapi](http://img.shields.io/nuget/vpre/Chromely.CefGlue.Winapi.svg?style=flat&label=Chromely.CefGlue.Winapi)](https://www.nuget.org/packages/Chromely.CefGlue.Winapi)
[![Chromely.CefSharp.Winapi](http://img.shields.io/nuget/vpre/Chromely.CefSharp.Winapi.svg?style=flat&label=Chromely.CefSharp.Winapi)](https://www.nuget.org/packages/Chromely.CefSharp.Winapi)
[![Chromely.Unofficial.CefGlue.NetStd](http://img.shields.io/nuget/v/Chromely.Unofficial.CefGlue.NetStd.svg?style=flat&label=Chromely.Unofficial.CefGlue.NetStd)](https://www.nuget.org/packages/Chromely.Unofficial.CefGlue.NetStd/)

[![Chromely + Angular](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20Angular%202%2B-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps)
<br>[![Chromely + React](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20React-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps)
<br>[![Chromely + Vue](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20Vue-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps) 

### Demo Code
Get started with the [demos](https://github.com/chromelyapps/demo-projects). 

### Creating a Simple App (Using CefGlue with Winapi host)
For more info see - [Getting Started](https://github.com/chromelyapps/Chromely/wiki/Getting-Started)

**Notes on Upgrade to CefGlue version 70 and CefSharp version 71 (Chromely v4)** - Please [see](https://github.com/chromelyapps/Chromely/wiki/Upgrade-to-CefGlue-v70-and-CefSharp-v71).

````csharp
class Program
{
   static int Main(string[] args)
   {
      var startUrl = "https://google.com";

      var config = ChromelyConfiguration
                     .Create()
                     .WithHostMode(WindowState.Normal, true)
                     .WithHostTitle("chromely")
                     .WithHostIconFile("chromely.ico")
                     .WithAppArgs(args)
                     .WithHostSize(1000, 600)
                     .WithStartUrl(startUrl);

      using (var window = ChromelyWindow.Create(config))
      {
         return window.Run(args);
      }
  }
}
````
 - [CefGlue Usage Readme](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/Chromely.CefGlue.Winapi.Demo/README.md) 
 - [CefSharp Usage Readme](https://github.com/chromelyapps/demo-projects/blob/master/regular-chromely/Chromely.CefSharp.Winapi.Demo/README.md)

#### Run Starter Template (Using dotnet core cli). For more info, please see - [dotnet cli demo template install](https://github.com/chromelyapps/Chromely/wiki/dotnet-cli-Starter-Template)

````csharp
dotnet new -i "Chromely.CefGlue.Win.Template::*"
dotnet new chromelywin 
dotnet restore
dotnet chromelycef.dll download v70 --cpu=x64 --dest="bin\Debug\netcoreapp2.1"
dotnet build
dotnet run 
````

### Chromely Demos 
For more info on demos - [Demos](https://github.com/chromelyapps/Chromely/wiki/Demos)
![](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/chromely_screens.gif)

### References
* WinApi - https://github.com/prasannavl/WinApi
* Cef - https://bitbucket.org/chromiumembedded/cef
* Xilium.CefGlue - https://gitlab.com/xiliumhq/chromiumembedded/cefglue
* CefSharp - https://github.com/cefsharp/CefSharp
* Json Serializer - https://github.com/lbv/litjson
* Caliburn.Light Container - https://github.com/tibel/Caliburn.Light/blob/master/src/Caliburn.Core/IoC/SimpleContainer.cs

Contributing
---
Contributions are always welcome, via PRs, issues raised, or any other means. To become a dedicated contributor, please [contact the Chromely team](https://github.com/orgs/chromelyapps/people) or [raise an issue](https://github.com/chromelyapps/Chromely/issues) mentioning your intent.

License
---
Chromely is MIT licensed. For dependency licenses [please see](https://github.com/chromelyapps/Chromely/blob/master/LICENSE.md).

Credits
---
Thanks to [JetBrains](https://www.jetbrains.com) for the OSS license of Resharper Ultimate.

Improved and optimized using:

<a href="https://www.jetbrains.com/resharper/
"><img src="https://blog.jetbrains.com/wp-content/uploads/2014/04/logo_resharper.gif" alt="Resharper logo" width="100" /></a>
