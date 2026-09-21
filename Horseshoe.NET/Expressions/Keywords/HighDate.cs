using System;

using Horseshoe.NET.DateAndTime;

namespace Horseshoe.NET.Expressions.Keywords
{
    public class HighDate : KeywordBase
    {
        public override object Value => DateTimeConstants.PreferBusinessDates ? DateTimeConstants.HighDate : DateTime.MaxValue;
    }
}
