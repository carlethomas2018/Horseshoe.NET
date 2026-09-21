using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// A number or mathematical constant
    /// </summary>
    public class NumericLiteral : Number
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        public override NumberType NumberType { get; } = NumberType.Literal;

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_Number;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override char PatternIdentifier => 'N';

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal NumericLiteral() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public NumericLiteral(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new NumericLiteral(rawValue, tokenPos);

        /// <inheritdoc cref="TokenBase.Parse(ReadOnlySpan{char}, ref int, IEnumerable{TokenBase}, out string, out int)"/>
        public override bool Parse
        (
            ReadOnlySpan<char> rawSource,
            ref int pos,
            IEnumerable<TokenBase> tokens,
            out string rawValue,
            out int startPos
        )
        {
            RelayMethodEntered(paramsAndArgs: new Dictionary<string, object>
            {
                [nameof(rawSource)] = rawSource.ToString(),
                [nameof(pos)] = pos,
                [nameof(tokens)] = CollectionUtil.ToCountAndLastString(tokens)
            });

            startPos = pos;
            char c = rawSource[pos];
            (char? next1, char? next2) = Next2(rawSource, pos);

            if 
            (
                IsDigit(c) ||
                (c == '.' && IsDigit(next1)) ||
                (c == '-' && (IsDigit(next1) || (next1 == '.' && IsDigit(next2))))
            )
            {
                sb.Clear();
                sb.Append(c);
                bool hasDecimal = false;
                pos++;

                if (c == '.')
                    hasDecimal = true;

                for (; pos < rawSource.Length; pos++)
                {
                    c = rawSource[pos];
                    (next1, _) = Next2(rawSource, pos);

                    if (c == '.')
                    {
                        if (hasDecimal || !IsDigit(next1))
                            throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{char}.{type}"), c, GetType().Name));
                        hasDecimal = true;
                    }
                    
                    if (!IsDigit(c))
                    {
                        rawValue = sb.ToString();
                        return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
                    }

                    sb.Append(c);
                }
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: false);
        }

        public static bool IsDigit(char? c) =>
            c.HasValue && c.Value.Between('0', '9');

        /// <summary>
        /// Calculates a token's numeric value based on its raw value
        /// </summary>
        /// <param name="rawValue">A token's raw value</param>
        /// <returns>The numeric value, or Undefined</returns>
        public override double GetValue() 
        {
            try
            {
                return double.Parse(RawValue);
            }
            catch (FormatException fmtex)
            {
                throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{value}.{type}"), RawValue.ToDisplayString(), typeof(NumericLiteral).Name), fmtex);
            }
        }
    }
}
