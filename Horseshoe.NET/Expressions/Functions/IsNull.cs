namespace Horseshoe.NET.Expressions.Functions
{
    public class IsNull : FunctionBase
    {
        public bool Execute(object arg0)
        {
            return arg0 == null;
        }
    }
}
