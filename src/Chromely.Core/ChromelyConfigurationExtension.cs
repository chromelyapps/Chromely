// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core.Infrastructure;

namespace Chromely.Core
{
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
        public static bool GetBooleanValue(this IChromelyConfiguration config, string key, bool defaultValue = false)
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

                    if (bool.TryParse(value.ToString(), out var result))
                    {
                        return result;
                    }
                }

                return defaultValue;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets string value from custom settings dictionary.
        /// </summary>
        /// <param name="config">
        /// The config object - instance of the <see cref="IChromelyConfiguration"/> class.
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
        public static string GetStringValue(this IChromelyConfiguration config, string key, string defaultValue = "")
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
                Logger.Instance.Log.Error(exception);
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
        public static int GetIntegerValue(this IChromelyConfiguration config, string key, int defaultValue = 0)
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

                    if (int.TryParse(value.ToString(), out var result))
                    {
                        return result;
                    }
                }

                return defaultValue;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return defaultValue;
        }
    }
}
