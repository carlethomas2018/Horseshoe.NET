using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents a mathematical constant, i.e. <c>π</c> (pi).
    /// </summary>
    public class MathematicalConstant : Number
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        public override NumberType NumberType { get; } = NumberType.MathematicalConstant;

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_MathematicalConstant;

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal MathematicalConstant() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public MathematicalConstant(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new MathematicalConstant(rawValue, tokenPos);

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

            if (IsMathematicalConstant(c))
            {
                pos++;
                rawValue = new string(c, 1);
                return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(returnValue: false);
        }

        public static bool IsMathematicalConstant(char c)
        {
            switch (c)
            {
                case 'Π': // 03A0 pi
                case 'ᴨ': // 1D28 pi
                case 'π': // 03C0 pi
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="Number.GetValue(string)"/>
        public new static double GetValue(string rawValue) 
        {
            switch (rawValue)
            {
                case "Π": // 03A0 pi
                case "ᴨ": // 1D28 pi
                case "π": // 03C0 pi
                    return Math.PI;
            }
            throw new ExpressionException(string.Format(Lang.Get("Token.Parse.Unexpected.{value}.{type}"), rawValue.ToDisplayString(), typeof(MathematicalConstant).Name));
        }
    }
}
