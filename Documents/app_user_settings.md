
# App/User Settings

Chromely provides storing and retrieval of application/user preferences settings.
Developers can provide a custom settings class, but the class must implement [IChromelyAppSettings](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyAppSettings.cs). If none is provided it uses the default - [DefaultAppSettings](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/Defaults/DefaultAppSettings.cs).

The default implementation of [IChromelyAppSettings](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyAppSettings.cs) has the following features:

- Property - AppName
    - Current application name

 - Property - DataPath
    - This is the full path of where the settings values are saved. If none is supplied it uses the [OS SpecialFolder ApplicationFolder](https://github.com/chromelyapps/Chromely/blob/1a358986894377da8cffe87e00ce0d5893db690b/src/Chromely.Core/Infrastructure/AppSettingInfo.cs).
    - The filename is in the format: "{AppName}_appsettings.config";

- Property - Settings 
    - This is a dynamic property and allows the developer to save any "Key/Value" items.
    - Chromely exposes a static property to save and retrieve values
        - Samples:
            -  ````csharp
                    ChromelyAppUser.App.Properties.Settings.Item1 = "Market 01";
                    ChromelyAppUser.App.Properties.Settings.Item2 = "Year 2020";

                    var list = new List<string>();
                    list.Add("item0001");
                    list.Add("item0002");
                    ChromelyAppUser.App.Properties.Settings.List1 = list;
               ````

- Method - Read
    - Reads a previously saved settings on application launch.
    
- Method - Save
    - Saves the current settings on application close.


To set a custom AppSettings:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseAppSettings<CustomAppSetting>()
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}

public class CustomAppSettings : IChromelyAppSettings
{
}
````

Or:

````csharp
class Program
{
   [STAThread]
   static void Main(string[] args)
   {
      AppBuilder
      .Create()
      .UseAppSettings<CustomAppSetting>(new CustomAppSetting())
      .UseApp<CustomChromelyApp>(new CustomChromelyApp())
      .Build()
      .Run(args);
   }
}

public class CustomAppSettings : IChromelyAppSettings
{
}
````