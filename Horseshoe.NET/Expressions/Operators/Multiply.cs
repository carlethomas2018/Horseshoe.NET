namespace Horseshoe.NET.Expressions.Operators
{
    /// <summary>
    /// Operator classes must extend <c>OperatorBase</c> to enable function parsing and evaluation.
    /// Each must contain exactly one 'Execute' method with 2 args.
    /// </summary>
    public class Multiply : OperatorBase
    {
        public override string[] Symbols => new[]
        { 
            "*",  // 002A
            "×",  // 00D7
            "·"   // 00B7
        };

        public double Execute (double arg1, double arg2)
        {
            return arg1 * arg2;
        }
    }
}
