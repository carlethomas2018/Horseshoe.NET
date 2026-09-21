namespace Horseshoe.NET.Expressions.Functions
{
    public class Not : FunctionBase
    {
        public bool Execute(bool arg0)
        {
            return !arg0;
        }
    }
}
