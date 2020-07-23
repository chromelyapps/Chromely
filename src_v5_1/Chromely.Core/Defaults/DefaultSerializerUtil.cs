// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;

namespace Chromely.Core.Defaults
{
    public class DefaultSerializerUtil : IChromelySerializerUtil
    {
        private JsonSerializerOptions _serializerOptions;

        public DefaultSerializerUtil(JsonSerializerOptions serializerOptions = null)
        {
            _serializerOptions = serializerOptions;
        }

        public JsonSerializerOptions SerializerOptions 
        { 
            get
            {
                if (_serializerOptions == null)
                {
                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;
                    _serializerOptions = options;
                }

                return _serializerOptions;
            }

            set
            {
                _serializerOptions = value;
            }
        }

        public string ObjectToJson(object value)
        {
            try
            {
                if (value == null)
                {
                    return null;
                }

                if (value.IsOfType<string>() && value.ToString().IsValidJson())
                {
                    return value.ToString();
                }

                return JsonSerializer.Serialize(value, SerializerOptions);
            }
            catch (Exception)
            {
                // Swallow
            }

            return value.ToString();
        }

        public string EnsureResponseDataIsJson(object value)
        {
            try
            {
                if (value == null)
                {
                    var returnNullData = new { Data = value };
                    return JsonSerializer.Serialize(returnNullData, SerializerOptions);
                }

                if (!value.IsOfType<string>())
                {
                    return JsonSerializer.Serialize(value, SerializerOptions);
                }

                if (value.ToString().IsValidJson())
                {
                    return JsonSerializer.Serialize(value, SerializerOptions);
                }

                var returnData = new { Data = value };
                return JsonSerializer.Serialize(returnData, SerializerOptions);
            }
            catch (Exception)
            {
                // Swallow
            }

            return value.ToString();
        }

        public IDictionary<string, object> JsonToArray(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<IDictionary<string, object>>(json, SerializerOptions);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            return null;
        }

        public IDictionary<string, string> ObjectToDictionary(object value)
        {
            try
            {
                if (value == null) return null;

                if (value.IsOfType<string>() && value.ToString().IsValidJson())
                {
                    return JsonSerializer.Deserialize<IDictionary<string, string>>(value.ToString(), SerializerOptions);
                }

                var dict = value as IDictionary<string, string>;
                if (dict != null)
                {
                    var dictRes = new Dictionary<string, string>();
                    foreach (var item in dict)
                    {
                        dictRes.Add(item.Key, item.Value);
                    }
                    return dictRes;
                }

                dict = value as IDictionary<string, string>;
                if (dict != null)
                {
                    return dict;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}
