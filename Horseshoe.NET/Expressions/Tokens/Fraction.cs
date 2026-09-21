using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents one of a specific subset of unicode symbols for fractional numbers.
    /// </summary>
    public class Fraction : Number
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        public override NumberType NumberType { get; } = NumberType.FractionalLiteral;

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_FractionalNumber;

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal Fraction() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public Fraction(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new Fraction(rawValue, tokenPos);

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
                case '⅛':
                case '¼':
                case '⅓':
                case '½':
                case '⅝':
                case '⅔':
                case '¾':
                case '⅞':
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="Number.GetValue(string)"/>
        public new static double GetValue(string rawValue) 
        {
            switch (rawValue)
            {
                case "⅛":
                    return 0.125;
                case "¼":
                    return 0.25;
                case "⅓":
                    return 1.0 / 3.0;
                case "½":
                    return 0.5;
                case "⅝":
                    return 0.625;
                case "⅔":
                    return 2.0 / 3.0;
                case "¾":
                    return 0.75;
                case "⅞":
                    return 0.875;
            }
            throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{value}.{type}"), rawValue.ToDisplayString(), typeof(Fraction).Name));
        }
    }
}
