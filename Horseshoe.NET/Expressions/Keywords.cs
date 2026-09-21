using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Horseshoe.NET.DateAndTime.DateTimeConstants;

namespace Horseshoe.NET.Expressions
{
    //public class Keywords : KeywordDefinitionBase
    //{
    //    public override List<Item> Definitions { get; } = new List<Item>
    //    {
    //        new KeywordConstantItem("Null", null),
    //        new KeywordConstantItem("HighDate", PreferBusinessDates ? HighDate : DateTime.MaxValue),
    //        new KeywordConstantItem("LowDate", PreferBusinessDates ? LowDate : DateTime.MinValue),
    //        new FunctionItem("Today", typeof(DateTime), 0, 0, args => DateTime.Today),
    //        new FunctionItem("Now", typeof(DateTime), 0, 0, args => DateTime.Now),
    //        new FunctionItem("And", typeof(bool), 2, 16, args => args.All(obj => (bool)obj)),
    //        new FunctionItem("Or", typeof(bool), 2, 16, args => args.Any(obj => (bool)obj)),
    //        new FunctionItem("If", 3, 3, args => (bool)args[0] ? args[1] : args[3]),
    //        new FunctionItem("IsNull", typeof(bool), 1, 1, args => args[0] == null),
    //    };
    //}
}
