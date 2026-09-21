using System;
using System.Collections.Generic;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents the whitespace between tokens, not an actual token
    /// </summary>
    public class Whitespace : TokenBase
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Whitespace;   

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_Whitespace;

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal Whitespace() : base() { }

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
            rawValue = string.Empty;
            startPos = pos;

            // decide whether to process this token and where to start
            for (; pos < rawSource.Length; pos++)
            {
                if (rawSource[pos] == ' ')                              // `  `,  `  Cos()`
                    continue;                                           //  01 ,   0123456
                break;
            }

            // pass the buck to the next parser
            return false;
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            throw new NotImplementedException();
    }
}
