using System;

namespace Horseshoe.NET.Expressions.Functions
{
    public class Now : FunctionBase
    {
        public DateTime Execute() { return DateTime.Now; }
    }
}
