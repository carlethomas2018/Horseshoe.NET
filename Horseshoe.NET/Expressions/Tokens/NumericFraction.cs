using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents one of a specific subset of unicode symbols for fractional numbers.
    /// </summary>
    public class NumericFraction : NumericBase
    {
        public override NumberType NumberType { get; } = NumberType.NumericFractional;

        /// <inheritdoc cref="ParseableToken.Priority"/>
        public override int Priority => ParserPriority_Fraction;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier => "NR";

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal NumericFraction() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public NumericFraction(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="ParseableToken.CreateInstance(string, int)"/>
        public override ParseableToken CreateInstance(string rawValue, int tokenPos) =>
            new NumericFraction(rawValue, tokenPos);

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

            if (IsFraction(c))
            {
                pos++;
                rawValue = new string(c, 1);
                return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(returnValue: false);
        }

        public static bool IsFraction(char c)
        {
            switch (c)
            { 
                case '⅛':  // 1/8
                case '¼':  // 1/4
                case '⅓':  // 1/3
                case '½':  // 1/2
                case '⅝':  // 5/8
                case '⅔':  // 2/3
                case '¾':  // 3/4
                case '⅞':  // 7/8
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="NumericBase.GetValue(string)"/>
        public override double GetValue() 
        {
            switch (RawValue)
            {
                case "⅛":  // 1/8
                    return 0.125;
                case "¼":  // 1/4
                    return 0.25;
                case "⅓":  // 1/3
                    return 1.0 / 3.0;
                case "½":  // 1/2
                    return 0.5;
                case "⅝":  // 5/8
                    return 0.625;
                case "⅔":  // 2/3
                    return 2.0 / 3.0;
                case "¾":  // 3/4
                    return 0.75;
                case "⅞":  // 7/8
                    return 0.875;
            }
            throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{value}.{type}"), RawValue.ToDisplayString(), typeof(NumericFraction).Name));
        }
    }
}
