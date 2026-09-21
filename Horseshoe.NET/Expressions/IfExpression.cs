using System;
using System.Collections.Generic;
using System.Text;

using Horseshoe.NET.Expressions.Tokens;

namespace Horseshoe.NET.Expressions
{
    public abstract class IfExpression : Expression
    {
        public IfExpression(Type returnType) : base(returnType)
        { 
        }

        public override object Evaluate()
        {
            //if (Components.Count != 3)
            //    throw new ExpressionException("This express")
            return null;
        }
    }
}
