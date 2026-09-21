using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Horseshoe.NET.Expressions
{
    internal class Lib
    {
        internal static Regex NumberRegex { get; } = new Regex(@"^[0-9]+$");
        internal static Regex StringRegex { get; } = new Regex(@"^(""[^""]*"")|('[^']*')$");
        internal static Regex KeywordRegex { get; } = new Regex(@"^[A-Z_]+$", RegexOptions.IgnoreCase);
        internal static Regex OperatorRegex { get; } = new Regex(@"^[!%^~&$#@*+=?<>\\\/|\\-]{1,3}$");
        internal static Regex FunctionRegex { get; } = new Regex(@"^[A-Z_]+\\(.*\\)$", RegexOptions.IgnoreCase);
    }
}
