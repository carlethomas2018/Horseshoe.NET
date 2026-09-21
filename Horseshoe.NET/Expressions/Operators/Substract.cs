namespace Horseshoe.NET.Expressions.Operators
{
    /// <summary>
    /// Operator classes must extend <c>OperatorBase</c> to enable function parsing and evaluation.
    /// Each must contain exactly one 'Execute' method with 2 args.
    /// </summary>
    public class Substract : OperatorBase
    {
        public override string[] Symbols => new[]
        { 
            "-",  // 002D
            "−"   // 2212
        };

        public double Execute (double arg1, double arg2)
        {
            return (arg1 == double.MinValue || arg1 == double.NaN ? 0.0 : arg1) - arg2;
        }
    }
}
