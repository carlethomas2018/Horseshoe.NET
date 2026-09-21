using System;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Represents number e.g. literal, fractional, mathematical constant, etc. 
    /// </summary>
    public abstract class NumericBase : TokenBase, IValueToken
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        /// <summary>
        /// The type of numerical token, e.g. literal, fractional, mathematical constant, etc. 
        /// </summary>
        public abstract NumberType NumberType { get; }

        /// <inheritdoc cref="IValueToken.Value"/>
        public object Value => GetValue();

        /// <inheritdoc cref="IValueToken.ValueType"/>
        public Type ValueType => typeof(double);

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        internal NumericBase() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public NumericBase(string rawValue, int tokenPos = -1) : base(rawValue, tokenPos: tokenPos)
        {
        }

        public abstract double GetValue();
    }
}
