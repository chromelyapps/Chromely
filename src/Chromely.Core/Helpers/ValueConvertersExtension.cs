// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueConvertersExtension.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Helpers
{
    using System;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The value converters extension.
    /// </summary>
    public static class ValueConvertersExtension
    {
        /// <summary>
        /// The try parse boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseBoolean(this object value, out bool result)
        {
            result = false;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case bool boolValue:
                        result = boolValue;
                        return true;
                }

                return bool.TryParse(value.ToString(), out result);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        /// <summary>
        /// The try parse integer.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseInteger(this object value, out int result)
        {
            result = 0;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case int intValue:
                        result = intValue;
                        return true;
                }

                return int.TryParse(value.ToString(), out result);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        /// <summary>
        /// The try parse string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseString(this object value, out string result)
        {
            result = string.Empty;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case string strValue:
                        result = strValue;
                        return true;
                }

                result = value.ToString();
                return true; 
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        /// <summary>
        /// The enum to string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EnumToString(this CefHandlerKey key)
        {
            return Enum.GetName(key.GetType(), key);
        }

        /// <summary>
        /// The to enum.
        /// </summary>
        /// <param name="enumValue">
        /// The enum value.
        /// </param>
        /// <typeparam name="TEnumType">
        /// Enum type to return.
        /// </typeparam>
        /// <returns>
        /// The enum to return.
        /// </returns>
        public static TEnumType ToEnum<TEnumType>(this string enumValue)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), enumValue);
        }
    }
}
