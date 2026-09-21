using System;

namespace Horseshoe.NET.Expressions.Functions
{
    public class Sqrt : FunctionBase
    {
        public double Execute(double arg0) { return Math.Sqrt(arg0); }
    }
}
