namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents operators such as mathematical and comparison used in calculating expressions, e.g. '+', '=', '%', etc.
    /// </summary>
    public class Scope : TokenBase
    {
        /// <summary>
        /// Used by the parsing engine to determine what type of token is being passed in if it matches (see <see cref="Is(string)")./>
        /// </summary>
        public override TokenType Type => TokenType.Scope;   

        /// <summary>
        /// The type of scope
        /// </summary>
        public ScopeType ScopeType { get; }

        public Scope(string rawValue) : base(rawValue)
        {
            ScopeType = SelectType(rawValue);
        }

        /// <summary>
        /// Chooses an operator type matching the raw input
        /// </summary>
        /// <param name="rawValue">The original text value parsed from the source text</param>
        /// <returns>A matching operator type, or Undefined</returns>
        public ScopeType SelectType(string rawValue)
        {
            switch (rawValue)
            {
                case "(":
                    return ScopeType.Start;
                case ")":
                    return ScopeType.End;
                case ",":
                    return ScopeType.Separator;
            }
            return ScopeType.Undefined;
        }

        public override string ToString()
        {
            return $"{Type} {{ ScopeType = \"{ScopeType}\", Value = \"{RawValue}\" }}";
        }
    }
}
