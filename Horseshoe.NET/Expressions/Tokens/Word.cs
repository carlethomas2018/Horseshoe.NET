using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class Word : TokenBase
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Word;

        /// <inheritdoc cref="ITokenParser.Priority"/>
        public override int Priority => ParserPriority_Word;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier => "VK";

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal Word() : base() { }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public Word(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new Number(rawValue, tokenPos);

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
                IsLetter(c) ||
                (c == '_' && (IsLetter(next1) || IsDigit(next1))) ||
                (c == '_' && next1 == '_' && (IsLetter(next2) || IsDigit(next2)))
            )
            {
                sb.Clear();
                sb.Append(c);
                pos++;

                for (; pos < rawSource.Length; pos++)
                {
                    c = rawSource[pos];

                    if (IsLetter(c) || IsDigit(c) || c == '_')
                    {
                        sb.Append(c);
                        continue;
                    }
                }

                rawValue = sb.ToString();
                return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: false);
        }

        public static bool IsDigit(char? c) =>
            c.HasValue && c.Value.Between('0', '9');

        public static bool IsLetter(char? c) =>
            c.HasValue && ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
    }
}
