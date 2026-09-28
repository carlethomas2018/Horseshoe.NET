using System;

using Horseshoe.NET.Expressions.Tokens;

namespace Horseshoe.NET.Expressions.TokenGroups
{
    /// <summary>
    /// A number or mathematical constant
    /// </summary>
    public class TempNumber : TokenBase, IValueToken
    {
        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Number;

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier => "NB";

        /// <inheritdoc cref="IValueToken.ReturnValue"/>
        public object ReturnValue { get; }

        /// <inheritdoc cref="IValueToken.ReturnType"/>
        public Type ReturnType => typeof(double);

        /// <summary>
        /// Constructor used by token groups when calculating mathematical expressions in order of operations
        /// </summary>
        /// <param name="value">A double value</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the temp token (and first token in group) in the original raw input, default is <c>-1</c></param>
        public TempNumber(double value, int tokenPos = -1) : base(string.Empty, tokenPos)
        {
            ReturnValue = value;
        }
    }
}
