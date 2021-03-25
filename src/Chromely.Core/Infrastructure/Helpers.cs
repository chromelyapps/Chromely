// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

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
        public static IDictionary<string, string> GetParameters(this string url, string referrer = null)
        {
            var result = new Dictionary<string, string>();
            var nameValueCollection = new NameValueCollection();

            string querystring = string.Empty;
            int index = url.IndexOf('?');
            if (index > 0)
            {
                querystring = url.Substring(url.IndexOf('?'));
                nameValueCollection = HttpUtility.ParseQueryString(querystring);
                if(nameValueCollection != null && nameValueCollection.Count > 0)
                {
                    result = nameValueCollection.AllKeys.ToDictionary(x => x, x => nameValueCollection[x]);
                }
            }

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
