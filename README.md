# Chromely
Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps using embedded Chromium ([Cef](https://bitbucket.org/chromiumembedded/cef)) without WinForm or WPF. Chromely uses Windows and Linux native GUI API as chromium hosts. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home), [CefSharp](https://github.com/cefsharp/CefSharp)), .NET/Javascript integration ([CefSharp](https://github.com/cefsharp/CefSharp)), Generic Message Routing ([Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home)).

### Platforms
Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.

[![Chromely.Core](http://img.shields.io/nuget/vpre/Chromely.Core.svg?style=flat&label=Chromely.Core)](https://www.nuget.org/packages/Chromely.Core)
[![Chromely.CefGlue.Winapi](http://img.shields.io/nuget/vpre/Chromely.CefGlue.Winapi.svg?style=flat&label=Chromely.CefGlue.Winapi)](https://www.nuget.org/packages/Chromely.CefGlue.Winapi)
[![Chromely.CefSharp.Winapi](http://img.shields.io/nuget/vpre/Chromely.CefSharp.Winapi.svg?style=flat&label=Chromely.CefSharp.Winapi)](https://www.nuget.org/packages/Chromely.CefSharp.Winapi)
[![Chromely.Unofficial.CefGlue.NetStd](http://img.shields.io/nuget/v/Chromely.Unofficial.CefGlue.NetStd.svg?style=flat&label=Chromely.Unofficial.CefGlue.NetStd)](https://www.nuget.org/packages/Chromely.Unofficial.CefGlue.NetStd/)

For more info/documentation, please check [Chromely wiki](https://github.com/mattkol/Chromely/wiki). 
<br>[![Chromely + Angular](https://img.shields.io/badge/Chromely%20Apps-Built%20with%20Angular%202%2B-green.svg)](https://github.com/mattkol/Chromely/wiki/Chromely-Apps)

### Creating a Simple App (Using CefGlue with Winapi host)
For more info see - [Getting Started](https://github.com/mattkol/Chromely/wiki/Getting-Started)
````csharp
class Program
{
   static int Main(string[] args)
   {
      string startUrl = "https://google.com";

      ChromelyConfiguration config = ChromelyConfiguration
                                    .Create()
                                    .WithAppArgs(args)
                                    .WithHostSize(1000, 600)
                                    .WithStartUrl(startUrl);

     var factory = WinapiHostFactory.Init();
     using (var window = factory.CreateWindow(() => new CefGlueBrowserHost(config),
           "chromely", constructionParams: new FrameWindowConstructionParams()))
      {
         window.SetSize(config.HostWidth, config.HostHeight);
         window.CenterToScreen();
         window.Show();
         return new EventLoop().Run(window);
     }
  }
}
````
### Chromely Demos 
For more info on demos - [Demos](https://github.com/mattkol/Chromely/wiki/Demos)
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/chromely_screens.gif)

### References
* WinApi - https://github.com/prasannavl/WinApi
* Cef - https://bitbucket.org/chromiumembedded/cef
* Xilium.CefGlue - https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
* CefSharp - https://github.com/cefsharp/CefSharp
* Json Serializer - https://github.com/lbv/litjson
* Caliburn.Light Container - https://github.com/tibel/Caliburn.Light/blob/master/src/Caliburn.Core/IoC/SimpleContainer.cs

