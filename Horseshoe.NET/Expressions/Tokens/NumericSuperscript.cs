using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents one of a specific subset of unicode symbols for fractional numbers.
    /// </summary>
    public class NumericSuperscript : NumericBase
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        public override NumberType NumberType { get; } = NumberType.Superscript;

        /// <inheritdoc cref="ParseableToken.Priority"/>
        public override int Priority => ParserPriority_NumericalSuperscript;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier => "NS";

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal NumericSuperscript() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public NumericSuperscript(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="ParseableToken.CreateInstance(string, int)"/>
        public override ParseableToken CreateInstance(string rawValue, int tokenPos) =>
            new NumericSuperscript(rawValue, tokenPos);

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
            RelayMethodEntered(paramsAndArgs: new Dictionary<string, object>
            {
                [nameof(rawSource)] = rawSource.ToString(),
                [nameof(pos)] = pos,
                [nameof(tokens)] = CollectionUtil.ToCountAndLastString(tokens)
            });

            startPos = pos;
            char c = rawSource[pos];

            if (IsSuperscript(c))
            {
                pos++;
                rawValue = new string(c, 1);
                return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(returnValue: false);
        }

        public static bool IsSuperscript(char c)
        {
            switch (c)
            { 
                case '⁰':  // 2070
                case '¹':  // 00B9
                case '²':  // 00B2
                case '³':  // 00B3
                case '⁴':  // 2074
                case '⁵':  // 2075
                case '⁶':  // 2076
                case '⁷':  // 2077
                case '⁸':  // 2078
                case '⁹':  // 2079
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="NumericBase.GetValue(string)"/>
        public override double GetValue() 
        {
            switch (RawValue)
            {
                case "⁰":  // 2070
                    return 0;
                case "¹":  // 00B9
                    return 1;
                case "²":  // 00B2
                    return 2;
                case "³":  // 00B3
                    return 3;
                case "⁴":  // 2074
                    return 4;
                case "⁵":  // 2075
                    return 5;
                case "⁶":  // 2076
                    return 6;
                case "⁷":  // 2077
                    return 7;
                case "⁸":  // 2078
                    return 8;
                case "⁹":  // 2079
                    return 9;
            }
            throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{value}.{type}"), RawValue.ToDisplayString(), typeof(Fraction).Name));
        }
    }
}
