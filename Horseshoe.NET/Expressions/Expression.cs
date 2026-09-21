using System;
using System.Collections.Generic;
using System.Text;

using Horseshoe.NET.Expressions.Tokens;

namespace Horseshoe.NET.Expressions
{
    public abstract class Expression : List<TokenBase>
    {
        public Type ReturnType { get; } = typeof(void);

        public List<Expression> Components { get; }

        public abstract List<TokenType> Signature { get; }

        public Expression(Type returnType = null) 
        { 
            if (returnType != null)
                ReturnType = returnType;
            Components = new List<Expression>();
        }

        public abstract object Evaluate();
    }
}
