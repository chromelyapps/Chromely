# Chromely
Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp).

### Platforms
Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.

### Base CefGlue/Chromium Version
CefGlue: 59.0.3071.109
CEF:3.3071.1644.g408afd1
Chromium: 59.0.3071.109

### Demos
Demos are located at: [Chromely.Demos](https://github.com/mattkol/Chromely/tree/master/Demos).
To run the Demo:
1. Downlaod all files at [CefGlue Demo](https://github.com/mattkol/Chromely/tree/master/Demos/CefGlueWin64) or  [CefSharp Demo](https://github.com/mattkol/Chromely/tree/master/Demos/CefSharpWin64).
2. Get Cef binaries from [download page](http://opensource.spotify.com/cefbuilds/index.html). Base Chromium version 59 is supported for CefGlue, for CefSharp it will be dependent on what package is installed. The test is targeted for x64.  
    * Copy all files from /Release to the demo folder.
    * Copy all files fron /Resources to demo folder.
3. Chromely uses a restful like service. To test an external dll implementing a restful service. Please see how to register an external restful service dll in :  [Register Service](https://github.com/mattkol/Chromely/blob/master/src/Demos/Chromely.CefGlue.Winapi.Demo/Program.cs). To run the demo implementation - create a folder "C:\ChromelyDlls" and copy [Chromely.Service.Demo.dll](https://github.com/mattkol/Chromely/tree/master/src/SharedDlls) to the folder.
4. Run cefglue_winapi_demo.exe or cefsharp_winapi_demo.exe

#### Chromely Demo Screenshots
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/CefGlue/chromely_cefglue_index.png)
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/CefGlue/chromely_cefglue_info.png)
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/CefGlue/chromely_cefglue_restful.png)
