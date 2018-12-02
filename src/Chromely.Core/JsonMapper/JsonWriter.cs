// ReSharper disable once StyleCop.SA1649

// --------------------------------------------------------------------------------------------------------------------
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
    using System.Globalization;
    using System.IO;
    using System.Text;

    internal enum Condition
    {
        InArray,
        InObject,
        NotAProperty,
        Property,
        Value
    }

    internal class WriterContext
    {
        public int Count;
        public bool InArray;
        public bool InObject;
        public bool ExpectingValue;
        public int Padding;
    }

    /// <summary>
    /// The json writer.
    /// Stream-like facility to output JSON text.
    /// </summary>
    public class JsonWriter
    {
        #region Fields
        private static NumberFormatInfo number_format;

        private WriterContext context;
        private Stack<WriterContext> ctx_stack;
        private bool has_reached_end;
        private char[] hex_seq;
        private int indentation;
        private int indent_value;
        private StringBuilder inst_string_builder;

        #endregion


        #region Properties
        public int IndentValue
        {
            get { return indent_value; }
            set
            {
                indentation = (indentation / indent_value) * value;
                indent_value = value;
            }
        }

        public bool PrettyPrint { get; set; }

        public TextWriter TextWriter { get; }

        public bool Validate { get; set; }

        #endregion


        #region Constructors
        static JsonWriter()
        {
            number_format = NumberFormatInfo.InvariantInfo;
        }

        public JsonWriter()
        {
            inst_string_builder = new StringBuilder();
            this.TextWriter = new StringWriter(inst_string_builder);

            Init();
        }

        public JsonWriter(StringBuilder sb) :
            this(new StringWriter(sb))
        {
        }

        public JsonWriter(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            this.TextWriter = writer;

            Init();
        }

        #endregion


        #region Private Methods
        private void DoValidation(Condition cond)
        {
            if (!context.ExpectingValue)
                context.Count++;

            if (!this.Validate)
                return;

            if (has_reached_end)
                throw new JsonException(
                    "A complete JSON symbol has already been written");

            switch (cond)
            {
                case Condition.InArray:
                    if (!context.InArray)
                        throw new JsonException(
                            "Can't close an array here");
                    break;

                case Condition.InObject:
                    if (!context.InObject || context.ExpectingValue)
                        throw new JsonException(
                            "Can't close an object here");
                    break;

                case Condition.NotAProperty:
                    if (context.InObject && !context.ExpectingValue)
                        throw new JsonException(
                            "Expected a property");
                    break;

                case Condition.Property:
                    if (!context.InObject || context.ExpectingValue)
                        throw new JsonException(
                            "Can't add a property here");
                    break;

                case Condition.Value:
                    if (!context.InArray &&
                        (!context.InObject || !context.ExpectingValue))
                        throw new JsonException(
                            "Can't add a value here");

                    break;
            }
        }

        private void Init()
        {
            has_reached_end = false;
            hex_seq = new char[4];
            indentation = 0;
            indent_value = 4;
            this.PrettyPrint = false;
            this.Validate = true;

            ctx_stack = new Stack<WriterContext>();
            context = new WriterContext();
            ctx_stack.Push(context);
        }

        private static void IntToHex(int n, char[] hex)
        {
            int num;

            for (int i = 0; i < 4; i++)
            {
                num = n % 16;

                if (num < 10)
                    hex[3 - i] = (char)('0' + num);
                else
                    hex[3 - i] = (char)('A' + (num - 10));

                n >>= 4;
            }
        }

        private void Indent()
        {
            if (this.PrettyPrint)
                indentation += indent_value;
        }


        private void Put(string str)
        {
            if (this.PrettyPrint && !context.ExpectingValue)
                for (int i = 0; i < indentation; i++)
                    this.TextWriter.Write(' ');

            this.TextWriter.Write(str);
        }

        private void PutNewline()
        {
            PutNewline(true);
        }

        private void PutNewline(bool add_comma)
        {
            if (add_comma && !context.ExpectingValue &&
                context.Count > 1)
                this.TextWriter.Write(',');

            if (this.PrettyPrint && !context.ExpectingValue)
                this.TextWriter.Write('\n');
        }

        private void PutString(string str)
        {
            Put(string.Empty);

            this.TextWriter.Write('"');

            int n = str.Length;
            for (int i = 0; i < n; i++)
            {
                switch (str[i])
                {
                    case '\n':
                        this.TextWriter.Write("\\n");
                        continue;

                    case '\r':
                        this.TextWriter.Write("\\r");
                        continue;

                    case '\t':
                        this.TextWriter.Write("\\t");
                        continue;

                    case '"':
                    case '\\':
                        this.TextWriter.Write('\\');
                        this.TextWriter.Write(str[i]);
                        continue;

                    case '\f':
                        this.TextWriter.Write("\\f");
                        continue;

                    case '\b':
                        this.TextWriter.Write("\\b");
                        continue;
                }

                if ((int)str[i] >= 32 && (int)str[i] <= 126)
                {
                    this.TextWriter.Write(str[i]);
                    continue;
                }

                // Default, turn into a \uXXXX sequence
                IntToHex((int)str[i], hex_seq);
                this.TextWriter.Write("\\u");
                this.TextWriter.Write(hex_seq);
            }

            this.TextWriter.Write('"');
        }

        private void Unindent()
        {
            if (this.PrettyPrint)
                indentation -= indent_value;
        }

        #endregion


        public override string ToString()
        {
            if (inst_string_builder == null)
                return string.Empty;

            return inst_string_builder.ToString();
        }

        public void Reset()
        {
            has_reached_end = false;

            ctx_stack.Clear();
            context = new WriterContext();
            ctx_stack.Push(context);

            if (inst_string_builder != null)
                inst_string_builder.Remove(0, inst_string_builder.Length);
        }

        public void Write(bool boolean)
        {
            DoValidation(Condition.Value);
            PutNewline();

            Put(boolean ? "true" : "false");

            context.ExpectingValue = false;
        }

        public void Write(decimal number)
        {
            DoValidation(Condition.Value);
            PutNewline();

            Put(Convert.ToString(number, number_format));

            context.ExpectingValue = false;
        }

        public void Write(double number)
        {
            DoValidation(Condition.Value);
            PutNewline();

            string str = Convert.ToString(number, number_format);
            Put(str);

            if (str.IndexOf('.') == -1 &&
                str.IndexOf('E') == -1)
                this.TextWriter.Write(".0");

            context.ExpectingValue = false;
        }

        public void Write(int number)
        {
            DoValidation(Condition.Value);
            PutNewline();

            Put(Convert.ToString(number, number_format));

            context.ExpectingValue = false;
        }

        public void Write(long number)
        {
            DoValidation(Condition.Value);
            PutNewline();

            Put(Convert.ToString(number, number_format));

            context.ExpectingValue = false;
        }

        public void Write(string str)
        {
            DoValidation(Condition.Value);
            PutNewline();

            if (str == null)
                Put("null");
            else
                PutString(str);

            context.ExpectingValue = false;
        }

        [CLSCompliant(false)]
        public void Write(ulong number)
        {
            DoValidation(Condition.Value);
            PutNewline();

            Put(Convert.ToString(number, number_format));

            context.ExpectingValue = false;
        }

        public void WriteArrayEnd()
        {
            DoValidation(Condition.InArray);
            PutNewline(false);

            ctx_stack.Pop();
            if (ctx_stack.Count == 1)
                has_reached_end = true;
            else
            {
                context = ctx_stack.Peek();
                context.ExpectingValue = false;
            }

            Unindent();
            Put("]");
        }

        public void WriteArrayStart()
        {
            DoValidation(Condition.NotAProperty);
            PutNewline();

            Put("[");

            context = new WriterContext();
            context.InArray = true;
            ctx_stack.Push(context);

            Indent();
        }

        public void WriteObjectEnd()
        {
            DoValidation(Condition.InObject);
            PutNewline(false);

            ctx_stack.Pop();
            if (ctx_stack.Count == 1)
                has_reached_end = true;
            else
            {
                context = ctx_stack.Peek();
                context.ExpectingValue = false;
            }

            Unindent();
            Put("}");
        }

        public void WriteObjectStart()
        {
            DoValidation(Condition.NotAProperty);
            PutNewline();

            Put("{");

            context = new WriterContext();
            context.InObject = true;
            ctx_stack.Push(context);

            Indent();
        }

        public void WritePropertyName(string property_name)
        {
            DoValidation(Condition.Property);
            PutNewline();

            PutString(property_name);

            if (this.PrettyPrint)
            {
                if (property_name.Length > context.Padding)
                    context.Padding = property_name.Length;

                for (int i = context.Padding - property_name.Length;
                     i >= 0; i--)
                    this.TextWriter.Write(' ');

                this.TextWriter.Write(": ");
            }
            else
                this.TextWriter.Write(':');

            context.ExpectingValue = true;
        }
    }
}