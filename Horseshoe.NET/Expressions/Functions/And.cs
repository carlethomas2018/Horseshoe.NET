namespace Horseshoe.NET.Expressions.Functions
{
    public class And : FunctionBase
    {
        public bool Execute
        (
            bool arg0, 
            bool arg1,
            bool arg2 = true,
            bool arg3 = true,
            bool arg4 = true,
            bool arg5 = true,
            bool arg6 = true,
            bool arg7 = true,
            bool arg8 = true,
            bool arg9 = true,
            bool arg10 = true,
            bool arg11 = true,
            bool arg12 = true,
            bool arg13 = true,
            bool arg14 = true,
            bool arg15 = true
        )
        {
            return arg0 && arg1 && arg2 && arg3 && arg4 && arg5 && arg6 && arg7 && arg8 && arg9 && arg10 && arg11 && arg12 && arg13 && arg14 && arg15;
        }
    }
}
