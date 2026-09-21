using System;

namespace Horseshoe.NET.Expressions.Functions
{
    public class Exp : FunctionBase
    {
        public double Execute(double arg0) { return Math.Exp(arg0); }
    }
}
