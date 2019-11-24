using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chromely.Core
{
    public class ConfigurationHandler
    {
        private const string CONFIGFILE = "chromelyconfig.json";

        public IChromelyConfiguration Parse<T>(string configFile = null) where T : IChromelyConfiguration
        {
            try
            {
                configFile = GetConfigFile(configFile);
                if (string.IsNullOrWhiteSpace(configFile))
                {
                    return null;
                }

                using (StreamReader jsonReader = new StreamReader(configFile))
                {
                    string json = jsonReader.ReadToEnd();
                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;

                    var configurationLocal = JsonSerializer.Deserialize<ConfigurationLocal>(json, options);
                    var config = (T)Activator.CreateInstance(typeof(T));

                    configurationLocal.Configure(config);

                    return config;
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
                return null;
            }
        }

        private static string GetConfigFile(string configFile = null)
        {
            if (string.IsNullOrWhiteSpace(configFile))
            {
                configFile = CONFIGFILE;
            }

            return UtilityLocal.EnsureFilePath(configFile);
        }
        private class StartUrlLocal
        {
            public string url { get; set; }
            public string loadType { get; set; }

            public string GetStartUrl()
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return "https://google.com";
                }

                if (string.IsNullOrWhiteSpace(loadType))
                {
                    return url;
                }

                switch (loadType.ToUpper())
                {
                    case StartUrlOption.ABSOLUTE:
                    case StartUrlOption.LOCALRESOURCE:
                        return url;
                    case StartUrlOption.FILEPROTOCOL:
                        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        return $"file:///{appDirectory}{url}";
                }

                return url;
            }
        }
        private class WindowCustomCreationLocal
        {
            public int windowStyles { get; set; }
            public int windowExStyles { get; set; }
            public bool useCustomtyle { get; set; }
        }
        private class CustomSettingLocal
        {
            public string name { get; set; }
            public string value { get; set; }
            public static Dictionary<string, string> GetSettings(List<CustomSettingLocal> settingLocals)
            {
                var settingsDic = new Dictionary<string, string>();
                if (settingLocals == null || !settingLocals.Any())
                {
                    return settingsDic;
                }

                foreach (var item in settingLocals)
                {
                    settingsDic.Add(item.name, item.value);
                }

                return settingsDic;
            }
        }

        private class CustomHandlerLocal
        {
            public string name { get; set; }
            public string customObjectQualifiedName { get; set; }
            public static Dictionary<string, object> GetHandlers(List<CustomHandlerLocal> handlerLocals)
            {
                var handlersDic = new Dictionary<string, object>();
                if (handlerLocals == null || !handlerLocals.Any())
                {
                    return handlersDic;
                }

                foreach (var item in handlerLocals)
                {
                    object value = string.IsNullOrWhiteSpace(item.customObjectQualifiedName) ? null : UtilityLocal.Create(item.customObjectQualifiedName);
                    handlersDic.Add(item.name, value);
                }

                return handlersDic;
            }
        }
        private class UrlSchemeLocal
        {
            public string name { get; set; }
            public string baseUrl { get; set; }
            public string scheme { get; set; }
            public string host { get; set; }
            public string urlSchemeType { get; set; }
            public bool baseUrlStrict { get; set; }
            public static List<UrlScheme> GetHandleCollections(List<UrlSchemeLocal> urlSchemeLocals)
            {
                var urlSchemes = new List<UrlScheme>();
                if (urlSchemeLocals == null || !urlSchemeLocals.Any())
                {
                    return urlSchemes;
                }

                foreach (var item in urlSchemeLocals)
                {
                    var urlScheme = new UrlScheme(item.name, item.scheme, item.host, item.baseUrl, GetType(item.urlSchemeType), item.baseUrlStrict);
                    urlSchemes.Add(urlScheme);
                }

                return urlSchemes;
            }

            private static UrlSchemeType GetType(string type)
            {
                switch (type.ToUpper())
                {
                    case UrlSchemeOption.NONE: return UrlSchemeType.None;
                    case UrlSchemeOption.RESOURCE: return UrlSchemeType.Resource;
                    case UrlSchemeOption.COMMAND: return UrlSchemeType.Command;
                    case UrlSchemeOption.CUSTOM: return UrlSchemeType.Custom;
                    case UrlSchemeOption.EXTERNAL: return UrlSchemeType.External;
                }

                return UrlSchemeType.None;
            }
        }
        private class CommandLineArgLocal
        {
            public string name { get; set; }
            public string value { get; set; }
        }
        private class ConfigurationLocal
        {
            public string appName { get; set; }
            public StartUrlLocal startUrl { get; set; }
            public bool loadCefBinariesIfNotFound { get; set; }
            public bool silentCefBinariesLoading { get; set; }
            public string cefLogFile { get; set; }
            public int windowLeft { get; set; }
            public int windowTop { get; set; }
            public int windowWidth { get; set; }
            public int windowHeight { get; set; }
            public bool windowNoResize { get; set; }
            public bool windowNoMinMaxBoxes { get; set; }
            public bool windowFrameless { get; set; }
            public bool windowCenterScreen { get; set; }
            public bool windowKioskMode { get; set; }
            public string windowState { get; set; }
            public string windowTitle { get; set; }
            public string windowIconFile { get; set; }
            public string logSeverity { get; set; }
            public string locale { get; set; }
            public bool debuggingMode { get; set; }
            public WindowCustomCreationLocal windowCustomCreation { get; set; }
            public List<CustomSettingLocal> customSettings { get; set; }
            public List<CustomHandlerLocal> customHandlers { get; set; }
            public List<UrlSchemeLocal> urlSchemes { get; set; }
            public List<string> controllerAssemblies { get; set; }
            public List<CommandLineArgLocal> commandLineArgs { get; set; }
            public List<string> commandLineOptions { get; set; }

            [JsonExtensionData]
            public Dictionary<string, object> ExtensionData { get; set; }

            public void Configure(IChromelyConfiguration config)
            {
                try
                {
                    if (config == null)
                    {
                        return;
                    }

                    config.AppName = appName;
                    if (startUrl != null)
                    {
                        config.StartUrl = startUrl.GetStartUrl();
                    }

                    config.WindowState = GetWindowState(windowState);
                    config.LoadCefBinariesIfNotFound = loadCefBinariesIfNotFound;
                    config.SilentCefBinariesLoading = silentCefBinariesLoading;
                    config.WindowLeft = windowLeft;
                    config.WindowTop = windowTop;
                    config.WindowWidth = windowWidth;
                    config.WindowHeight = windowHeight;
                    config.WindowNoResize = windowNoResize;
                    config.WindowNoMinMaxBoxes = windowNoMinMaxBoxes;
                    config.WindowFrameless = windowFrameless;
                    config.WindowCenterScreen = windowCenterScreen;
                    config.WindowKioskMode = windowKioskMode;
                    config.WindowTitle = windowTitle;
                    config.WindowIconFile = windowIconFile;

                    if (windowCustomCreation != null)
                    {
                        config.WindowCustomStyle = new WindowCustomStyle(windowCustomCreation.windowStyles, windowCustomCreation.windowExStyles);
                        config.UseWindowCustomStyle = windowCustomCreation.useCustomtyle;
                    }

                    config.AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
                    config.DebuggingMode = debuggingMode;

                    config.ControllerAssemblies = GetAssemblyInfos(controllerAssemblies);
                    config.CommandLineOptions = commandLineOptions;
                    config.ExtensionData = ExtensionData;

                    if (urlSchemes != null)
                    {
                        config.UrlSchemes = UrlSchemeLocal.GetHandleCollections(urlSchemes);
                    }

                    if (commandLineArgs != null)
                    {
                        config.CommandLineArgs = new Dictionary<string, string>();
                        foreach (var item in commandLineArgs)
                        {
                            config.CommandLineArgs.Add(item.name, item.value);
                        }
                    }

                    if (customSettings != null)
                    {
                        config.CustomSettings = CustomSettingLocal.GetSettings(customSettings);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.Error(exception);
                }
            }

            private WindowState GetWindowState(string state)
            {
                if (string.IsNullOrWhiteSpace(state))
                {
                    return WindowState.Normal;
                }

                switch (state.ToUpper())
                {
                    case WindowStateOption.NORMAL:
                        return WindowState.Normal;
                    case WindowStateOption.MAXIMIZE:
                        return WindowState.Maximize;
                    case WindowStateOption.FULLSCREEN:
                        return WindowState.Fullscreen;
                }

                return WindowState.Normal;
            }

            private List<ControllerAssemblyInfo> GetAssemblyInfos(List<string> controllerAssemblies)
            {
                var assemblyInfos = new List<ControllerAssemblyInfo>();
                if (controllerAssemblies == null || !controllerAssemblies.Any())
                {
                    return assemblyInfos;
                }

                assemblyInfos.RegisterServiceAssemblies(controllerAssemblies);

                return assemblyInfos;
            }
        }
        private static class UtilityLocal
        {
            public static object Create(string fullyQualifiedName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(fullyQualifiedName))
                    {
                        var objectType = Type.GetType(fullyQualifiedName);
                        return Activator.CreateInstance(objectType);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.Error(exception);
                }

                return null;
            }

            public static string EnsureFilePath(string file)
            {
                if (string.IsNullOrWhiteSpace(file)) return file;

                if (!File.Exists(file))
                {
                    // If local file
                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    file = Path.Combine(appDirectory, file);
                    if (!File.Exists(file))
                    {
                        return null;
                    }
                }

                return file;
            }
        }
    }
}
