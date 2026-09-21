using System;

namespace Horseshoe.NET.Expressions.Keywords
{
    public class User : KeywordBase
    {
        public override object Value => Environment.UserName;
    }
}
