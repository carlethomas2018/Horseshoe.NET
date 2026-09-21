namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// The type of numerical token, e.g. literal, fractional, mathematical constant, etc. 
    /// </summary>
    public enum NumberType
    {
        /// <summary>
        /// e.g. 90, -55.275
        /// </summary>
        Literal,

        /// <summary>
        /// e.g. ½
        /// </summary>
        FractionalLiteral,

        /// <summary>
        /// e.g. π
        /// </summary>
        MathematicalConstant,

        ///// <summary>
        ///// i.e. √ -> move to operator
        ///// </summary>
        //Prefix,

        /// <summary>
        /// i.e. (2)²
        /// </summary>
        Superscript,

        /// <summary>
        /// e.g. ³√27, 2π²
        /// </summary>
        Combined
    }
}
