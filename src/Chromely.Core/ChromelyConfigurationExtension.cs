// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationExtension.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ChromelyConfigurationExtension
    {
        /// <summary>
        /// Gets boolean value from custom settings dictionary.
        /// </summary>
        /// <param name="config">
        /// The config object - instance of the <see cref="ChromelyConfiguration"/> class.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> value.
        /// </returns>
        public static bool GetBooleanValue(this ChromelyConfiguration config, string key, bool defaultValue = false)
        {
            try
            {
                if (config?.CustomSettings != null && config.CustomSettings.ContainsKey(key))
                {
                    var value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is bool boolValue)
                    {
                        return boolValue;
                    }

                    if (bool.TryParse(value.ToString(), out var result))
                    {
                        return result;
                    }
                }

                return defaultValue;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets string value from custom settings dictionary.
        /// </summary>
        /// <param name="config">
        /// The config object - instance of the <see cref="ChromelyConfiguration"/> class.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> value.
        /// </returns>
        public static string GetStringValue(this ChromelyConfiguration config, string key, string defaultValue = "")
        {
            try
            {
                if (config?.CustomSettings != null && config.CustomSettings.ContainsKey(key))
                {
                    var value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is string strValue)
                    {
                        return strValue;
                    }

                    return value.ToString();
                }

                return defaultValue;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets integer value from custom settings dictionary.
        /// </summary>
        /// <param name="config">
        /// The config object - instance of the <see cref="ChromelyConfiguration"/> class.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> value.
        /// </returns>
        public static int GetIntegerValue(this ChromelyConfiguration config, string key, int defaultValue = 0)
        {
            try
            {
                if (config?.CustomSettings != null && config.CustomSettings.ContainsKey(key))
                {
                    var value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is int intValue)
                    {
                        return intValue;
                    }

                    if (int.TryParse(value.ToString(), out var result))
                    {
                        return result;
                    }
                }

                return defaultValue;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return defaultValue;
        }
    }
}
