using Horseshoe.NET.Collections;
using System;
using System.Collections.Generic;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class Date : TokenBase, IValueToken
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Date;

        /// <inheritdoc cref="IValueToken.Value"/>
        public object Value => GetValue(RawValue);

        /// <inheritdoc cref="IValueToken.ValueType"/>
        public Type ValueType => typeof(DateTime);

        /// <inheritdoc cref="TokenBase.Priority"/>
        public override int Priority => ParserPriority_Date;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier => "DT";

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal Date() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public Date(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        /// <inheritdoc cref="TokenBase.CreateInstance(string, int)"/>
        public override TokenBase CreateInstance(string rawValue, int tokenPos) =>
            new Date(rawValue, tokenPos);

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

            if (rawSource[pos] == '#')
            {
                sb.Clear();
                sb.Append(rawSource[pos]);

                for (; pos < rawSource.Length; pos++)
                {
                    sb.Append(rawSource[pos]);
                    if (rawSource[pos] == '#')
                    {
                        rawValue = sb.ToString();
                        return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: true);
                    }
                }
            }

            rawValue = string.Empty;
            return RelayMethodReturningValue(message: string.Format("pos={0}, rawValue={1}", pos, rawValue.ToDisplayString()), returnValue: false);
        }

        public static DateTime GetValue(string rawValue)
        {
            return rawValue.StartsWith("#") && rawValue.EndsWith("#")
                ? DateTime.Parse(rawValue.Substring(1, rawValue.Length - 2))
                : DateTime.Parse(rawValue);
        }
    }
}
