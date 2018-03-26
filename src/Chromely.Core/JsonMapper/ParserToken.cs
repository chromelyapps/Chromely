// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserToken.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// The authors disclaim copyright to this source code. For more details, see
// the COPYING file included with this distribution @ https://github.com/lbv/litjson
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
//
// This is a port of LitJson to.NET Standard for Chromely.Mostly provided as-is. 
// For more info: https://github.com/lbv/litjson
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable All
namespace LitJson
{
    /// <summary>
    /// The parser token.
    /// Internal representation of the tokens used by the lexer and the parser.
    /// </summary>
    internal enum ParserToken
    {
        // Lexer tokens (see section A.1.1. of the manual)
        None = char.MaxValue + 1,

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