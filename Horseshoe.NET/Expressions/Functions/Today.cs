using System;

namespace Horseshoe.NET.Expressions.Functions
{
    public class Today : FunctionBase
    {
        public DateTime Execute() { return DateTime.Today; }
    }
}
