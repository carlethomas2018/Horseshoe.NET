using System;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// Applies to token types that represent a value e.g. Strings, Dates, etc.
    /// </summary>
    public interface IValueToken
    {
        /// <summary>
        /// The value represented by the token
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The type of value represented by the token
        /// </summary>
        Type ValueType { get; }
    }
}
