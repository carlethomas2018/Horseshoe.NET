using System;

using static Horseshoe.NET.DateAndTime.DateTimeConstants;

namespace Horseshoe.NET.Expressions.Keywords
{
    public class _LowDate : KeywordBase
    {
        public override string Name => "LowDate";
        public override object Value => PreferBusinessDates ? LowDate : DateTime.MinValue;
    }
}
