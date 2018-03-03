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

namespace Chromely.Core
{
    using System;
    using Chromely.Core.Infrastructure;

    public static class ChromelyConfigurationExtension
    {
        public static bool GetBooleanValue(this ChromelyConfiguration config, string key, bool defaultValue = false)
        {
            try
            {
                bool result = false;

                if ((config != null) &&
                    (config.CustomSettings != null) &&
                    (config.CustomSettings.ContainsKey(key)))
                {
                    object value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is bool)
                    {
                        return (bool)value;
                    }

                    if (bool.TryParse(value.ToString(), out result))
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

        public static string GetStringValue(this ChromelyConfiguration config, string key, string defaultValue = "")
        {
            try
            {
                if ((config != null) &&
                    (config.CustomSettings != null) &&
                    (config.CustomSettings.ContainsKey(key)))
                {
                    object value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is string)
                    {
                        return (string)value;
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

        public static int GetIntegerValue(this ChromelyConfiguration config, string key, int defaultValue = 0)
        {
            try
            {
                int result = 0;

                if ((config != null) &&
                    (config.CustomSettings != null) &&
                    (config.CustomSettings.ContainsKey(key)))
                {
                    object value = config.CustomSettings[key];
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    if (value is int)
                    {
                        return (int)value;
                    }

                    if (int.TryParse(value.ToString(), out result))
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
