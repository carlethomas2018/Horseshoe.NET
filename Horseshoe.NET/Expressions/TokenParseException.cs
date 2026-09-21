using System;

namespace Horseshoe.NET.Expressions
{
    public class TokenParseException : Exception
    {
        public TokenParseException(string message) : base(message) { }
        public TokenParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
