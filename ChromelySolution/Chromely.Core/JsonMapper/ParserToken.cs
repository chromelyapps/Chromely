#region Header
/**
 * ParserToken.cs
 *   Internal representation of the tokens used by the lexer and the parser.
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

namespace LitJson
{
    internal enum ParserToken
    {
        // Lexer tokens (see section A.1.1. of the manual)
        None = System.Char.MaxValue + 1,
        Number,
        True,
        False,
        Null,
        CharSeq,
        // Single char
        Char,

        // Parser Rules (see section A.2.1 of the manual)
        Text,
        Object,
        ObjectPrime,
        Pair,
        PairRest,
        Array,
        ArrayPrime,
        Value,
        ValueRest,
        String,

        // End of input
        End,

        // The empty rule
        Epsilon
    }
}