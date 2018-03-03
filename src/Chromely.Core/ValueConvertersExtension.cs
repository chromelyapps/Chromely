using Chromely.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core
{
    public static class ValueConvertersExtension
    {
        public static bool TryParseBoolean(this object value, out bool result)
        {
            result = false;

            try
            {
                if (value == null)
                {
                    return false;
                }

                if (value is bool)
                {
                    result = (bool)value;
                    return true;
                }

                if (bool.TryParse(value.ToString(), out result))
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        public static bool TryParseInteger(this object value, out int result)
        {
            result = 0;

            try
            {
                if (value == null)
                {
                    return false;
                }

                if (value is int)
                {
                    result = (int)value;
                    return true;
                }

                if (int.TryParse(value.ToString(), out result))
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        public static bool TryParseString(this object value, out string result)
        {
            result = string.Empty;

            try
            {
                if (value == null)
                {
                    return false;
                }

                if (value is string)
                {
                    result = (string)value;
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
    }
}
