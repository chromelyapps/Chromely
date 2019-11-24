// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Web;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.Network
{
    class ReturnedData
    {
        public object Data { get; set; }
    }

    public static class ExtensionMethods
    {
        public static string ToJson(this object value)
        {
            try
            {
                if (IsOfType<string>(value) && IsValidJson(value.ToString()))
                {
                    return value.ToString();
                }

                var options = new JsonSerializerOptions();
                options.ReadCommentHandling = JsonCommentHandling.Skip;
                options.AllowTrailingCommas = true;

                return JsonSerializer.Serialize(value);
            }
            catch (Exception)
            {
                // Swallow
            }

            return value.ToString();
        }

        public static string EnsureResponseDataIsJsonFormat(this object value)
        {
            try
            {
                if (value == null)
                {
                    var returnNullData = new ReturnedData() { Data = value };
                    return JsonSerializer.Serialize(returnNullData, JsonSerializingOption);
                }

                if (!IsOfType<string>(value))
                {
                    return JsonSerializer.Serialize(value, JsonSerializingOption);
                }

                if (IsValidJson(value.ToString()))
                {
                    return JsonSerializer.Serialize(value, JsonSerializingOption);
                }

                var returnData = new ReturnedData() { Data = value };
                return JsonSerializer.Serialize(returnData, JsonSerializingOption);
            }
            catch (Exception)
            {
                // Swallow
            }

            return value.ToString();
        }

        public static IDictionary<string, object> JsonToArray(this string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<IDictionary<string, object>>(json, JsonSerializingOption);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return null;
        }

        public static IDictionary<string, string> ToObjectDictionary(this object value)
        {
            try
            {
                if (value == null) return null;

                if (IsOfType<string>(value) && IsValidJson(value.ToString()))
                {
                    return JsonSerializer.Deserialize<IDictionary<string, string>>(value.ToString(), JsonSerializingOption);
                }

                var dict1 = value as IDictionary<string, string>;
                if (dict1 != null)
                {
                    var dict1Res = new Dictionary<string, string>();
                    foreach (var item in dict1)
                    {
                        dict1Res.Add(item.Key, item.Value);
                    }
                    return dict1Res;
                }

                var dict2 = value as IDictionary<string, string>;
                if (dict2 != null)
                {
                    return dict2;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        private static IDictionary<string, string> JsonElementToDictionary(JsonElement jsonElement)
        {
            var dic = new Dictionary<string, string>();

            foreach (var jsonProperty in jsonElement.EnumerateObject())
            {
                dic.Add(jsonProperty.Name, jsonProperty.Value.ToString());
            }

            return dic;
        }

        private static T2 IDictionary<T1, T2>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The object to dictionary.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public static IDictionary<string, object> ObjectToDictionary(this object source)
        {
            return source.ObjectToDictionary<object>();
        }

        /// <summary>
        /// The object to dictionary.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// Object type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public static IDictionary<string, T> ObjectToDictionary<T>(this object source)
        {
            if (source == null)
            {
                throw new NullReferenceException("Unable to convert anonymous object to a dictionary. The source anonymous object is null.");
            }

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                var value = property.GetValue(source);
                if (IsOfType<T>(value))
                {
                    dictionary.Add(property.Name, (T)value);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// The get parameters.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The name value collection.
        /// </returns>
        public static IDictionary<string, string> GetParameters(this string url)
        {
            var nameValueCollection = new NameValueCollection();

            string querystring = string.Empty;
            int index = url.IndexOf('?');
            if (index > 0)
            {
                querystring = url.Substring(url.IndexOf('?'));
                nameValueCollection = HttpUtility.ParseQueryString(querystring);
            }

            if (string.IsNullOrEmpty(querystring))
            {
                return new Dictionary<string, string>();
            }

            return nameValueCollection.AllKeys.ToDictionary(x => x, x => nameValueCollection[x]);
        }

        public static string JavaScriptStringEncode(this string value)
        {
            return JavaScriptStringEncode(value, false);
        }

        // https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs
        public static string JavaScriptStringEncode(this string value, bool addDoubleQuotes)
        {
            if (String.IsNullOrEmpty(value))
                return addDoubleQuotes ? "\"\"" : String.Empty;

            int len = value.Length;
            bool needEncode = false;
            char c;
            for (int i = 0; i < len; i++)
            {
                c = value[i];

                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
                {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
                return addDoubleQuotes ? "\"" + value + "\"" : value;

            var sb = new System.Text.StringBuilder();
            if (addDoubleQuotes)
                sb.Append('"');

            for (int i = 0; i < len; i++)
            {
                c = value[i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                    sb.AppendFormat("\\u{0:x4}", (int)c);
                else switch ((int)c)
                    {
                        case 8:
                            sb.Append("\\b");
                            break;

                        case 9:
                            sb.Append("\\t");
                            break;

                        case 10:
                            sb.Append("\\n");
                            break;

                        case 12:
                            sb.Append("\\f");
                            break;

                        case 13:
                            sb.Append("\\r");
                            break;

                        case 34:
                            sb.Append("\\\"");
                            break;

                        case 92:
                            sb.Append("\\\\");
                            break;

                        default:
                            sb.Append(c);
                            break;
                    }
            }

            if (addDoubleQuotes)
                sb.Append('"');

            return sb.ToString();
        }

        /// <summary>
        /// The is of type.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// Object type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public static bool IsValidJson(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                return true;
            }

            return false;
        }

       private static JsonSerializerOptions JsonSerializingOption
       {
            get
            {
                var options = new JsonSerializerOptions();
                options.ReadCommentHandling = JsonCommentHandling.Skip;
                options.AllowTrailingCommas = true;

                return options;
            }
       }
    }
}