using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions
{
    public class ExpressionSequence : Expression2
    {
        public List<Expression2> Expressions { get; }
        public ExpressionSequence(string rawSource) : base(rawSource)   
        {
            Expressions = new List<Expression2>();
        }
    }
}
