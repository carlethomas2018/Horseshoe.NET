namespace Horseshoe.NET.Expressions
{
    public enum FunctionType
    {
        Undefined,

        // context-dependent
        If,

        // boolean
        And,
        Or,
        IsNull,

        // date
        Now,
        Today,
        DateAdd, 

        // number
        Abs,
        Cos,
        Sin,
        Tan,
        Sec,
        Csc,
        Cot,
        DateDiff,
        Pow,
        Sqrt,

        // string
        Substring,
        Chr
    }
}
