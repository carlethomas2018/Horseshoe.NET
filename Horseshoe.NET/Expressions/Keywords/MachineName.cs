using System;

namespace Horseshoe.NET.Expressions.Keywords
{
    public class MachineName : KeywordBase
    {
        public override object Value => Environment.MachineName;
    }
}
