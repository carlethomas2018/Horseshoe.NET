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
        public OperatorType OperatorType => GetType(RawValue);

        /// <inheritdoc cref="ParseableToken.Priority"/>
        public override int Priority => ParserPriority_Operator;

        public override string PatternIdentifier => throw new NotImplementedException();

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

            if (IsOperator(rawSource[pos], out bool isSingleCharOperator))
            {
                sb.Clear();
                sb.Append(rawSource[pos++]);

                if (isSingleCharOperator)
                {
                    // tokenize the single-char operator parsed up this point
                    rawValue = sb.ToString();
                    return true;
                }

                for (; pos < rawSource.Length; pos++)
                {
                    if (IsOperator(rawSource[pos], out isSingleCharOperator))
                    {
                        // tokenize the multi-char operator parsed up this point and then
                        // pass the buck to the next round of parsing to separately tokenize this single-char operator
                        if (isSingleCharOperator)
                        {
                            rawValue = sb.ToString();
                            return true;
                        }
                        sb.Append(rawSource[pos]);
                    }
                    else break;
                }

                // tokenize the multi-char operator parsed up this point
                rawValue = sb.ToString();
                return true;
            }

            rawValue = string.Empty;
            return false;
        }

        public override string ToString() =>
            $"{Type} {{ Pos = {TokenPos}, OperatorType = {OperatorType.ToDisplayString()}, Text = {RawValue.ToDisplayString()} }}";

        public static bool IsOperator(char c, out bool isSingleCharOperator)
        {
            isSingleCharOperator = false;
            switch (c)
            {
                case '+':  // plus
                case '-':  // 002D minus
                case '−':  // 2212 minus
                case '*':  // 002A multiply
                case '×':  // 00D7 multiply
                case '·':  // 00B7 multiply
                case '/':  // 002F divide
                case '∕':  // 2215 divide
                case '÷':  // 00F7 divide
                case '%':  // modulus
                    isSingleCharOperator = true;
                    return true;
                case '=':
                case '!':
                case '<':
                case '>':
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Chooses an operator type matching the raw value
        /// </summary>
        /// <param name="rawValue">The original text value parsed from the source text</param>
        /// <returns>A matching operator type, or Undefined</returns>
        public static OperatorType GetType(string rawValue)
        {
            switch (rawValue)
            {
                case "+":
                    return OperatorType.Add;
                case "-":  // 002D
                case "−":  // 2212
                    return OperatorType.Subtract;
                case "*":  // 002A
                case "×":  // 00D7
                case "·":  // 00B7
                    return OperatorType.Multiply;
                case "/":  // 002F
                case "∕":  // 2215
                case "÷":  // 00F7
                    return OperatorType.Divide;
                case "%":
                    return OperatorType.Modulus;
                case "=":
                    return OperatorType.Equal;
                case "!=":
                case "<>":
                    return OperatorType.NotEqual;
                case ">":
                    return OperatorType.GreaterThan;
                case ">=":
                    return OperatorType.GreaterThanOrEqual;
                case "<":
                    return OperatorType.LessThan;
                case "<=":
                    return OperatorType.LessThanOrEqual;
            }
            return OperatorType.Undefined;
        }
    }
}
