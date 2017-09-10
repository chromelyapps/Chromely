#region Header
/**
 * JsonException.cs
 *   Base class throwed by LitJSON when a parsing error occurs.
 *
 * The authors disclaim copyright to this source code. For more details, see
 * the COPYING file included with this distribution.
 **/
#endregion

#region Port Info
/**
 * This is a port of LitJson to .NET Standard for Chromely. Mostly provided as-is. 
 * For more info: https://github.com/lbv/litjson
 **/
#endregion

using System;


namespace LitJson
{
    public class JsonException : ApplicationException
    {
        public JsonException() : base()
        {
        }

        internal JsonException(ParserToken token) :
            base(String.Format(
                    "Invalid token '{0}' in input string", token))
        {
        }

        internal JsonException(ParserToken token,
                                Exception inner_exception) :
            base(String.Format(
                    "Invalid token '{0}' in input string", token),
                inner_exception)
        {
        }

        internal JsonException(int c) :
            base(String.Format(
                    "Invalid character '{0}' in input string", (char)c))
        {
        }

        internal JsonException(int c, Exception inner_exception) :
            base(String.Format(
                    "Invalid character '{0}' in input string", (char)c),
                inner_exception)
        {
        }


        public JsonException(string message) : base(message)
        {
        }

        public JsonException(string message, Exception inner_exception) :
            base(message, inner_exception)
        {
        }
    }
}