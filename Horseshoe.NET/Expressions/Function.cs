using Horseshoe.NET.Expressions.Tokens;

namespace Horseshoe.NET.Expressions
{
    public class Function : TokenBase
    {
        /// <summary>
        /// Used by the parsing engine to determine what type of token is being passed in if it matches (see <see cref="Is(string)")./>
        /// </summary>
        public override TokenType Type => TokenType.Word;

        /// <summary>
        /// The string value represented by the text
        /// </summary>
        public string Value => RawValue.ToUpper();

        public FunctionType FunctionType { get; }

        public Function(string rawValue) : base(rawValue)
        {
            FunctionType = GetFunctionType(rawValue);
        }

        /// <summary>
        /// Chooses an function type matching the raw input
        /// </summary>
        /// <param name="rawValue">The original text value parsed from the source text</param>
        /// <returns>A matching function type, or Undefined</returns>
        public FunctionType GetFunctionType(string rawValue)
        {
            switch (rawValue.ToUpper())
            {
                case "AND":
                    return FunctionType.And;
                case "OR":
                    return FunctionType.Or;
                case "IF":
                    return FunctionType.If;
                case "ABS":
                    return FunctionType.Abs;
                case "COS":
                    return FunctionType.Cos;
                case "SIN":
                    return FunctionType.Sin;
                case "TAN":
                    return FunctionType.Tan;
                case "SEC":
                    return FunctionType.Sec;
                case "CSC":
                    return FunctionType.Csc;
                case "COT":
                    return FunctionType.Cot;
            }

            return FunctionType.Undefined;
        }


        public override string ToString()
        {
            return $"{Type} {{ FunctionType = \"{FunctionType}\", Value = \"{RawValue}\" }}";
        }
    }
}
