namespace Horseshoe.NET.Expressions.Functions
{
    public class Or : FunctionBase
    {
        public bool Execute
        (
            bool arg0, 
            bool arg1,
            bool arg2 = false,
            bool arg3 = false,
            bool arg4 = false,
            bool arg5 = false,
            bool arg6 = false,
            bool arg7 = false,
            bool arg8 = false,
            bool arg9 = false,
            bool arg10 = false,
            bool arg11 = false,
            bool arg12 = false,
            bool arg13 = false,
            bool arg14 = false,
            bool arg15 = false
        )
        {
            return arg0 || arg1 || arg2 || arg3 || arg4 || arg5 || arg6 || arg7 || arg8 || arg9 || arg10 || arg11 || arg12 || arg13 || arg14 || arg15;
        }
    }
}
