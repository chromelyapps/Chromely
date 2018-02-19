/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using LitJson;

    public static class ExtensionMethods
    {
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

        public static string EnsureResponseIsJsonFormat(this object obj)
        {
            JsonData newJsonData = new JsonData();

            try
            {
                JsonData jsonData = JsonMapper.ToObject(obj.EnsureJson());
                if (jsonData.IsArray)
                {
                    return jsonData.ToJson();
                }

                ICollection<string> keys = null;
                try { keys = jsonData.Keys; } catch (Exception) {}

                if ((keys == null) || (keys.Count == 0))
                {
                    newJsonData = new JsonData();
                    newJsonData["Data"] = jsonData.ToJson();
                    return newJsonData.ToJson();
                }

                return jsonData.ToJson();
            }
            catch (Exception)
            {
                // Swallow
            }

            newJsonData = new JsonData();
            newJsonData["Data"] = obj.ToString();

            return newJsonData.ToJson();
        }


        public static int ArrayCount(this object obj)
        {
            try
            {
                JsonData jsonData = JsonMapper.ToObject(obj.EnsureJson());
                if (jsonData.IsArray)
                {
                    return jsonData.Count;
                }
            }
            catch (Exception ex)
            {
                // Swallow
            }

            return 0;
        }

        public static IDictionary<string, object> ToObjectDictionary(this object obj)
        {
            try
            {
                if (obj is IDictionary<string, object>)
                {
                    return (IDictionary<string, object>)obj;
                }

                if (obj is IDictionary<string, string>)
                {
                    IDictionary<string, string> dict = (IDictionary<string, string>)obj;
                    IDictionary<string, object> responseDic = new Dictionary<string, object>();
                    foreach (var item in dict)
                    {
                        responseDic.Add(item.Key, item.Value);
                    }

                    return responseDic;
                }

                // If json
                if (obj is string)
                {
                    try
                    {
                        JsonData jsonData = JsonMapper.ToObject(obj.ToString());
                        if (!jsonData.IsArray && jsonData.IsObject)
                        {
                            int count = jsonData.Count;
                            string[] arrayKeys = new string[count];

                            // Copy the list to the array.
                            IDictionary<string, object> responseDic = new Dictionary<string, object>();
                            jsonData.Keys.CopyTo(arrayKeys, 0);
                            for (int i = 0; i < count; i++)
                            {
                               responseDic.Add(arrayKeys[i], jsonData[arrayKeys[i]]);
                            }

                            return responseDic;
                        }

                    }
                    catch (Exception exception)
                    {
                    }
                }

                return obj.ObjectToDictionary();
            }
            catch(Exception)
            {
            }

            return new Dictionary<string, object>();
        }

        public static IDictionary<string, object> ObjectToDictionary(this object source)
        {
            return source.ObjectToDictionary<object>();
        }

        public static IDictionary<string, T> ObjectToDictionary<T>(this object source)
        {
            if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object value = property.GetValue(source);
                if (IsOfType<T>(value))
                {
                    dictionary.Add(property.Name, (T)value);
                }
            }
            return dictionary;
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new NullReferenceException("Unable to convert anonymous object to a dictionary. The source anonymous object is null.");
        }
    }
}
