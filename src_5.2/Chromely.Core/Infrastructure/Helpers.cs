// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.ComponentModel;

namespace Chromely.Core.Infrastructure
{
    public static class Helpers
    {
        /// <summary>
        /// The object to dictionary.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public static IDictionary<string, string> ToDictionary(this object source)
        {
            var result = new Dictionary<string, string>();
            var objToDic =  source.ToDictionary<object>();
            if (objToDic != null)
            {
                foreach (var item in objToDic)
                {
                    result.Add(item.Key, item.Value?.ToString());
                }
            }

            return result;
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
        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
            {
                return null;
            }

            var dictionary = source as IDictionary<string, T>;
            if (dictionary != null)
            {
                return dictionary;
            }

            dictionary = new Dictionary<string, T>();
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

        public static object JsonToObject(this JsonElement jsonElement)
        {
            object result = jsonElement;
            try
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.Null:
                        result = null;
                        break;
                    case JsonValueKind.Number:
                        result = jsonElement.GetDouble();
                        break;
                    case JsonValueKind.False:
                        result = false;
                        break;
                    case JsonValueKind.True:
                        result = true;
                        break;
                    case JsonValueKind.Undefined:
                        result = null;
                        break;
                    case JsonValueKind.String:
                        var strValue = jsonElement.GetString();
                        if (DateTime.TryParse(strValue, out DateTime date))
                        {
                            result = date;
                        }
                        else
                        {
                            result = strValue;
                        }

                        break;
                    case JsonValueKind.Object:
                        result = jsonElement.JsonToDictionary();
                        break;
                    case JsonValueKind.Array:
                        ArrayList objectList = new ArrayList();
                        foreach (JsonElement item in jsonElement.EnumerateArray())
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
                        result = objectList;
                        break;
                }
            }
            catch (Exception exception)
            {
            }

            return result;
        }

        public static IDictionary<string, object> JsonToDictionary(this JsonElement jsonElement)
        {
            var dic = new Dictionary<string, object>();

            try
            {
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return dic;
        }

        public static string GetPathFromUrl(this string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.AbsolutePath;
            }
            catch { }

            return url;
        }

        public static IDictionary<string, object> GetParameters(this IDictionary<string, string> parameters, string referrer = null)
        {
            var paremetersWithList = new Dictionary<string, object>();
            if (parameters != null && parameters.Any())
            {
                foreach (var item in parameters)
                {
                    paremetersWithList[item.Key] = new List<object>
                    {
                        item.Value
                    };
                }
            }

            return paremetersWithList;
        }


        /// <summary>
        /// The get parameters.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="referrer">
        /// The referrer.
        /// </param>
        /// <returns>
        /// The name value collection.
        /// </returns>
        public static IDictionary<string, object> GetParameters(this string url, string referrer = null)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var pathAndQuery = new PathAndQuery();
                pathAndQuery.Parse(url);
                if (pathAndQuery.QueryParameters != null && pathAndQuery.QueryParameters.Any())
                {
                    foreach (var item in pathAndQuery.QueryParameters)
                    {
                        if (!result.ContainsKey(item.Key))
                        {
                            result.Add(item.Key, item.Value);
                        }
                    }
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(referrer))
            {
                if (result.ContainsKey(RequestConstants.Referrer))
                {
                    var guidStr = Guid.NewGuid().ToString();
                    result.Add($"{RequestConstants.Referrer}_{guidStr}", referrer);
                }
                else
                {
                    result.Add(RequestConstants.Referrer, referrer);
                }
            }

            return result;
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

        public static string FlattenException(this Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static IChromelyResponse ErrorResponse(this Exception exception, string requestId)
        {
            var response = new ChromelyResponse(requestId);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.BadRequest;
            response.StatusText = "Bad Request";
            response.Data = exception?.FlattenException();

            return response;
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
        public static bool IsOfType<T>(this object value)
        {
            if (value == null)
            {
                return false;
            }

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
    }
}
