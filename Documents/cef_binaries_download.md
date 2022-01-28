# CEF Binaries Download

Note: This description is for 64-bit. The guide will also work for 32-bit - the right binaries need to be downloaded.

CEF binaries are automatically downloaded for Windows builds for Nuget packages. For all platforms (Windows, Linux, MacOS, binaries are downloaded at runtime if configured to do so by the developer. 


#### [CEF Download Options](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/ICefDownloadOptions.cs)
| Option| Description | 
| :---         | :---         | 
| AutoDownloadWhenMissing  | If set to "true", CEF binaries will be downloaded if missing.| 
| DownloadSilently  | If set to "true", no notification will be provided during CEF binaries download.| 
| NotificationType   | The notification type of [CefDownloadNotificationType](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/CefDownloadNotificationType.cs)| 

#### [CEF Download Notification Types](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Configuration/CefDownloadNotificationType.cs)
| CEF Notification Type| Description | 
| :---         | :---         | 
| Logger | Logs notification messages to the Logger as set by the application. | 
| Console | Show notification messages in the Console. | 
| HTML | Display notification messages in HTML pages. | 
| Custom | Other options - for developer use. | 

#### CEF Download Default Notifications
| Notification Type| Notification | 
| :---         | :---         | 
| Logger | [LoggerCefDownloadNotification](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Loader/LoggerCefDownloadNotification.cs) | 
| Console | [ConsoleCefDownloadNotification](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Loader/ConsoleCefDownloadNotification.cs) | 
| HTML | [HtmlCefDownloadNotification](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Loader/HtmlCefDownloadNotification.cs)| 


Auto download at runtime is the default.
To disable auto download and do it manually:

````C#
   var config = DefaultConfiguration.CreateForRuntimePlatform();
   config.CefDownloadOptions.AutoDownloadWhenMissing = false;
````

**Note:** If no NotificationType is set, [LoggerCefDownloadNotification](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/Loader/LoggerCefDownloadNotification.cs) will be used.

For manual download please follow these steps:

1.  Get CEF binaries from [Spotify CEF binaries download page](https://cef-builds.spotifycdn.com/index.html). 

     To get the version info see [Version File](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely/CefGlue/Interop/version.g.cs).
     
     #### Versions Map 
    | Chromely.CefGlue| Chromely.Unofficial.CefGlue.NetStd | Required CEF Binary |
    | :---         | :---         | :--- |
    | 5.2.96.* | 96.0.4664.110 | 96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110 |
          
    For instance binaries download for Chromely 5.2.96.* will be url links: 
    ```` 
    http://opensource.spotify.com/cefbuilds/cef_binary_96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110_windows64_minimal.tar.bz2

    http://opensource.spotify.com/cefbuilds/cef_binary_96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110_linux64_minimal.tar.bz2

    http://opensource.spotify.com/cefbuilds/cef_binary_96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110_macosx64_minimal.tar.bz2
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
