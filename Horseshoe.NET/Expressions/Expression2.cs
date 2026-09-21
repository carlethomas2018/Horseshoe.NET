using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions
{
    public class Expression2
    {
        public string RawSource { get; }

        public ExpressionType Type { get; }

        public List<Expression2> ContainedExpressions { get; }

        public Expression2(string rawSource)
        {
            RawSource = rawSource;
            ContainedExpressions = new List<Expression2>();
            Parse(out ExpressionType type);
            Type = type;
        }

        private void Parse(out ExpressionType type)
        {
            type = ExpressionType.Undefined;
            if (Lib.NumberRegex.IsMatch(RawSource))
            {
                // Handle number parsing
            }
            else if (Lib.StringRegex.IsMatch(RawSource))
            {
                // Handle string parsing
            }
            else if (Lib.KeywordRegex.IsMatch(RawSource))
            {
                // Handle keyword parsing
            }
            else if (Lib.OperatorRegex.IsMatch(RawSource))
            {
                // Handle operator parsing
            }
            else if (Lib.FunctionRegex.IsMatch(RawSource))
            {
                // Handle function parsing
            }
        }

        public bool IsToken() 
        { 
            return false;
        }
    }
}
