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
    using LitJson;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public static class ExtensionMethods
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            try
            {
                if (obj is IDictionary<string, object>)
                {
                    return (IDictionary<string, object>)obj;
                }

                // If json
                if (obj is string)
                {
                    try
                    {
                        JsonData jsonData = JsonMapper.ToObject(obj.ToString());
                        return jsonData.ToDictionary();
                    }
                    catch (Exception)
                    { }
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
