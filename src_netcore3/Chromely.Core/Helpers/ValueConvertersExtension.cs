// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueConvertersExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
        /// The enum to string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EnumToString(this CefEventKey key)
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
