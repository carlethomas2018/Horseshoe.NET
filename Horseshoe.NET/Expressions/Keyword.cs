using Horseshoe.NET.Expressions.Tokens;

namespace Horseshoe.NET.Expressions
{
    public class Keyword : TokenBase, IValueToken
    {
        /// <summary>
        /// Used by the parsing engine to determine what type of token is being passed in if it matches (see <see cref="Is(string)")./>
        /// </summary>
        public override TokenType Type => TokenType.Word;

        /// <inheritdoc cref="IValueToken.ReturnValue"/>
        public object ReturnValue { get; private set; }

        /// <inheritdoc cref="IValueToken.ReturnValue"/>
        public System.Type ReturnType { get; private set; }



        public KeywordType KeywordType { get; }

        public Keyword(string rawValue) : base(rawValue)
        {
            KeywordType = GetKeywordType(rawValue);
        }

        /// <summary>
        /// Chooses an keyword type matching the raw input
        /// </summary>
        /// <param name="rawValue">The original text value parsed from the source text</param>
        /// <returns>A matching keyword type, or Undefined</returns>
        public KeywordType GetKeywordType(string rawValue)
        {
            switch (rawValue.ToUpper())
            {
                case "LOWDATE":
                    return KeywordType.LowDate;
                case "HIGHDATE":
                    return KeywordType.HighDate;
            }

            return KeywordType.Undefined;
        }

        public override string ToString()
        {
            return $"{Type} {{ KeywordType = \"{KeywordType}\", Value = \"{RawValue}\" }}";
        }
    }
}
