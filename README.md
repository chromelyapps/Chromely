# Chromely
Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp).

### Platforms
Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.

Xilium.CefGlue .NET Standard port is available on [Nuget](https://www.nuget.org/packages/Chromely.Unofficial.CefGlue.NetStd/).

For more info/documentation, please check [Chromely wiki](https://github.com/mattkol/Chromely/wiki).

### Chromely Demos 
For more info on demos - [Demos](https://github.com/mattkol/Chromely/wiki/Demos)
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/Cefsharp/chromely_cefsharp_index_info.png)


### VS Projects Description 
| Project | Framework| Comment |
| :---         |     :---      | :--- |
| Chromely.Unofficial.CefGlue.NetStd   | .NET Standard    | *** Unofficial port of Xilium.CefGlue. Available on [Nuget](https://www.nuget.org/packages/Chromely.Unofficial.CefGlue.NetStd/).    |
| Chromely.Core    | .NET Standard       |   The core library required to build either a Chromely CefSharp or Chromely CefGlue apps.    |
| Chromely.CefGlue.Winapi    | .NET Standard        | Chromely CefGlue implementation library - this is in .NET Standard as it can be used in both .NET (Win) and .NET Core (Win, Linux)     |
| Chromely.CefSharp.Winapi     | .NET       | Chromely CefSharp implementation is only for .NET     |

### References
* WinApi - https://github.com/prasannavl/WinApi
* Cef - https://bitbucket.org/chromiumembedded/cef
* Xilium.CefGlue - https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
* CefSharp - https://github.com/cefsharp/CefSharp
* Json Serializer - https://github.com/lbv/litjson
* Caliburn.Micro Container - https://github.com/Caliburn-Micro/Caliburn.Micro/blob/master/src/Caliburn.Micro/SimpleContainer.cs

