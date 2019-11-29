<p align="center"><img src="https://github.com/chromelyapps/Chromely/blob/master/nugets/chromely.ico?raw=true" /></p>
<h1 align="center">Chromely</h1>

Chromely is a lightweight alternative to <a href="https://github.com/ElectronNET/Electron.NET">Electron.NET</a>, <a href="https://github.com/electron/electron">Electron</a> for .NET/.NET Core developers.

Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps based on [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home), ~~[CefSharp](https://github.com/chromelyapps/Chromely.Legacy)~~ implementations of  embedded Chromium ([CEF](https://bitbucket.org/chromiumembedded/cef)) **without WinForms or WPF**. Chromely uses Windows and Linux native GUI API as "thin" chromium hosts. It can be extended to use WinForms or WPF. 

With Chromely you can build Single Page Application (SPA) HTML5 desktop apps with or without Node/npm. Building SPA apps using javascript frameworks like Angular, React, Vue or similar is easy. You can use Visual Studio Code or any IDE you are familiar with as long as Chromely knows the entry html file from the compiled/bundled files. For more info please see - [Chromely-Apps](https://github.com/chromelyapps/Chromely.Legacy/wiki/Chromely-Apps).

Options of communicating (IPC) with rendering process are via:

- Generic Message Routing ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home)) - more info @ [Generic Message Routing](https://github.com/chromelyapps/Chromely.Legacy/wiki/Generic-Message-Routing).
- Ajax HTTP/XHR ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home), [CefSharp](https://github.com/cefsharp/CefSharp)) -  more info @ [Custom Scheme Handling](https://github.com/chromelyapps/Chromely.Legacy/wiki/Custom-Scheme-Handling).

[<h4>Roadmap & Help Wanted</h4>](https://github.com/chromelyapps/Chromely/wiki/Roadmap-and-Help-Wanted) 

##### If you like Chromely, please give it a star - it helps! #####

Have a quick question? Wanna chat? Connect on  [![Join the chat at https://gitter.im/chromely_/Lobby](https://badges.gitter.im/chromely_/Lobby.svg)](https://gitter.im/chromely_/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Have an app/project/tool using Chromely - [please share!](https://github.com/chromelyapps/Chromely/issues/63)

### Platforms
Cross-platform -**Windows**, **Linux**, **MacOS**. Built on CEF, CefGlue, NET Standard 2.0, .NET Core 3.0, .NET Framework 4.61 and above.

- Windows<sup>(1)</sup> 32-bit 
- Windows<sup>(1)</sup> 64-bit 
- Linux<sup>(2)</sup> 32-bit   
- Linux<sup>(2)</sup> 64-bit   
- MacOSX<sup>(3)</sup> 64-bit  
- Linux ARM<sup>(4)</sup>      

&nbsp;<sup>(1)</sup>&nbsp; Windows 7, Service Pack 1 and newer    
&nbsp;<sup>(2)</sup>&nbsp; Ubuntu 16.04 and newer    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(Mono currently not working)    
&nbsp;<sup>(3)</sup>&nbsp; Tested on macOS Mojave 10.14.6  (Other versions will likely work too)     
&nbsp;<sup>(4)</sup>&nbsp; i.e. Raspberry Pi 3+    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(manual download of CEF builds for ARM available on http://chromely.org/cefbuilds/index.html) 

[![Chromely.Core](http://img.shields.io/nuget/vpre/Chromely.Core.svg?style=flat&label=Chromely.Core)](https://www.nuget.org/packages/Chromely.Core)
[![Chromely.Core](http://img.shields.io/nuget/vpre/Chromely.CefGlue.svg?style=flat&label=Chromely.CefGlue)](https://www.nuget.org/packages/Chromely.CefGlue)
[![Chromely.Core](http://img.shields.io/nuget/vpre/Chromely.svg?style=flat&label=Chromely)](https://www.nuget.org/packages/Chromely)

[![Chromely + Angular](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20Angular%202%2B-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps)
<br>[![Chromely + React](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20React-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps)
<br>[![Chromely + Vue](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20Vue-green.svg)](https://github.com/chromelyapps/Chromely/wiki/Chromely-Apps) 

### Demo Code
Get started with the [demos](https://github.com/chromelyapps/demo-projects). 

### Creating a Simple App
For more info see - [Getting Started](https://github.com/chromelyapps/Chromely/blob/master/Documents/getting_started.md)

A basic Chromely project requires:

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

### Chromely Demos 
For more info on demos - [Demos](https://github.com/chromelyapps/Chromely/wiki/Demos)
![](https://github.com/chromelyapps/Chromely/blob/master/Screenshots/chromely_screens_n3.gif)

### References
* Cef - https://bitbucket.org/chromiumembedded/cef
* Xilium.CefGlue - https://gitlab.com/xiliumhq/chromiumembedded/cefglue
* Caliburn.Light Container - https://caliburnmicro.com/documentation/simple-container

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
