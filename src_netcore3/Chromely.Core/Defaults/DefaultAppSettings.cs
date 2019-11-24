using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Chromely.Core.Defaults
{
    public class DefaultAppSettings : IChromelyAppSettings
    {
        private ChromelyDynamic _chromelyDynamic;
        public string AppName { get; set; }
        public string DataPath { get; set; }

        public dynamic Settings 
        { 
            get
            {
                if (_chromelyDynamic == null)
                {
                    _chromelyDynamic = new ChromelyDynamic();
                }

                return _chromelyDynamic;
            }
        }

        public virtual void Read(IChromelyConfiguration config)
        {
            try
            {
                var appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, AppName);

                if (appSettingsFile == null)
                {
                    return;
                }

                var info = new FileInfo(appSettingsFile);
                if ((info.Exists) && info.Length > 0)
                {
                    using (StreamReader jsonReader = new StreamReader(appSettingsFile))
                    {
                        string json = jsonReader.ReadToEnd();
                        var options = new JsonSerializerOptions();
                        options.ReadCommentHandling = JsonCommentHandling.Skip;
                        options.AllowTrailingCommas = true;

                        var settingsJsonElement = JsonSerializer.Deserialize<JsonElement>(json, options);
                        var settingsDic = JsonToDictionary(settingsJsonElement);
                        _chromelyDynamic = new ChromelyDynamic(settingsDic);
                    }
                }

                 if (File.Exists(appSettingsFile))
                {
                    DataPath = appSettingsFile;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        public virtual void Save(IChromelyConfiguration config)
        {
            try
            {
                if (_chromelyDynamic == null || _chromelyDynamic.Empty)
                {
                    return;
                }

                var appSettingsFile = DataPath;

                if (string.IsNullOrWhiteSpace(appSettingsFile))
                {
                    appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, AppName, true);
                }

                if (appSettingsFile == null)
                {
                    return;
                }

                using (StreamWriter streamWriter = File.CreateText(appSettingsFile))
                {
                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;

                    var jsonDic = JsonSerializer.Serialize(_chromelyDynamic.Dictionary, options);
                    streamWriter.Write(jsonDic);

                    Logger.Instance.Log.Info("AppSettings FileName:" + appSettingsFile);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        private IDictionary<string, object> JsonToDictionary(JsonElement jsonElement)
        {
            var dic = new Dictionary<string, object>();

            foreach (var jsonProperty in jsonElement.EnumerateObject())
            {
                switch (jsonProperty.Value.ValueKind)
                {
                    case JsonValueKind.Null:
                        dic.Add(jsonProperty.Name, null);
                        break;
                    case JsonValueKind.Number:
                        dic.Add(jsonProperty.Name, jsonProperty.Value.GetDouble());
                        break;
                    case JsonValueKind.False:
                        dic.Add(jsonProperty.Name, false);
                        break;
                    case JsonValueKind.True:
                        dic.Add(jsonProperty.Name, true);
                        break;
                    case JsonValueKind.Undefined:
                        dic.Add(jsonProperty.Name, null);
                        break;
                    case JsonValueKind.String:
                        var strValue = jsonProperty.Value.GetString();
                        if (DateTime.TryParse(strValue, out DateTime date))
                        {
                            dic.Add(jsonProperty.Name, date);
                        }
                        else
                        {
                            dic.Add(jsonProperty.Name, strValue);
                        }

                        break;
                    case JsonValueKind.Object:
                        dic.Add(jsonProperty.Name, JsonToDictionary(jsonProperty.Value));
                        break;
                    case JsonValueKind.Array:
                        ArrayList objectList = new ArrayList();
                        foreach (JsonElement item in jsonProperty.Value.EnumerateArray())
                        {
                            switch (item.ValueKind)
                            {
                                case JsonValueKind.Null:
                                    objectList.Add(null);
                                    break;
                                case JsonValueKind.Number:
                                    objectList.Add(item.GetDouble());
                                    break;
                                case JsonValueKind.False:
                                    objectList.Add(false);
                                    break;
                                case JsonValueKind.True:
                                    objectList.Add(true);
                                    break;
                                case JsonValueKind.Undefined:
                                    objectList.Add(null);
                                    break;
                                case JsonValueKind.String:
                                    var itemValue = item.GetString();
                                    if (DateTime.TryParse(itemValue, out DateTime itemDate))
                                    {
                                        objectList.Add(itemDate);
                                    }
                                    else
                                    {
                                        objectList.Add(itemValue);
                                    }

                                    break;
                                default:
                                    objectList.Add(JsonToDictionary(item));
                                    break;
                            }
                        }
                        dic.Add(jsonProperty.Name, objectList);
                        break;
                }
           }

            return dic;
        }
    }
}
