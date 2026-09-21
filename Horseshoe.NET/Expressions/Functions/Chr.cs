namespace Horseshoe.NET.Expressions.Functions
{
    public class Chr : FunctionBase
    {
        public string Execute(int arg0)
        {
            return new string((char)arg0, 1);
        }
    }
}
