using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents operators such as mathematical and comparison used in calculating expressions, e.g. '+', '=', '%', etc.
    /// </summary>
    public class Operator : ParseableToken
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Operator;

        /// <summary>
        /// The type of operator
        /// </summary>
        public OperatorType OperatorType { get; }

        /// <inheritdoc cref="ParseableToken.Priority"/>
        public override int Priority => ParserPriority_Operator;

        public override string PatternIdentifier { get; }

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal Operator() : base() { }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public Operator(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
            Process(rawValue, out OperatorType type, out string patternIdentifier);
            OperatorType = type == OperatorType.Undefined ? throw new ExpressionException("Unrecognized operator") : type;
            PatternIdentifier = patternIdentifier;
        }

        /// <inheritdoc cref="ParseableToken.CreateInstance(string, int)"/>
        public override ParseableToken CreateInstance(string rawValue, int tokenPos) =>
            new Operator(rawValue, tokenPos);

        /// <inheritdoc cref="ParseableToken.Parse(ReadOnlySpan{char}, ref int, IEnumerable{TokenBase}, out string, out int)"/>
        public override bool Parse
        (
            ReadOnlySpan<char> rawSource,
            ref int pos,
            IEnumerable<TokenBase> tokens,
            out string rawValue,
            out int startPos
        )
        {
            startPos = pos;

            Process(rawSource[pos], out OperatorType type, out _);

            if (type == OperatorType.Undefined)
            {
                rawValue = string.Empty;
                return false;
            }

            char? next = Next(rawSource, pos);
            sb.Clear();
            sb.Append(rawSource[pos++]);

            Process(next, out type, out _);

            // is this a one-character operator?
            if (type == OperatorType.Undefined)
            {
                rawValue = sb.ToString();
                return true;
            }

            // this is a two-character operator
            pos++;
            sb.Append(next ?? throw new ThisShouldNeverHappenException("next was null"));
            rawValue = sb.ToString();
            return true;
        }

        public override string ToString() =>
            $"{Type} {{ Pos = {TokenPos}, OperatorType = {OperatorType.ToDisplayString()}, Text = {RawValue.ToDisplayString()} }}";

        public static void Process(char? c, out OperatorType type, out string patternIdentifier)
        {
            type = OperatorType.Undefined;
            patternIdentifier = c.HasValue ? new string(c.Value, 1) : string.Empty;
            switch (c)
            {
                case '+':  // plus
                    type = OperatorType.Add; break;
                case '-':  // 002D minus
                case '−':  // 2212 minus
                    type = OperatorType.Subtract;
                    patternIdentifier = "-"; break;
                case '*':  // 002A multiply
                case '×':  // 00D7 multiply
                case '·':  // 00B7 multiply
                    type = OperatorType.Multiply;
                    patternIdentifier = "×"; break;
                case '/':  // 002F divide
                case '∕':  // 2215 divide
                case '÷':  // 00F7 divide
                    type = OperatorType.Divide;
                    patternIdentifier = "÷"; break;
                case '%':  // modulus
                    type = OperatorType.Modulus; break;
                case '=':
                    type = OperatorType.Equal; break;
                case '!':
                    type = OperatorType.Not; break;
                case '<':
                    type = OperatorType.LessThan; break;
                case '>':
                    type = OperatorType.GreaterThan; break;
            }
        }

        public static void Process(string oper, out OperatorType type, out string patternIdentifier)
        {
            if (string.IsNullOrEmpty(oper))
                throw new ThisShouldNeverHappenException("raw or blank operator");

            patternIdentifier = oper;

            switch (oper.Length)
            {
                case 1:
                    Process(oper[0], out type, out patternIdentifier); return;
                case 2:
                    switch (oper)
                    {
                        case "!=":
                        case "<>":
                            type = OperatorType.NotEqual; return;
                        case ">=":
                            type = OperatorType.GreaterThanOrEqual; return;
                        case "<=":
                            type = OperatorType.LessThanOrEqual; return;
                        default:
                            throw new ExpressionException("unrecognized operator");
                    }
                default:
                    throw new ThisShouldNeverHappenException("operator exceeds max chars");
            }
        }
    }
}
