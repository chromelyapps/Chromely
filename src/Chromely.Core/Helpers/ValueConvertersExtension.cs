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

namespace Chromely.Core.Helpers
{
    using Chromely.Core.Infrastructure;
    using System;

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
