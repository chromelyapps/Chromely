// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonReader.cs" company="Chromely">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public enum JsonToken
    {
        None,

        ObjectStart,
        PropertyName,
        ObjectEnd,

        ArrayStart,
        ArrayEnd,

        Int,
        Long,
        Double,

        String,

        Boolean,
        Null
    }

    /// <summary>
    /// The json reader.
    /// Stream-like access to JSON text.
    /// </summary>
    public class JsonReader
    {
        #region Fields
        private static IDictionary<int, IDictionary<int, int[]>> parse_table;

        private Stack<int> automaton_stack;
        private int current_input;
        private int current_symbol;

        private Lexer lexer;
        private bool parser_in_string;
        private bool parser_return;
        private bool read_started;
        private TextReader reader;
        private bool reader_is_owned;

        #endregion


        #region Public Properties
        public bool AllowComments
        {
            get { return lexer.AllowComments; }
            set { lexer.AllowComments = value; }
        }

        public bool AllowSingleQuotedStrings
        {
            get { return lexer.AllowSingleQuotedStrings; }
            set { lexer.AllowSingleQuotedStrings = value; }
        }

        public bool SkipNonMembers { get; set; }

        public bool EndOfInput { get; private set; }

        public bool EndOfJson { get; private set; }

        public JsonToken Token { get; private set; }

        public object Value { get; private set; }

        #endregion


        #region Constructors
        static JsonReader()
        {
            PopulateParseTable();
        }

        public JsonReader(string json_text) :
            this(new StringReader(json_text), true)
        {
        }

        public JsonReader(TextReader reader) :
            this(reader, false)
        {
        }

        private JsonReader(TextReader reader, bool owned)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            parser_in_string = false;
            parser_return = false;

            read_started = false;
            automaton_stack = new Stack<int>();
            automaton_stack.Push((int)ParserToken.End);
            automaton_stack.Push((int)ParserToken.Text);

            lexer = new Lexer(reader);

            this.EndOfInput = false;
            this.EndOfJson = false;

            this.SkipNonMembers = true;

            this.reader = reader;
            reader_is_owned = owned;
        }

        #endregion


        #region Static Methods
        private static void PopulateParseTable()
        {
            // See section A.2. of the manual for details
            parse_table = new Dictionary<int, IDictionary<int, int[]>>();

            TableAddRow(ParserToken.Array);
            TableAddCol(ParserToken.Array, '[',
                         '[',
                         (int)ParserToken.ArrayPrime);

            TableAddRow(ParserToken.ArrayPrime);
            TableAddCol(ParserToken.ArrayPrime, '"',
                         (int)ParserToken.Value,

                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, '[',
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, ']',
                         ']');
            TableAddCol(ParserToken.ArrayPrime, '{',
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, (int)ParserToken.Number,
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, (int)ParserToken.True,
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, (int)ParserToken.False,
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');
            TableAddCol(ParserToken.ArrayPrime, (int)ParserToken.Null,
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest,
                         ']');

            TableAddRow(ParserToken.Object);
            TableAddCol(ParserToken.Object, '{',
                         '{',
                         (int)ParserToken.ObjectPrime);

            TableAddRow(ParserToken.ObjectPrime);
            TableAddCol(ParserToken.ObjectPrime, '"',
                         (int)ParserToken.Pair,
                         (int)ParserToken.PairRest,
                         '}');
            TableAddCol(ParserToken.ObjectPrime, '}',
                         '}');

            TableAddRow(ParserToken.Pair);
            TableAddCol(ParserToken.Pair, '"',
                         (int)ParserToken.String,
                         ':',
                         (int)ParserToken.Value);

            TableAddRow(ParserToken.PairRest);
            TableAddCol(ParserToken.PairRest, ',',
                         ',',
                         (int)ParserToken.Pair,
                         (int)ParserToken.PairRest);
            TableAddCol(ParserToken.PairRest, '}',
                         (int)ParserToken.Epsilon);

            TableAddRow(ParserToken.String);
            TableAddCol(ParserToken.String, '"',
                         '"',
                         (int)ParserToken.CharSeq,
                         '"');

            TableAddRow(ParserToken.Text);
            TableAddCol(ParserToken.Text, '[',
                         (int)ParserToken.Array);
            TableAddCol(ParserToken.Text, '{',
                         (int)ParserToken.Object);

            TableAddRow(ParserToken.Value);
            TableAddCol(ParserToken.Value, '"',
                         (int)ParserToken.String);
            TableAddCol(ParserToken.Value, '[',
                         (int)ParserToken.Array);
            TableAddCol(ParserToken.Value, '{',
                         (int)ParserToken.Object);
            TableAddCol(ParserToken.Value, (int)ParserToken.Number,
                         (int)ParserToken.Number);
            TableAddCol(ParserToken.Value, (int)ParserToken.True,
                         (int)ParserToken.True);
            TableAddCol(ParserToken.Value, (int)ParserToken.False,
                         (int)ParserToken.False);
            TableAddCol(ParserToken.Value, (int)ParserToken.Null,
                         (int)ParserToken.Null);

            TableAddRow(ParserToken.ValueRest);
            TableAddCol(ParserToken.ValueRest, ',',
                         ',',
                         (int)ParserToken.Value,
                         (int)ParserToken.ValueRest);
            TableAddCol(ParserToken.ValueRest, ']',
                         (int)ParserToken.Epsilon);
        }

        private static void TableAddCol(ParserToken row, int col,
                                         params int[] symbols)
        {
            parse_table[(int)row].Add(col, symbols);
        }

        private static void TableAddRow(ParserToken rule)
        {
            parse_table.Add((int)rule, new Dictionary<int, int[]>());
        }

        #endregion


        #region Private Methods
        private void ProcessNumber(string number)
        {
            if (number.IndexOf('.') != -1 ||
                number.IndexOf('e') != -1 ||
                number.IndexOf('E') != -1)
            {

                double n_double;
                if (double.TryParse(number, out n_double))
                {
                    this.Token = JsonToken.Double;
                    this.Value = n_double;

                    return;
                }
            }

            int n_int32;
            if (int.TryParse(number, out n_int32))
            {
                this.Token = JsonToken.Int;
                this.Value = n_int32;

                return;
            }

            long n_int64;
            if (long.TryParse(number, out n_int64))
            {
                this.Token = JsonToken.Long;
                this.Value = n_int64;

                return;
            }

            ulong n_uint64;
            if (ulong.TryParse(number, out n_uint64))
            {
                this.Token = JsonToken.Long;
                this.Value = n_uint64;

                return;
            }

            // Shouldn't happen, but just in case, return something
            this.Token = JsonToken.Int;
            this.Value = 0;
        }

        private void ProcessSymbol()
        {
            if (current_symbol == '[')
            {
                this.Token = JsonToken.ArrayStart;
                parser_return = true;

            }
            else if (current_symbol == ']')
            {
                this.Token = JsonToken.ArrayEnd;
                parser_return = true;

            }
            else if (current_symbol == '{')
            {
                this.Token = JsonToken.ObjectStart;
                parser_return = true;

            }
            else if (current_symbol == '}')
            {
                this.Token = JsonToken.ObjectEnd;
                parser_return = true;

            }
            else if (current_symbol == '"')
            {
                if (parser_in_string)
                {
                    parser_in_string = false;

                    parser_return = true;

                }
                else
                {
                    if (this.Token == JsonToken.None)
                        this.Token = JsonToken.String;

                    parser_in_string = true;
                }

            }
            else if (current_symbol == (int)ParserToken.CharSeq)
            {
                this.Value = lexer.StringValue;

            }
            else if (current_symbol == (int)ParserToken.False)
            {
                this.Token = JsonToken.Boolean;
                this.Value = false;
                parser_return = true;

            }
            else if (current_symbol == (int)ParserToken.Null)
            {
                this.Token = JsonToken.Null;
                parser_return = true;

            }
            else if (current_symbol == (int)ParserToken.Number)
            {
                ProcessNumber(lexer.StringValue);

                parser_return = true;

            }
            else if (current_symbol == (int)ParserToken.Pair)
            {
                this.Token = JsonToken.PropertyName;

            }
            else if (current_symbol == (int)ParserToken.True)
            {
                this.Token = JsonToken.Boolean;
                this.Value = true;
                parser_return = true;

            }
        }

        private bool ReadToken()
        {
            if (this.EndOfInput)
                return false;

            lexer.NextToken();

            if (lexer.EndOfInput)
            {
                Close();

                return false;
            }

            current_input = lexer.Token;

            return true;
        }

        #endregion


        public void Close()
        {
            if (this.EndOfInput)
                return;

            this.EndOfInput = true;
            this.EndOfJson = true;

            if (reader_is_owned)
                reader.Close();

            reader = null;
        }

        public bool Read()
        {
            if (this.EndOfInput)
                return false;

            if (this.EndOfJson)
            {
                this.EndOfJson = false;
                automaton_stack.Clear();
                automaton_stack.Push((int)ParserToken.End);
                automaton_stack.Push((int)ParserToken.Text);
            }

            parser_in_string = false;
            parser_return = false;

            this.Token = JsonToken.None;
            this.Value = null;

            if (!read_started)
            {
                read_started = true;

                if (!ReadToken())
                    return false;
            }


            int[] entry_symbols;

            while (true)
            {
                if (parser_return)
                {
                    if (automaton_stack.Peek() == (int)ParserToken.End)
                        this.EndOfJson = true;

                    return true;
                }

                current_symbol = automaton_stack.Pop();

                ProcessSymbol();

                if (current_symbol == current_input)
                {
                    if (!ReadToken())
                    {
                        if (automaton_stack.Peek() != (int)ParserToken.End)
                            throw new JsonException(
                                "Input doesn't evaluate to proper JSON text");

                        if (parser_return)
                            return true;

                        return false;
                    }

                    continue;
                }

                try
                {

                    entry_symbols =
                        parse_table[current_symbol][current_input];

                }
                catch (KeyNotFoundException e)
                {
                    throw new JsonException((ParserToken)current_input, e);
                }

                if (entry_symbols[0] == (int)ParserToken.Epsilon)
                    continue;

                for (int i = entry_symbols.Length - 1; i >= 0; i--)
                    automaton_stack.Push(entry_symbols[i]);
            }
        }

    }
}