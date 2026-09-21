using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class String : TokenBase, IValueToken
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.String;

        /// <inheritdoc cref="IValueToken.Value"/>
        public object Value => GetValue(RawValue);

        /// <inheritdoc cref="IValueToken.ValueType"/>
        public Type ValueType => typeof(string);

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_String;

        private char QuoteChar { get; set; }

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        public String() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances, also called via reflection by the parse engine
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public String(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new String(rawValue, tokenPos);

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

            if(rawSource[pos].In('\'', '"'))
            {
                QuoteChar = rawSource[pos++];
                sb.Clear();
                sb.Append(QuoteChar);

                for (; pos < rawSource.Length; pos++)
                {
                    sb.Append(rawSource[pos]);
                    if (rawSource[pos] == QuoteChar)
                    {
                        rawValue = sb.ToString();
                        return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
                    }
                }
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: false);
        }

        /// <summary>
        /// Returns the string value of a token's raw value
        /// </summary>
        /// <param name="rawValue">A token's raw value</param>
        /// <returns>The string value</returns>
        public static string GetValue(string rawValue)
        {
            return (rawValue.StartsWith("\"") && rawValue.EndsWith("\"")) || (rawValue.StartsWith("'") && rawValue.EndsWith("'"))
                ? rawValue.Substring(1, rawValue.Length - 2)
                : rawValue;
        }
    }
}
