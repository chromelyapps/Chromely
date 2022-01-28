# CEF Binaries Download

Note: This description is for 64-bit. The guide will also work for 32-bit - the right binaries need to be downloaded.

CEF binaries are automatically downloaded for Windows builds for Nuget packages. For all platforms (Windows, Linux, MacOS, binaries are downloaded at runtime if configured to do so by the developer. 

Auto download at runtime is the default.
To disable auto download and do it manually:

````C#
   var config = DefaultConfiguration.CreateForRuntimePlatform();
   config.CefDownloadOptions.AutoDownloadWhenMissing = false;
````


For manual download please follow these steps:

1.  Get CEF binaries from [Spotify CEF binaries download page](https://cef-builds.spotifycdn.com/index.html). 

     To get the version info see [Version File](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/CefGlue/Interop/version.g.cs).
     
     #### Versions Map 
    | Chromely.CefGlue| Chromely.Unofficial.CefGlue.NetStd | Required CEF Binary |
    | :---         | :---         | :--- |
    | 5.0.77.* | 77.0.3865.120 | 77.1.14%2Bg4fb61d2%2Bchromium-77.0.3865.120 |
    | 5.0.79.* | 79.0.3945.130 | 79.1.36+g90301bd+chromium-79.0.3945.130 |
    | 5.0.81.* | 81.0.4044.113 | 81.2.21%2Bge864886%2Bchromium-81.0.4044.113 |
    | 5.0.83.* | 83.0.4103.106 | 83.5.0%2Bgbf03589%2Bchromium-83.0.4103.106 |
    | 5.1.86.* | 86.0.4240.183 | 86.0.21%2Bg6a2c8e7%2Bchromium-86.0.4240.183 |
    | 5.1.88.* | 88.0.4324.96  | 88.1.6%2Bg4fe33a1%2Bchromium-88.0.4324.96 |
          
    For instance binaries download for Chromely 5.0.77.* will be url links: 
    ```` 
    http://opensource.spotify.com/cefbuilds/cef_binary_77.1.14%2Bg4fb61d2%2Bchromium-77.0.3865.120_windows64_minimal.tar.bz2

    http://opensource.spotify.com/cefbuilds/cef_binary_77.1.14%2Bg4fb61d2%2Bchromium-77.0.3865.120_linux64_minimal.tar.bz2

    http://opensource.spotify.com/cefbuilds/cef_binary_77.1.14%2Bg4fb61d2%2Bchromium-77.0.3865.120_macosx64_minimal.tar.bz2
     ````

2. Unzip/untar the compressed/zippped file to last folder level.

3. - Windows
        - Copy all **files and folders** from Release folder to the appropriate bin folder - where the project exe file is 
                located.
        - Copy all **files and folders** from Resources folder to appropriate bin folder - where the project exe file is located.
        - A final merged layout of the files should look like:
           ![](https://github.com/mattkol/Chromely/blob/master/Screenshots/win_cef_binaries.png)

    - Linux
        - Copy all **files and folders** from Release folder to the appropriate bin folder - where the project exe file is 
                located.
        - Copy all **files and folders** from Resources folderto appropriate bin folder - where the project exe file is located.
        - A final merged layout of the files should look like:
           ![](https://github.com/mattkol/Chromely/blob/master/Screenshots/linux_cef_binaries.png)


    - MacOS
        - Copy file **\Release\Chromium Embedded Framework.framework\Chromium Embedded Framework** to the appropriate bin folder - where the project exe file is 
        - Rename file **\Release\Chromium Embedded Framework.framework\Chromium Embedded Framework** to **libcef.dylib** 
        - Copy all **files and folders** from "\Release\Chromium Embedded Framework.framework\Libraries" folder to appropriate bin folder - where the project exe file is located.
        - Copy all **files and folders** from "\Release\Chromium Embedded Framework.framework\Resources" folder to appropriate bin folder - where the project exe file is locat
        - A final merged layout of the files should look like:
           ![](https://github.com/mattkol/Chromely/blob/master/Screenshots/macos_cef_binaries.png)
