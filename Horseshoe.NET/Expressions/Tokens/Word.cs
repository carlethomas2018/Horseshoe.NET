using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class Word : TokenBase, ITokenParser
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Word;

        /// <inheritdoc cref="ITokenParser.Priority"/>
        public int Priority => TokenBase.ParserPriority_Word;

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

        public bool Parse
        (
            StringBuilder sb,
            char c,
            ref int startPos,
            int curPos,
            ReadOnlySpan<char> next32,
            List<TokenBase> tokens,
            ref bool readingString,
            ref bool readingDate,
            ref bool readingNumber,
            ref bool readingWord,
            ref bool readingCompoundOperator,
            ref bool customFlag1,
            ref bool customFlag2
        )
        {
            if (readingWord)
            {
                // a little housekeeping
                readingString = false;
                readingDate = false;
                readingNumber = false;
                readingCompoundOperator = false;

                if (IsWordChar(c))
                {
                    sb.Append(c);
                    return true;
                }
                else
                {
                    tokens.Add(new Word(sb.ToString(), startPos));
                    sb.Clear();
                    readingWord = false;
                    return false;
                }
            }

            // pass the buck to the string, date or number parser, if applicable
            if (readingString || readingDate || (readingNumber && char.IsDigit(c)))
            {
                return false;
            }

            if (IsWordChar(c, isStartingChar: true))
            {
                if (sb.Length > 0)
                {
                    if (readingNumber)
                        throw new ExpressionException(Lang.Get("Token.Word.StartingChar"));
                    if (readingCompoundOperator)
                    {
                        tokens.Add(new Operator(sb.ToString(), startPos));
                    }
                }

                sb.Clear();
                sb.Append(c);
                readingWord = true;
                startPos = curPos;

                // a little housekeeping
                readingString = false;
                readingDate = false;
                readingNumber = false;
                readingCompoundOperator = false;
            }
            else
            {

            }

            // pass the buck to the next parser
            return false;
        }

        public static bool IsWordChar(char c, bool isStartingChar = false)
        {
            if (c == '_')
                return true;

            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                return true;

            if (!isStartingChar && char.IsDigit(c))
                return true;

            return false;
        }
    }
}
