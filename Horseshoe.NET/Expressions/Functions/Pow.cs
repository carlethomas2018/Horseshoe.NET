using System;

namespace Horseshoe.NET.Expressions.Functions
{
    public class Pow : FunctionBase
    {
        public double Execute(double arg0, double arg1) { return Math.Pow(arg0, arg1); }
    }
}
