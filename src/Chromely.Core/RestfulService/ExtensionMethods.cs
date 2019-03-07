// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    using LitJson;

    /// <summary>
    /// The extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Ensure object is returned in json format.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EnsureJson(this object obj)
        {
            try
            {
               return JsonMapper.ToJson(obj);
            }
            catch (Exception)
            {
                // Swallow
            }

            return obj.ToString();
        }

        /// <summary>
        /// Ensure response is json format.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EnsureResponseIsJsonFormat(this object obj)
        {
            JsonData newJsonData;

            try
            {
                var jsonData = JsonMapper.ToObject(obj.EnsureJson());
                if (jsonData.IsArray)
                {
                    return jsonData.ToJson();
                }

                ICollection<string> keys = null;
                try
                {
                    keys = jsonData.Keys;
                }
                catch (Exception)
                {
                    // ignored
                }

                if ((keys == null) || (keys.Count == 0))
                {
                    newJsonData = new JsonData { ["Data"] = jsonData.ToJson() };
                    return newJsonData.ToJson();
                }

                return jsonData.ToJson();
            }
            catch (Exception)
            {
                // Swallow
            }

            newJsonData = new JsonData { ["Data"] = obj.ToString() };
            return newJsonData.ToJson();
        }

        /// <summary>
        /// Gets the object array count.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ArrayCount(this object obj)
        {
            try
            {
                var jsonData = JsonMapper.ToObject(obj.EnsureJson());
                if (jsonData.IsArray)
                {
                    return jsonData.Count;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return 0;
        }

        /// <summary>
        /// The to object dictionary.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public static IDictionary<string, object> ToObjectDictionary(this object obj)
        {
            try
            {
                if (obj is IDictionary<string, object> objects)
                {
                    return objects;
                }

                if (obj is IDictionary<string, string> dict)
                {
                    return dict.ToDictionary<KeyValuePair<string, string>, string, object>(item => item.Key, item => item.Value);
                }

                if (obj is JsonData jsonDataObj)
                {
                    if (jsonDataObj.IsObject)
                    {
                        var objDic = new Dictionary<string, object>();
                        foreach (var key in jsonDataObj.Keys)
                        {
                            objDic[key] = jsonDataObj[key];
                        }

                        return objDic;
                    }
                }

                // If json
                if (obj is string)
                {
                    try
                    {
                        var jsonData = JsonMapper.ToObject(obj.ToString());
                        if (!jsonData.IsArray && jsonData.IsObject)
                        {
                            var count = jsonData.Count;
                            string[] arrayKeys = new string[count];

                            // Copy the list to the array.
                            var responseDic = new Dictionary<string, object>();
                            jsonData.Keys.CopyTo(arrayKeys, 0);
                            for (int i = 0; i < count; i++)
                            {
                               responseDic.Add(arrayKeys[i], jsonData[arrayKeys[i]]);
                            }

                            return responseDic;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                return obj.ObjectToDictionary();
            }
            catch (Exception)
            {
                // ignored
            }

            return new Dictionary<string, object>();
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
    }
}